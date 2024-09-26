<template>
    <div>
        <pre>{{ data }}</pre>
    </div>
</template>

<script setup>import { ref, onMounted } from 'vue';
import axios from 'axios';

const data = ref(null);

onMounted(() => {
  axios.get("")
    .then(response => {
      data.value = response.data;
    })
    .catch(error => {
      console.error(error);
    });
});</script>
