<template>
    <nav>
        <NavBar />
    </nav>
    <div v-if="data">
        <div class="card" style="width:30rem" >
            <img :src="`/images/${data.imageUrl}.jpg`" class="img-fluid">
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
                <button type="button" class="btn btn-primary" @click="addProductToCart()">加入購物車</button>
                <br />
                <router-link class="btn btn-secondary" to="/products">返回商品列</router-link>
            </div>
        </div>
    </div>
</template>

<script setup>
    import NavBar from './NavBar.vue'
    import { ref, onMounted } from 'vue';
    import { useRoute,useRouter } from 'vue-router';
    import axios from 'axios';


    const quantity = ref(1);
    const route = useRoute();
    const router = useRouter();
    const data = ref(null);
    const token = localStorage.getItem("jwtToken");
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
            axios.post("https://localhost:7266/api/Cart/add",
                {
                    "ProductId": productId,
                    "Quantity": quantity.value
                },
                { headers: { "Authorization": `Bearer ${token}` } }
            )
                .then(response => {
                    alert("加入購物車成功");
                    router.go(0);
                })
                .catch(error => {
                    alert("加入購物車失敗");
                });
        }
    }

    const getProductData = () => {
        axios.get("https://localhost:7266/api/Product/list/detail/" + productId)
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