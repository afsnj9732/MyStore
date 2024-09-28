<template>
    <nav>
        <NavBar />
    </nav>
    <div v-if="data">
        <p>{{data.name}}</p>
        <p>{{data.description}}</p>
        <p>{{data.price}}</p>
        <img :src="`/images/${data.imageUrl}.jpg`" width="200" height="200">
        <p>{{data.stockQuantity}}</p>
        <div v-if="token">
            購買數量:<input type="number" v-model="quantity" @input="validateQuantity()">
            <button type="button" @click="addProductToCart()">加入購物車</button>
        </div>
        <router-link to="/products">返回商品列表</router-link>
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
                    //router.go(0);
                    //if (error.response) {
                    //    alert(error.response.data);
                    //} else {
                    //    alert("資料格式不符規定，請重新輸入");
                    //}
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