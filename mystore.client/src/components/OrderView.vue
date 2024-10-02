<template>
    <nav>
        <Navbar />
    </nav>
    <div v-if="data">
        <table class="table">
            <thead>
                <tr>
                    <th>訂購日期(點擊顯示訂單項目)</th>
                    <th>總金額</th>
                </tr>
            </thead>
            <tbody v-for="order in data" :key="order.orderDate">
                <tr>
                    <td>
                        <button type="button" class="accordion-button" data-bs-toggle="collapse" :href="`#${order.orderDate}`" role="button" aria-expanded="false" :aria-controls="`${order.orderDate}`">
                            {{order.orderDate}}
                        </button>
                        <div class="collapse multi-collapse" :id="`${order.orderDate}`">
                            <div class="card card-body">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>商品名稱</th>
                                            <th>商品數量</th>
                                        </tr>
                                    </thead>
                                    <tbody v-for="orderItem in order.tOrderItems" :key="orderItem.productId">
                                        <tr>
                                            <td>{{orderItem.productName}}</td>
                                            <td>{{orderItem.quantity}}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td>{{order.totalPrice}}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script setup>
    import Navbar from './NavbarView.vue'
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const data = ref(null);
    const token = sessionStorage.getItem('jwtToken');

    const getOrderData = () => {
        axios.get(import.meta.env.VITE_API_LOCAL + "api/Order/get/",
            {
                headers: {
                    "Authorization": `Bearer ${token}`,
                    'Ocp-Apim-Subscription-Key': import.meta.env.VITE_API_KEY
                }
            })
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
