# Forge API Intro Lab 4 - Forge API Web Intro JS 

This is a part of introductory materials for Forge API.

The labs consist of four modules. Starting from Lab1, it incrementally 
adds code or reuse the code you have written, and implement toward a simple web service application. 

This is the forth project that built on top of Lab 3 and add JavaScript layer to view a translated model in an embedded viewer.   

**How to run the sample project**

* obtain client ID and secret from https://developer.autodesk.com/

In order to use Forge, you will need to register your app and obtain a pair of client ID and secret keys.

* Once you have the above information, open the Web.config and set your own configuration setting there.  


```xml
  <appSettings>
    <add key="baseApiUrl" value="https://developer.api.autodesk.com/"/>
    <add key="clientID" value=""/>
    <add key="clientSecret" value=""/>
    <add key="bucketKey" value=""/>
  </appSettings>
```

The project is written in ASP.NET C#, using Microsoft Visual Studio 2015, .NET 4.5.2.

Dependency: RestSharp 106.2.0 (NuGet will automatically download a package when you build.) 

Disclaimer: Minimum error checking for simplicity and readability to focus on learning Forge API. 

To Do: Detailed explanation to come on:  
https://fieldofviewblog.wordpress.com/

For additional information about Forge, please refer to:
https://fieldofviewblog.wordpress.com/Forge

Written by M.Harada, March 2015. 

Last updated: 1/22/2018 by M.Harada 
