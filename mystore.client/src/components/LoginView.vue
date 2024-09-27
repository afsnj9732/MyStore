<template>
    <div>
        Email:<input type="text" v-model="email" /><br />
        密碼:<input type="text" v-model="password" />
        <button @click="login()">登入</button>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const email = ref(null);
    const password = ref(null);
    let recaptchaToken;

    grecaptcha.ready(function () {
        grecaptcha.execute('6LdoNBIqAAAAABPwyhXYJInO4cjAIh-I6l52_0PN').then(function (token) {
            recaptchaToken = token;
        });
    });




    const login = () => {
        axios.post("https://localhost:7266/api/Member/login",
            {
                "Email": email.value,
                "Password": password.value,
                "RecaptchaToken": recaptchaToken
            })
            .then(response => {
                localStorage.setItem("jwtToken", response.data.token);
            })
            .catch(error => {
                console.error("登入失敗");
            });
    }
</script>
