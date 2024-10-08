<template>
    <div v-if="data&&data.products">
        <div>
            <table class="table">
                <thead>
                    <tr>
                        <th scope="col" width="20%">商品名稱</th>
                        <th scope="col" width="20%">商品價格</th>
                        <th scope="col" width="20%">商品圖片</th>
                        <th scope="col" width="20%">庫存數量</th>
                    </tr>
                </thead>
                <tbody v-for="product in data.products" :key="product.productId">
                    <tr>
                        <td>{{product.name}}</td>
                        <td>{{product.price}}</td>
                        <td>
                            <img :src="`/images/${product.imageUrl}.jpg`" class="img-fluid" style="width:100px">
                        </td>
                        <td>
                            <input type="number" v-model="product.stockQuantity" @blur="updateProduct(product)">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const data = ref(null);
    const token = sessionStorage.getItem('jwtToken');

    const updateProduct = (item) => {
        axios.post(import.meta.env.VITE_API_LOCAL+"api/Product/update",
            {
                "StockQuantity": item.stockQuantity,
                "ProductId":item.productId
            },
            {
                headers: {
                    "Authorization": `Bearer ${token}`,
                    'Ocp-Apim-Subscription-Key': import.meta.env.VITE_API_KEY
                }
            })
            .then(response => {
                alert("修改成功")
            })
            .catch(error => {
                console.error(error);
            });
    }

    const getProductData = () => {
        axios.get(import.meta.env.VITE_API_LOCAL+"api/Product/list",
            {
                params: {
                    "Page": 0,
                    "SearchWord": null
                },
                headers: {
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
        getProductData();
    });
</script>
