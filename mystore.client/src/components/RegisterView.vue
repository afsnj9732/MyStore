<template>
    <nav>
        <NavBar />
    </nav>
    <div>
        Email:<input type="text" v-model="email" /><br />
        密碼:<input type="text" v-model="password" /><br />
        再次輸入密碼:<input type="text" v-model="confirmPassword" /><br />
        <button @click="register()">註冊</button>
    </div>
</template>

<script setup>
    import NavBar from './NavBar.vue'
    import { ref } from 'vue';
    import axios from 'axios';

    const email = ref(null);
    const password = ref(null);
    const confirmPassword = ref(null);
    let recaptchaToken;

    grecaptcha.ready(function () {
        grecaptcha.execute('6LdoNBIqAAAAABPwyhXYJInO4cjAIh-I6l52_0PN').then(function (token) {
            recaptchaToken = token;
        });
    });




    const register = () => {
        if (!recaptchaToken) {
        console.error("recaptcha token 尚未獲取");
        return; 
        }
        axios.post("https://localhost:7266/api/Member/register",
            {
                "Email": email.value,
                "Password": password.value,
                "ConfirmPassword":confirmPassword.value,
                "RecaptchaToken": recaptchaToken
            })
            .then(response => {
                //localStorage.setItem("jwtToken", response.data.token);
            })
            .catch(error => {
                console.error("註冊失敗");
            });
    }
</script>
