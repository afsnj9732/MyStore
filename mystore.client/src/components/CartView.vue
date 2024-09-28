<template>
    <nav>
        <NavBar />
    </nav>
    <div v-if="data">
        <span v-for="cartItem in data.cartItems" :key="cartItem.productId">
            商品名稱:<span>{{cartItem.productName}}</span>
            商品價格:<span>{{cartItem.price}}</span>
            購買數量:
            <input type="number" v-model="cartItem.quantity" @blur="updateQuantity(cartItem)">
            <button type="button" @click="deleteItem(cartItem)">移除</button><br />
        </span>

        <br />
        總金額:{{data.totalPrice}}
    </div>
    <br />
    <br />
    <button @click="placeOrder">訂購</button>

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


    const placeOrder = (stripeToken) => {
        axios.post("https://localhost:7266/api/Order/place/" + stripeToken,
            {},
            { headers: { "Authorization": `Bearer ${token}` } })
            .then(response => {
                router.push('/orders');
            })
            .catch(error => {
                console.error(error);
            });
    }

    const deleteItem = (item) => {
        axios.post("https://localhost:7266/api/Cart/delete/" + item.productId,
            {},
            { headers: { "Authorization": `Bearer ${token}` } })
            .then(response => {
                alert("移除成功");
                router.go(0);
            })
            .catch(error => {
                console.error(error);
            });
    }

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
