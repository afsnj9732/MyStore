<script setup lang="ts">
    import Navbar from './components/NavbarView.vue'
    import { ref, provide,computed } from 'vue'

    const jwtToken = computed(() => sessionStorage.getItem('jwtToken'));
    const navIsLogIn = ref(false);
    const getNavCartItemCount = ref(() => { });
    provide("navIsLogIn", navIsLogIn);
    provide("getNavCartItemCount", getNavCartItemCount)
    provide("jwtToken", jwtToken);
</script>

<template>
    <nav>
        <Navbar />
    </nav>
    <main>
        <div class="container" style="min-height: 80vh;">
            <router-view></router-view>
        </div>
    </main>
</template>


