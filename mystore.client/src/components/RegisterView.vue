<template>
    <div class="container">
        <div class="row justify-content-center">
            <div class="card" style="width: 23rem;">
                <div class="card-body">
                    <form @submit.prevent>
                        <div class="mb-3">
                            <label for="exampleInputEmail1" class="form-label fw-bold">Email</label>
                            <input type="email" class="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" v-model="email " required>
                        </div>
                        <div class="mb-3">
                            <label for="exampleInputPassword1" class="form-label fw-bold">密碼</label>
                            <input type="password" class="form-control" id="exampleInputPassword1" v-model="password" required placeholder="長度限制5~16碼">
                        </div>
                        <div class="mb-3">
                            <label for="exampleInputPassword1" class="form-label fw-bold">請再次輸入密碼</label>
                            <input type="password" class="form-control" id="exampleInputPassword1" v-model="confirmPassword" required placeholder="長度限制5~16碼">
                        </div>
                        <div class="d-flex justify-content-between">
                            <button :disabled="isDisabled" type="submit" class="btn btn-primary" @click="register()">註冊</button>
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
    import { ref } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router';

    const email = ref(null);
    const password = ref(null);
    const confirmPassword = ref(null);
    let recaptchaToken;
    const isDisabled = ref(false);
    const router = useRouter();

    const register = () => {
        if (email.value && password.value && confirmPassword.value) {
            if (typeof grecaptcha === 'undefined') {
                alert("reCAPTCHA 載入失敗，請重新嘗試，或檢查網路狀況");
                return;
            }
            isDisabled.value = true;
            grecaptcha.ready(function () {
                grecaptcha.execute(import.meta.env.VITE_RECAPTCHA).then(function (token) {
                    recaptchaToken = token;
                    axios.post(import.meta.env.VITE_API_LOCAL + "api/Member/register",
                        {
                            "Email": email.value,
                            "Password": password.value,
                            "ConfirmPassword": confirmPassword.value,
                            "RecaptchaToken": recaptchaToken
                        }, {
                        headers: {
                            'Ocp-Apim-Subscription-Key': import.meta.env.VITE_API_KEY
                        }
                    }

                    )
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
                        })
                        .finally(() => {
                            isDisabled.value = false;
                        });
                });
            });
        }
    }


</script>
