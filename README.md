# InstaHashtagUsage
This is a desktop app for obtaining instagram hashtag publications count. 
App checks input hashtags coutns on site using a headless browser and categorizes them based on appsettings.json file config.
Console prototype is [here](https://github.com/keepdream1ng/scrapInstagramHashtag).

Example appsettings.json line for 4 categories: "hashtagCategoriesThresholds": [500000, 10000, 0]
You can also show hidden browser whith option "headlessBrowser": false

This app needs to login to Instagram like any other user, and it also just helps categorize hashtags you can lookup online.
But I suggest not to use your main account on this one, just in case the platform will want to ban you.

I feel confident with a web interfaces so that is why I'm building it as Winform with a webview for Blazor components.
Using it lets you copy entire result table just by selecting UI elements.

Preview of UI:

![](https://github.com/keepdream1ng/InstaHashtagUsage/blob/main/pics/preview.gif)

Running this on someone else's pc may require installation from https://developer.microsoft.com/en-us/microsoft-edge/webview2/ if OS is older.
