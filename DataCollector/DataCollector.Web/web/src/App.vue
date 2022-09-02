<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import CreatorView from "@/views/CreatorView.vue";

import {CreatorClient, CreatorDto} from "@/creator-client"
import {reactive, ref} from 'vue'


// let CreatorList: CreatorDto[];


let CreatorList: CreatorDto[];
let client = new CreatorClient();


(async () => { CreatorList = await client.creators() })();

</script>

<template>
<CreatorView :creators="CreatorList" />
</template>
<style scoped>
</style>
