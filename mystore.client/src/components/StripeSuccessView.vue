<template>
    <div v-if="data">
        <table class="table">
            <thead>
                <tr>
                    <th>訂購日期</th>
                    <th>總金額</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <div type="button" >
                            {{data.orderDate}}
                        </div>
                        <div class="" :id="`${data.orderDate}`">
                            <div class="card card-body">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>商品名稱</th>
                                            <th>商品數量</th>
                                            <th>商品單價</th>
                                        </tr>
                                    </thead>
                                    <tbody v-for="orderItem in data.tOrderItems" :key="data.productId">
                                        <tr>
                                            <td>{{orderItem.productName}}</td>
                                            <td>{{orderItem.quantity}}</td>
                                            <td>{{orderItem.price}}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td>{{data.totalPrice}}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script setup>
    import { ref, onMounted, inject } from 'vue';
    import axios from 'axios';

        const updateCartItem = inject("getNavCartItemCount");
    const data = ref(null);
    const token = inject("jwtToken")
    const urlParams = new URLSearchParams(window.location.search);
    const sessionId = urlParams.get('session_id');



    const fullfillOrder = () => {
        axios.post(import.meta.env.VITE_API_LOCAL + "api/Order/fullfill",
            {"SessionId":sessionId},
            {
                headers: {
                    "Authorization": `Bearer ${token.value}`,
                    'Ocp-Apim-Subscription-Key': import.meta.env.VITE_API_KEY
                }
            })
            .then(response => {
                data.value = response.data;
            })
            .catch(error => {
                console.error(error);

            })
            .finally(() => {
                updateCartItem.value();
            });
    }

    onMounted(() => {
        fullfillOrder();
    });
</script>
