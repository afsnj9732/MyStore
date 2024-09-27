<template>
        <div v-if="data&&data.products">
            <button v-for="page in data.totalPage" :key="page" @click="getProductData(page)">
                {{page}}
            </button>
            <a v-for="product in data.products" :key="product.productId">
                <p>{{product.name}}</p>
                <p>{{product.description}}</p>
                <p>{{product.price}}</p>
                <img :src="`/images/${product.imageUrl}.jpg`" width="200" height="200">
                <p>{{product.stockQuantity}}</p>
                <router-link :to="`/products/detail/${product.productId}`">詳細資訊</router-link>
            </a>
        </div>
</template>

<script setup>
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
