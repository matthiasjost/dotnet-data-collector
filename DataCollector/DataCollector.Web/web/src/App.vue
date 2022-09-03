<script setup lang="ts">
  import { reactive, ref, watch } from 'vue'
  import { RouterLink, RouterView } from 'vue-router'
  import CreatorView from "@/views/CreatorView.vue";
  import {Client } from "@/creator-client"

  const items = reactive({ creators: new Array() })
  const searchValue = ref({})
  const client = new Client();

  function searchValueInputChange(value: any)
  {
    searchValue.value = value;
    console.log("searchValueInputChange");
    console.log(value);
  }
  watch(searchValue, async (newSearchValue) => {

    if(newSearchValue.toString().length != 0)
    {
      items.creators = await client.search(newSearchValue.toString());
    }
    else
    {
      items.creators = await client.creators()
    }

    console.log(searchValue);
    console.log(items.creators);
  });

  (async () => {
    items.creators = await client.creators()
    console.log(items.creators);
  })();

</script>

<template>
  <CreatorView :creatorsProp="items.creators" @searchValueInput="searchValueInputChange" />
</template>

<style scoped>

</style>
