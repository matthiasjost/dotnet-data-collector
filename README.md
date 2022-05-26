
# 🗞️ .NET Data Collector

## Project Description

The dotnet data collector is an application collecting information about content creation channels from this [list](https://github.com/matthiasjost/dotnet-content-creators).

### A few words about "The List"
The list is a markdown file with a list of curated dotnet content creators publishing content in English and must have published something in 2022 already.

## Project Goals
* Detect broken links on the dontent content creator markdown list.
* Keep the original list clean and simple and easy to extend: Use the .NET Data Collector to enrich the data collected from the markdown list.

## Implementation

### Data extracted directly from the list

Channel:
* URL of the channel 

Creator:
* Name of the creator
* Country of the creator

### Data aggregated by the .NET Data Collector

Channel:
* Broken Link Checker

Creator:
* Nothing

Content:
* New Items in their RSS feed


