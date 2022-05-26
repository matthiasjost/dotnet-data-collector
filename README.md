
# 🗞️ .NET Data Collector


The dotnet data collector is an application collecting information about content creation channels from this [list](https://github.com/matthiasjost/dotnet-content-creators).

The list is a markdown file with a list of curated dotnet content creators publishing content in English and must have published something in 2022 already.

## 🎨 Project Goals

* Detect broken links on the dontent content creator markdown list.
* Keep the original list clean and simple and easy to extend: Use the .NET Data Collector to enrich the data from the markdown list.
* The additionally collected data is stored inside a Database which includes:
  * A represantation of the whole markdnown structure: Creator Names, respective country and channel Urls.
  * The RSS feed extracted from the channel HTML 

## :octocat: Extracted Data 

This table descirbes everything that can be directly parsed from the original dotnetn content creator list.

| Subject  | Data Parsed |
| ------------- | ------------- |
| Channel | URL, "Type"
| Creator | Name, Country


## 🐙 Aggregated Data

This table contains a description of everything that should be added/enriched to the orginally parsed data by this tool.

| Subject  | Data Aggregated / Extracted |
| ------------- | ------------- |
| Channel | Last 200 OK HTTP Response Code Time, RSS URL
| Creator | Nothing
| Content | All RSS items 

## ✔️ Features




