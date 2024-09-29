<template>
    <nav>
        <NavBar />
    </nav>
    <div v-if="data">
        <table class="table">
            <thead>
                <tr>
                    <th>商品名稱</th>
                    <th>商品價格</th>
                    <th>購買數量</th>
                    <th></th>
                </tr>
            </thead>
            <tbody v-for="cartItem in data.cartItems" :key="cartItem.productId">
                <tr>
                    <td>{{cartItem.productName}}</td>
                    <td>{{cartItem.price}}</td>
                    <td>
                        <input type="number" v-model="cartItem.quantity" @blur="updateQuantity(cartItem)">
                    </td>
                    <td>
                        <button type="button" class="btn btn-danger" @click="deleteItem(cartItem)">移除</button><br />

                    </td>
                </tr>
            </tbody>
        </table>
        <br />
        總金額:{{data.totalPrice}}
    </div>
    <br />
    <br />
    <button class="btn btn-primary"  @click="placeOrder">訂購</button>

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


    const placeOrder = () => {
        axios.post("https://localhost:7266/api/Order/place/",
            {},
            { headers: { "Authorization": `Bearer ${token}` } })
            .then(response => {
                alert("訂購成功")
                router.push('/orders');
            })
            .catch(error => {
                alert(error.response.data)
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
