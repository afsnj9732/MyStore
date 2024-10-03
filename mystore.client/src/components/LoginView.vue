<template>
    <nav>
        <Navbar />
    </nav>
    <div class="container">
        <div class="row justify-content-center">
            <div class="card" style="width: 18rem;">
                <div class="card-body">
                    <form @submit.prevent>
                        <div class="mb-3">
                            <label for="exampleInputEmail1" class="form-label fw-bold">Email</label>
                            <input type="email" class="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" v-model="email " required>
                        </div>
                        <div class="mb-3">
                            <label for="exampleInputPassword1" class="form-label fw-bold">密碼</label>
                            <input type="password" class="form-control" id="exampleInputPassword1" v-model="password" required>
                        </div>
                        <div class="d-flex justify-content-between">
                            <button :disabled="isDisabled" type="submit" class="btn btn-primary" @click="login()">登入</button>
                            <span v-show="isDisabled" class="spinner-border" role="status">
                            </span>
                        </div>
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
        if (email.value && password.value) {
            isDisabled.value = true;
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
