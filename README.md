# InstaHashtagUsage
This is a desktop app for obtaining instagram hashtag publications count. 
App checks input hashtags counts on website using a headless browser and categorizes them based on appsettings.json file config.
Console prototype is [here](https://github.com/keepdream1ng/scrapInstagramHashtag).

# Motivation
My lovely wife as a marketing specialist used to spend a bunch of time on instagram hashtag research
and I like making people's lives easier through streamlined workflows.
While companies may get access to the Instagram API for such purposes,
my program offers an instant and effective solution without bureaucratic obstacles.
I feel confident with web interfaces so that is why I'm building it as Winform with a webview for Blazor components.
Using it lets me write less code for updating UI.

# Usage
This app needs to login to Instagram like any other user, and helps categorize hashtags you can lookup online.
But I suggest not to use your main account on this one, just in case the platform will want to ban you.

You can also show hidden browser whith option:
```json
  "headlessBrowser": false,
```
Example appsettings.json line for 4 categories ( 500k+ | 500k - 10k | 10k - 0 | 0 ): 
```json
  "hashtagCategoriesThresholds": [500000, 10000, 0],
```
If you find chars you need to remove from input automatically, add it into this array:
```json
  "cleanThisChars": [" ", ".", ",", "#", "\n", "\t"],
```
Add hashtags to search anytime, don't forget to create and select different table to be filled if your research has multiple topics.
Edit table header after double click to note the topic. You can copy the entire result just by selecting UI, and pressing Ctrl+C.

Running this on non dev's pc may require installation of [webview2](https://developer.microsoft.com/en-us/microsoft-edge/webview2/) if OS is older.

Preview of UI:

![](https://github.com/keepdream1ng/InstaHashtagUsage/blob/main/pics/preview.gif)

