<template>
    <nav>
        <NavBar />
    </nav>
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
                        <button v-show="data.stockQuantity > 0" type="button" class="btn btn-primary p-2" @click="addProductToCart()">加入購物車</button>
                        <router-link class="btn btn-secondary p-2" to="/products">返回商品列</router-link>
                    </span>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
    import NavBar from './NavbarView.vue'
    import { ref, onMounted } from 'vue';
    import { useRoute,useRouter } from 'vue-router';
    import axios from 'axios';


    const quantity = ref(1);
    const route = useRoute();
    const router = useRouter();
    const data = ref(null);
    const token = sessionStorage.getItem("jwtToken");
    const productId = route.params.productId;

    const validateQuantity = () => {
        if (quantity.value < 1) {
            quantity.value = 1;
        } else if (quantity.value > data.value.stockQuantity){
            quantity.value = data.value.stockQuantity;
        }
    }

    const addProductToCart = () => {
        if (token) {
            axios.post("https://mystoreserverapi.azure-api.net/api/Cart/add",
                {
                    "ProductId": productId,
                    "Quantity": quantity.value
                },
                {
                    headers: {
                        "Authorization": `Bearer ${token}`,
                        'Ocp-Apim-Subscription-Key': 'ffbbcbcdf59542a7bec95f9ea8de0805'
                    }
                }
            )
                .then(response => {
                    alert("加入購物車成功");
                    router.go(0);
                })
                .catch(error => {
                    alert("加入購物車失敗");
                });
        }else{
            router.push('/login');

        }
    }

    const getProductData = () => {
        axios.get("https://mystoreserverapi.azure-api.net/api/Product/detail/" + productId,
            {
                headers: {
                    'Ocp-Apim-Subscription-Key': 'ffbbcbcdf59542a7bec95f9ea8de0805'
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