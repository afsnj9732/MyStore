<template>
    <nav>
        <NavBar />
    </nav>
    <div>
        Email:<input type="text" v-model="email" /><br />
        密碼:<input type="text" v-model="password" />
        <button @click="login()">登入</button>
    </div>
</template>

<script setup>
    import NavBar from './NavBar.vue'
    import { ref } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router'; 

    const router = useRouter();
    const email = ref(null);
    const password = ref(null);
    let recaptchaToken;


    const login = () => {
        grecaptcha.ready(function () {
            grecaptcha.execute('6LdoNBIqAAAAABPwyhXYJInO4cjAIh-I6l52_0PN').then(function (token) {
                recaptchaToken = token;
                axios.post("https://localhost:7266/api/Member/login",
                    {
                        "Email": email.value,
                        "Password": password.value,
                        "RecaptchaToken": recaptchaToken
                    })
                    .then(response => {
                        sessionStorage.setItem("jwtToken", response.data.token);
                        alert("登入成功");
                        router.push('/');
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
