<template>
    <nav>
        <Navbar />
    </nav>
    <div class="container">
        <div class="row justify-content-center">
            <div class="card" style="width: 18rem;">
                <div class="card-body">
                    <form @submit.prevent>
                        Email:<input type="email" v-model="email " required /><br /><br />
                        密碼:<input type="password" v-model="password" required /><br /><br />
                        <button :disabled="isDisabled" type="submit" class="btn btn-primary" @click="login()">登入</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
    import Navbar from './NavbarView.vue'
    import { ref } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router';

    const router = useRouter();
    const email = ref(null);
    const password = ref(null);
    const isDisabled = ref(false);
    let recaptchaToken;

    //const loadReCaptchaScript = () => {
    //    return new Promise((resolve) => {
    //        const script = document.createElement('script');
    //        script.src = 'https://www.google.com/recaptcha/api.js?render=6LdoNBIqAAAAABPwyhXYJInO4cjAIh-I6l52_0PN';
    //        script.onload = () => {
    //            resolve();
    //        };
    //        document.body.appendChild(script);
    //    });
    //};


    const login = () => {
        isDisabled.value = true;
        if (email.value && password.value) {
            grecaptcha.ready(function () {
                grecaptcha.execute(import.meta.env.VITE_RECAPTCHA).then(function (token) {
                    recaptchaToken = token;
                    axios.post(import.meta.env.VITE_API_LOCAL+"api/Member/login",
                        {
                            "Email": email.value,
                            "Password": password.value,
                            "RecaptchaToken": recaptchaToken
                        }, {
                            headers: {
                                'Ocp-Apim-Subscription-Key': import.meta.env.VITE_API_KEY
                            }
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
                        })
                        .finally(() => {
                            isDisabled.value = false;
                        });

                });
            });

        }

    }
    //onMounted(() => {
    //    loadReCaptchaScript();
    //})
</script>
