import { createRouter, createWebHistory } from 'vue-router';
import Home from './components/Home.vue';
import Product from './components/Product.vue';
import Cart from './components/Cart.vue';
import Order from './components/Order.vue';
import Login from './components/Login.vue';
import Register from './components/Register.vue';


const routes = [
    { path: '/', component: Home },
    { path: '/product', component: Product },
    { path: '/cart', component: Cart },
    { path: '/order', component: Order },
    { path: '/login', component: Login },
    { path: '/register', component: Register }

];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;
