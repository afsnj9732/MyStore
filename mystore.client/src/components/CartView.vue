<template>
    <nav>
        <NavBar />
    </nav>
    <div v-if="data">
        <a v-for="cartItem in data" :key="cartItem.productId">
            <p>{{cartItem.productId}}</p>
            <p>{{cartItem.productName}}</p>
            <p>{{cartItem.price}}</p>
            <p>{{cartItem.quantity}}</p>
        </a>
    </div>
</template>

<script setup>
    import NavBar from './NavBar.vue'
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const data = ref(null);

    const getCartData = () => {
        axios.get("https://localhost:7266/api/Cart/get/")
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
