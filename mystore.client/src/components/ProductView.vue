<template>
    <div v-if="loading" class="d-flex justify-content-center p-2 m-2" >
        <div class="spinner-border" style="width: 5rem; height: 5rem;" role="status">
        </div>
    </div>
    <div v-if="loadFailed" class="d-flex justify-content-center p-2 fw-bold fs-5" >資料讀取失敗，請嘗試重新整理</div>
    <div v-if="data&&data.products">
        <nav class="navbar ">
            <div class="container justify-content-end">
                <span>
                    <span class="navbar-brand fw-bold">商品頁數:</span>
                    <button v-for="page in data.totalPage" :key="page" class="btn btn-outline-primary" @click="getProductData(page)">
                        {{page}}
                    </button>
                </span>
                <form class="d-flex">
                    <input class="form-control me-2 border border-3" type="search" v-model="searchWord" aria-label="Search">
                    <button class="btn btn-outline-success" type="button" @click="getProductData(1)">Search</button>
                </form>
            </div>
        </nav>
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
    import { ref, onMounted,inject } from 'vue';
    import axios from 'axios';

    const data = ref(null);
    const searchWord = ref(null);
    const loading = ref(true);
    const loadFailed = ref(false);

    const getProductData = (page) => {
        if (searchWord.value && searchWord.value.trim() == "") {
            searchWord.value = null;
            alert("請輸入查詢數值");
            return;
        }
        axios.get(import.meta.env.VITE_API_LOCAL + "api/Product/list",
            {
                params: {
                    "Page": page,
                    "SearchWord": searchWord.value ? searchWord.value.trim() : null
                    //參數可以接受null，但不接受""空字串
                },
                headers: {
                    'Ocp-Apim-Subscription-Key': import.meta.env.VITE_API_KEY
                }
            })
            .then(response => {
                data.value = response.data;
                loadFailed.value = false;
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
