<template>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container-fluid">
                <router-link class="navbar-brand" to="/">MyStore</router-link>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <router-link class="nav-link text-dark" to="/">首頁</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link class="nav-link text-dark" to="/products">商品</router-link>
                        </li>

                        <li v-if="isLogIn" class="nav-item">
                            <button class="nav-link text-dark" @click="logout()">登出</button>
                        </li>
                        <li v-if="isLogIn" class="nav-item  d-flex align-items-center">
                            <router-link class="nav-link text-dark" to="/cart">購物車</router-link>
                            <a>{{data}}</a>
                        </li>
                        <li v-if="isLogIn" class="nav-item">
                            <router-link class="nav-link text-dark" to="/orders">訂單</router-link>
                        </li>

                        <li v-if="!isLogIn" class="nav-item">
                            <router-link class="nav-link text-dark" to="/login">登入</router-link>
                        </li>
                        <li v-if="!isLogIn" class="nav-item">
                            <router-link class="nav-link text-dark" to="/register">註冊</router-link>
                        </li>
                        <li v-show="role === 'Admin'" class="nav-item">
                            <router-link class="nav-link text-dark" to="/dashboard">後臺</router-link>
                        </li>

                    </ul>
                </div>
            </div>
        </nav>
    </header>
</template>

<script setup>
    import { ref,onMounted } from 'vue';
    import axios from 'axios'
    import { useRouter } from 'vue-router'
    import { jwtDecode } from 'jwt-decode';


    const token = sessionStorage.getItem('jwtToken');
    const role = ref(null);

    const isLogIn = ref(!!token);
    const router = useRouter();
    const data = ref(null);

    const logout = () => {
        sessionStorage.removeItem("jwtToken");
        alert("登出成功");
        isLogIn.value = !isLogIn.value;
        router.push('/');
    }

    const getCartCount = () => {
        if (token) {
            axios.get("https://localhost:7266/api/Cart/count", { headers: { "Authorization": `Bearer ${token}` } })
                .then(response => {
                    data.value = response.data;
                })
                .catch(error => {
                    console.error(error);
                });
        }
    }

    onMounted(() => {
        getCartCount();
        if (token) {
            const decoded = jwtDecode(token);
            if (decoded) {
                role.value = decoded.role;
            }
        }
    })
</script>