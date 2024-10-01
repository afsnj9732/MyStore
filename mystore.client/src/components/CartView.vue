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
        <br />
        <br />
        <button v-if="data && data.totalPrice > 0" class="btn btn-primary" @click="placeOrder">訂購</button>
    </div>
</template>
<script setup>
    import NavBar from './NavbarView.vue'
    import { ref, onMounted } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router';


    const router = useRouter();
    const quantity = ref(null);
    const data = ref(null);
    const token = sessionStorage.getItem('jwtToken');




    const deleteItem = (item) => {
        axios.post("https://mystoreserverapi.azure-api.net/api/Cart/delete/" + item.productId,
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
            axios.post("https://mystoreserverapi.azure-api.net/api/Cart/update",
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
        axios.get("https://mystoreserverapi.azure-api.net/api/Cart/get", { headers: { "Authorization": `Bearer ${token}` } })
            .then(response => {
                data.value = response.data;
            })
            .catch(error => {
                console.error(error);
            });
    }


    const placeOrder = () => {
        const handler = window.StripeCheckout.configure({
            key: 'pk_test_51Pesm02La0H5PIYutJuIiEkUXFXagRryVad9x9fhP4WDFqjjzPhO0shoZqRQMhdXxvtEfGxmb7gruzpjkVCKDXA00012AZZjHg',
            locale: 'auto',
            token: (stripeToken) => {

                axios.post("https://mystoreserverapi.azure-api.net/api/Order/place/" + stripeToken.id,
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
        });

        handler.open({
            name: 'Stripe金流API測試',
            description: '測試卡號4242 4242 4242 4242',
            amount: data.value.totalPrice * 100,
            currency: 'twd'
        });
    };

    const loadStripeScript = () => {
        return new Promise((resolve) => {
            const script = document.createElement('script');
            script.src = 'https://checkout.stripe.com/checkout.js';
            script.onload = () => {
                resolve();
            };
            document.body.appendChild(script);
        });
    };


    onMounted(async () => {
        getCartData();
        await loadStripeScript();
    });
</script>
