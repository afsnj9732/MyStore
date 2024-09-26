<template>
    <div>
        <div v-if="data&&data.products">
            <a v-for="product in data.products" :key="product.productId">
                <p>{{product.productId}}</p>
            </a>
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const data = ref(null);

    onMounted(() => {
        axios.get("https://localhost:7266/api/Product/list/1")
            .then(response => {
                data.value = response.data;
            })
            .catch(error => {
                console.error(error);
            });
    });</script>
