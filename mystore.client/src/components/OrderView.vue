<template>
    <nav>
        <NavBar />
    </nav>
    <div v-if="data">
        <div v-for="order in data" :key="order.orderDate">
            訂購日期:<p>{{order.orderDate}}</p><br />
            <span v-for="orderItem in order.tOrderItems" :key="orderItem.productId">
                商品名稱:<span>{{orderItem.productName}}</span><br/>
                商品數量:<span>{{orderItem.quantity}}</span><br />
            </span>
            總金額:<p>{{order.totalPrice}}</p><br />
        </div>
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
