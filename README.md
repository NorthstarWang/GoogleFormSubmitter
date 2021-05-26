# Introduction
This application is built using Asp.net core MVC 5.0, by using web scraping technology, it get google form information and submit form multiple times with customizable statistics for users.

## Index

- [Install](#install)

## Install
Make sure your device have .Net core SDK.
To run the application, please do following procedures.
1. Run below command in the command line to apply EF Core migration and generate database
```
Update-Database
```
2. As the application has enabled external login authentication, the credentials from Microsoft azure and facebook developer are required, reference: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/?view=aspnetcore-5.0&tabs=visual-studio, to apply credentials to the local data, use
```
dotnet user-secrets set "Authentication:Microsoft:ClientId" "<client-id>"
dotnet user-secrets set "Authentication:Microsoft:ClientSecret" "<client-secret>"
dotnet user-secrets set "Authentication:Facebook:AppId" "<app-id>"
dotnet user-secrets set "Authentication:Facebook:AppSecret" "<app-secret>"
```
