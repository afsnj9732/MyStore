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
    import { useRouter } from 'vue-router';

    const email = ref(null);
    const password = ref(null);
    const confirmPassword = ref(null);
    let recaptchaToken;
    const router = useRouter();


    const register = () => {
        grecaptcha.ready(function () {
            grecaptcha.execute('6LdoNBIqAAAAABPwyhXYJInO4cjAIh-I6l52_0PN').then(function (token) {
                recaptchaToken = token;
                axios.post("https://localhost:7266/api/Member/register",
                    {
                        "Email": email.value,
                        "Password": password.value,
                        "ConfirmPassword": confirmPassword.value,
                        "RecaptchaToken": recaptchaToken
                    })
                    .then(response => {
                        alert("註冊成功");
                        router.push('/login');
                    })
                    .catch(error => {
                        if (error.response) {
                            alert(error.response.data);
                        } else {
                            alert("資料格式不符規定，請重新輸入");
                        }
                    });
            });
        });

    }
</script>
