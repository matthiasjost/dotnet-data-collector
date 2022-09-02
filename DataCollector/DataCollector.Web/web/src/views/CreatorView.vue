<script lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import CreatorView from "@/views/CreatorView.vue";

import { CreatorClient } from "@/creator-client"

export default {
  name: "CreatorView",

  mounted() {
    console.log(`the component is now mounted.`)
    var client = new CreatorClient();
    var list = client.creators();
    console.log(list);
  }
}

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
      <div><input type="search" id="search" name="search" placeholder="Search"></div>
    </div>
    <table role="grid">
      <thead>
      <tr>
        <th scope="col">#</th>
        <th scope="col">Country</th>
        <th scope="col">Name</th>
        <th scope="col">Channels</th>
        <th scope="col">Last Check</th>
      </tr>
      </thead>
      <tbody>
      <tr>
        <th scope="row">1</th>
        <td>Cell</td>
        <td>Cell</td>
        <td>Cell</td>
        <td>Cell</td>
      </tr>
      <tr>
        <th scope="row">2</th>
        <td>Cell</td>
        <td>Cell</td>
        <td>Cell</td>
        <td>Cell</td>
      </tr>
      </tbody>
    </table>
    <p>This list is based on the following GitHub Repository: <a href="https://github.com/matthiasjost/dotnet-content-creators">matthiasjost/dotnet-content-creators</a></p>
  </main>
</template>


<style scoped>

</style>