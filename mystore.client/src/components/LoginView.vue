<template>
    <nav>
        <NavBar />
    </nav>
    <div class="container">
        <div class="row justify-content-center">
            <div class="card" style="width: 18rem;">
                <div class="card-body">
                    <form @submit.prevent>
                        Email:<input type="email" v-model="email " required /><br /><br />
                        密碼:<input type="password" v-model="password" required /><br /><br />
                        <button type="submit" class="btn btn-primary" @click="login()">登入</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
    import NavBar from './NavbarView.vue'
    import { ref } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router';

    const router = useRouter();
    const email = ref(null);
    const password = ref(null);
    let recaptchaToken;


    const login = () => {
        if (email.value && password.value) {
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
                            if (error.response.data.apiMessage) {
                                alert(error.response.data.apiMessage);
                            } else {
                                alert("資料格式不符規定，請重新輸入");
                            }
                        });

                });
            });
        }

    }
</script>
