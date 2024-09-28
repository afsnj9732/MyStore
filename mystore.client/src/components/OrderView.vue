<template>
    <nav>
        <NavBar />
    </nav>
    <div v-if="data">
        <a v-for="orderItem in data" :key="orderItem.orderDate">
            訂購日期:<p>{{orderItem.orderDate}}</p>
            總金額:<p>{{orderItem.totalPrice}}</p>
        </a>
    </div>
</template>

<script setup>
    import NavBar from './NavBar.vue'
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const data = ref(null);
    const token = localStorage.getItem('jwtToken');

    const getOrderData = () => {
        axios.get("https://localhost:7266/api/Order/get/", { headers: { "Authorization": `Bearer ${token}` } })
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
