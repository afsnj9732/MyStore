<template>
    <nav>
        <Navbar />
    </nav>
    <div v-if="loading">資料讀取中...</div>
    <div v-if="loadFailed">讀取失敗，請嘗試重新整理</div>
    <div v-if="data&&data.products">
        <div class="btn-toolbar" role="toolbar">
            <h4 class="m-1 p-1">商品頁數:</h4>
            <button v-for="page in data.totalPage" :key="page" class="btn btn-outline-primary" @click="getProductData(page)">
                {{page}}
            </button>
            <input type="text" v-model="searchWord" />
            <button type="button" class="btn btn-primary" @click="getProductData(1)">查詢</button>
        </div>
        <div>
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
                <tbody v-for="product in data.products" :key="product.productId">
                    <tr>
                        <td>{{product.name}}</td>
                        <td>{{product.price}}</td>
                        <td>
                            <img :src="`/images/${product.imageUrl}.jpg`" class="img-fluid" style="width:100px">
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
    import Navbar from './NavbarView.vue'
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const data = ref(null);
    const searchWord = ref(null);
    const loading = ref(true);
    const loadFailed = ref(false);

    const getProductData = (page) => {
        axios.get(import.meta.env.VITE_API_LOCAL+"api/Product/list",
            {
                params: {
                        "Page": page,
                    "SearchWord": searchWord.value ? searchWord.value : null
                    //參數可以接受null，但不接受""空字串
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
                loadFailed.value = true;
            })
            .finally(() => {
                loading.value = false;
            });
    }

    onMounted(() => {
        getProductData(1);
    });
</script>
