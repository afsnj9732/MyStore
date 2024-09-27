<template>
    <nav>
        <NavBar />
    </nav>
    <div v-if="data">
        <p>{{data.name}}</p>
        <p>{{data.description}}</p>
        <p>{{data.price}}</p>
        <img :src="`/images/${data.imageUrl}.jpg`" width="200" height="200">
        <p>{{data.stockQuantity}}</p>
        <router-link to="/products">返回商品列表</router-link>
    </div>
</template>

<script setup>
    import NavBar from './NavBar.vue'
    import { ref, onMounted } from 'vue';
    import { useRoute } from 'vue-router';
    import axios from 'axios';

    const route = useRoute();
    const data = ref(null);


    const getProductData = (id) => {
        axios.get("https://localhost:7266/api/Product/list/detail/"+id)
            .then(response => {
                data.value = response.data;
            })
            .catch(error => {
                console.error(error);
            });
    }

    onMounted(() => {
        const productId = route.params.productId;
        getProductData(productId);
    });
</script>