<script lang="ts" setup>
import { ref, watch } from 'vue'

  defineProps(['creatorsProp'])
  const searchValue = ref("");

  function onInput(event: any)
  {
    let text = event.target.value
    console.log(text);
  }
  watch(searchValue, async (newSearchValue) => {
    //console.log(newSearchValue);
  })
</script>
<template>
  <main class="container">
    <nav>
      <ul>
        <li><strong>content-creators.net</strong></li>
      </ul>
      <ul>
        <li><a href="https://www.github.com/matthiasjost/dotnet-content-creators" role="button">GitHub</a></li>
      </ul>
    </nav>
    <h1>.NET Content Creators</h1>
    <div class="grid">
      <div>
        <input id="search" name="search" placeholder="Search" type="search" v-model="searchValue" @input="$emit('searchValueInput', $event.target.value)">
      </div>
    </div>
    <table role="grid">
      <thead>
      <tr>
        <th scope="col">#</th>
        <th scope="col">Country</th>
        <th scope="col">Name</th>
        <th scope="col">Channels</th>
        <th scope="col">Tags</th>
        <th scope="col">Last Check</th>
      </tr>
      </thead>
      <tbody>
      <tr v-for="(item, index) in creatorsProp">
        <th scope="row">{{ index + 1 }}</th>
        <td>{{ item.countryOrSection }}</td>
        <td>{{ item.name }}</td>

        <td>
          <span v-for="(link, index) in item.links" :key="link.url">
            <span v-if="index === (item.links.length-1)">
              <a :href="link.url">{{ link.label }}</a>
            </span>
            <span v-if="index != (item.links.length-1)">
              <a :href="link.url">{{ link.label }}</a>,
            </span>
          </span>
        </td>
        <td>
          <span v-for="(link, index) in item.tags" :key="link.url">
            <span v-if="index === (item.tags.length-1)">
              {{ item.tags }}
            </span>
            <span v-if="index != (item.links.length-1)">
              {{ item.tags }},
            </span>
          </span>
        </td>
        <td>-</td>
      </tr>
      </tbody>
    </table>
    <p>This list is based on the following GitHub Repository: <a
        href="https://github.com/matthiasjost/dotnet-content-creators">matthiasjost/dotnet-content-creators</a></p>
  </main>
</template>

<style scoped>

</style>