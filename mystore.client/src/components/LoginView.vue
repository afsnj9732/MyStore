<template>
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
    import { ref,inject } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router';
    import { jwtDecode } from 'jwt-decode';

    const router = useRouter();
    const email = ref(null);
    const password = ref(null);
    const isDisabled = ref(false);
    let userRole = inject("role")
    let jwt = inject("jwtToken");
    let isLogIn = inject('navIsLogIn');
    let getCartItemCount = inject('getNavCartItemCount');
    let getItemCount = inject('getItemCount'); 
    let recaptchaToken;


    const login = () => {
        if (email.value && password.value) {
            if (typeof grecaptcha === 'undefined') {
                alert("reCAPTCHA 載入失敗，請重新嘗試，或檢查網路狀況");
                return;
            }
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
                            localStorage.setItem("jwtToken", response.data.token);
                            userRole.value = jwtDecode(jwt.value).role;
                            alert("登入成功");
                            isLogIn.value = true;
                            getCartItemCount.value();
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

</script>
