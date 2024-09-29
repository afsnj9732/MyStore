<template>
    <nav>
        <NavBar />
    </nav>
    <div v-if="data&&data.products">
        <div class="btn-toolbar" role="toolbar">
            <h4 class="m-1 p-1">商品頁數:</h4>
            <button v-for="page in data.totalPage" :key="page" class="btn btn-outline-primary" @click="getProductData(page)">
                {{page}}
            </button>
        </div>
        <div v-for="product in data.products" :key="product.productId">
            <table class="table">
                <thead>
                    <tr>
                        <th scope="col" width="20%">商品名稱</th>
                        <th scope="col" width="20%">商品價格</th>
                        <th scope="col" width="20%">商品圖片</th>
                        <th scope="col" width="20%">剩餘庫存</th>
                        <th scope="col" width="20%"></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>{{product.name}}</td>
                        <td>{{product.price}}</td>
                        <td>
                            <img :src="`/images/${product.imageUrl}.jpg`" class="img-fluid"  style="width:100px">
                        </td>
                        <td>{{product.stockQuantity}}</td>
                        <td>
                            <router-link class="btn btn-primary" :to="`/products/detail/${product.productId}`">詳細資訊</router-link>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script setup>
    import NavBar from './NavBar.vue'
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const data = ref(null);

    const getProductData = (page) => {
        axios.get("https://localhost:7266/api/Product/list/"+page)
            .then(response => {
                data.value = response.data;
            })
            .catch(error => {
                console.error(error);
            });
    }

    onMounted(() => {
        getProductData(1);
    });
</script>
