<template>
    <nav>
        <NavBar />
    </nav>
    <div v-if="data">
        <a v-for="cartItem in data" :key="cartItem.productId">
            商品名稱:<p>{{cartItem.productName}}</p>
            商品價格:<p>{{cartItem.price}}</p>
            購買數量:<p>{{cartItem.quantity}}</p>
        </a>
    </div>
</template>

<script setup>
    import NavBar from './NavBar.vue'
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const data = ref(null);
    const token = localStorage.getItem('jwtToken');

    const getCartData = () => {
        axios.get("https://localhost:7266/api/Cart/get", { headers: { "Authorization": `Bearer ${token}` } })
            .then(response => {
                data.value = response.data;
            })
            .catch(error => {
                console.error(error);
            });
    }

    onMounted(() => {
        getCartData();
    });
</script>
