<template>
    <div class="container">
        <div v-if="data" class="row justify-content-center">
            <div class="card" style="width: 25rem;">
                <img :src="`/images/${data.imageUrl}.jpg`" class="card-img-top img-fluid">
                <div class="card-body">
                    <h5 class="card-title">商品名稱:{{data.name}}</h5>
                    <p class="card-text">商品說明:{{data.description}}</p>
                    <p>商品價格:{{data.price}}</p>
                    <p>商品庫存:{{data.stockQuantity}}</p>
                    <span v-if="token">
                        <span>購買數量:</span>
                        <input type="number" v-model="quantity" @blur="validateQuantity()">
                    </span>
                    <br />
                    <br />
                    <span class="d-flex justify-content-between">
                        <button  v-show="data.stockQuantity > 0" type="button" class="btn btn-primary p-2" @click="addProductToCart()">加入購物車</button>
                        <router-link class="btn btn-secondary p-2" to="/products">返回商品列</router-link>
                    </span>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted,inject } from 'vue';
    import { useRoute,useRouter } from 'vue-router';
    import axios from 'axios';


    const quantity = ref(1);
    const route = useRoute();
    const router = useRouter();
    const data = ref(null);
    const token = inject("jwtToken");
    const updateCartItem = inject("getNavCartItemCount");
    const productId = route.params.productId;


    const validateQuantity = () => {
        if (quantity.value < 1) {
            quantity.value = 1;
        } else if (quantity.value > data.value.stockQuantity){
            quantity.value = data.value.stockQuantity;
        }
    }

    const addProductToCart = () => {
        if (token.value) {
            axios.post(import.meta.env.VITE_API_LOCAL+"api/Cart/add",
                {
                    "ProductId": productId,
                    "Quantity": quantity.value
                },
                {
                    headers: {
                        "Authorization": `Bearer ${token.value}`,
                        'Ocp-Apim-Subscription-Key': import.meta.env.VITE_API_KEY
                    }
                }
            )
                .then(response => {
                    alert("加入購物車成功");
                    updateCartItem.value();
                    //router.go(0);
                })
                .catch(error => {
                    alert("加入購物車失敗");
                })
                .finally(() => {

                });
        }else{
            router.push('/login');

        }
    }

    const getProductData = () => {
        axios.get(import.meta.env.VITE_API_LOCAL+"api/Product/detail/" + productId,
            {
                headers: {
                    'Ocp-Apim-Subscription-Key': import.meta.env.VITE_API_KEY
                }
            }
        )
            .then(response => {
                data.value = response.data;
            })
            .catch(error => {
                console.error(error);
            });
    }

    onMounted(() => {
        getProductData();
    });
</script>