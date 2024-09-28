<template>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container-fluid">
                <a class="navbar-brand">MyStore</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <router-link class="nav-link text-dark" to="/">Home</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link class="nav-link text-dark" to="/products">商品</router-link>
                        </li>

                        <li v-if="isLogIn" class="nav-item">
                            <button class="nav-link text-dark" @click="logout()">登出</button>
                        </li>
                        <li v-if="isLogIn" class="nav-item  d-flex align-items-center">
                            <router-link class="nav-link text-dark" to="/cart">購物車</router-link>
                            <a id="CartItemCount"></a>
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


                    </ul>
                </div>
            </div>
        </nav>
    </header>
</template>

<script setup>
    import { ref } from 'vue';
    import axios from 'axios'
    import { useRouter } from 'vue-router'

    const token = localStorage.getItem('jwtToken');
    const isLogIn = ref(!!token);
    const router = useRouter();

    const logout = () => {
        localStorage.removeItem("jwtToken");
        alert("登出成功");
        isLogIn.value = !isLogIn.value;
        router.push('/');
    }

</script>