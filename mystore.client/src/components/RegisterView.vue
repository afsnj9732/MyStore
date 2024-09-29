<template>
    <nav>
        <NavBar />
    </nav>
    <div class="container">
        <div class="row justify-content-center">
            <div class="card" style="width: 23rem;">
                <div class="card-body">
                    <form @submit.prevent>
                        Email:<input type="email" v-model="email" required /><br /><br />
                        密碼:<input type="password" v-model="password" placeholder="密碼限制5~16碼" required /><br /><br />
                        再次輸入密碼:<input type="password" v-model="confirmPassword" placeholder="密碼限制5~16碼" required /><br /><br />
                        <button type="submit" class="btn btn-primary" @click="register()">註冊</button>
                    </form>
                </div>
            </div>
        </div>
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
        if (email.value && password.value && confirmPassword.value) {
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
                            console.log("axios error");
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
