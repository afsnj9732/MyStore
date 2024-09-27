<template>
    <nav>
        <NavBar />
    </nav>
    <div v-if="data">
        <a v-for="orderItem in data" :key="orderItem.productId">
            <p>{{orderItem.orderDate}}</p>
            <p>{{orderItem.totalPrice}}</p>
        </a>
    </div>
</template>

<script setup>
    import NavBar from './NavBar.vue'
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const data = ref(null);

    const getOrderData = () => {
        axios.get("https://localhost:7266/api/")
            .then(response => {
                data.value = response.data;
            })
            .catch(error => {
                console.error(error);
            });
    }

    onMounted(() => {
        getOrderData();
    });
</script>
