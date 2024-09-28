<template>
    <nav>
        <NavBar />
    </nav>
    <div v-if="data">
        <a v-for="cartItem in data" :key="cartItem.productId">
            商品名稱:<p>{{cartItem.productName}}</p>
            商品價格:<p>{{cartItem.price}}</p>
            購買數量:
            <input type="number" v-model="cartItem.quantity" @blur="updateQuantity(cartItem)">
        </a>
    </div>
</template>

<script setup>
    import NavBar from './NavBar.vue'
    import { ref, onMounted } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router';

    const router = useRouter();
    const quantity = ref(null);
    const data = ref(null);
    const token = localStorage.getItem('jwtToken');

    const updateQuantity = (item) => {
        if (item.quantity < 1) {
            item.quantity = 1;
        } else if (item.quantity > item.productStockQuantity) {
            item.quantity = item.productStockQuantity;
        } else {
            axios.post("https://localhost:7266/api/Cart/update",
                {
                    "ProductId": item.productId,
                    "Quantity": item.quantity
                },
                { headers: { "Authorization": `Bearer ${token}` } })
                .then(response => {
                    router.go(0);
                })
                .catch(error => {
                    console.error(error);
                });
        }
    }

    const getCartData = () => {
        axios.get("https://localhost:7266/api/Cart/get", { headers: { "Authorization": `Bearer ${token}` } })
            .then(response => {
                data.value = response.data;
            })
            .catch(error => {
                console.error(error);
            });
    }

    onMounted(() => {
        getCartData();
    });
</script>
