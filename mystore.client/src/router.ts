import { createRouter, createWebHistory } from 'vue-router';
import Home from './components/HomeView.vue';
import Product from './components/ProductView.vue';
import ProductDetail from './components/ProductDetailView.vue';
import Cart from './components/CartView.vue';
import Order from './components/OrderView.vue';
import Login from './components/LoginView.vue';
import Register from './components/RegisterView.vue';
import Dashboard from './components/AdminDashboardVue.vue';


const routes = [
    { path: '/', component: Home },
    { path: '/products', component: Product },
    { path: '/products/detail/:productId', component: ProductDetail },
    { path: '/cart', component: Cart },
    { path: '/orders', component: Order },
    { path: '/login', component: Login },
    { path: '/register', component: Register },
    { path: '/dashboard', component: Dashboard }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;
