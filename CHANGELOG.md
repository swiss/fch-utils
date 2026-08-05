# Changelog

## v3.0.1
Minor corrections in the XML documentation of public members.

## v3.0.0
- XML comments for public members added.
- BREAKING CHANGE: Visibility of ```Swiss.FCh.Utils.Services.HtmlNormalizer``` and ```Swiss.FCh.Utils.Services.SmtpClientFactory``` is now ```internal```.
- BREAKING CHANGE: ```Swiss.FCh.Utils.Rhos.Stage``` is now a ```static``` class.

## v2.0.2
NuGet dependencies updated

## v2.0.1
Distinction between different dotnet versions corrected

## v2.0.0
BREAKING CHANGE: ```HtmlNormalizerOptions``` moved from ```Swiss.FCh.Utils.Models``` to ```Swiss.FCh.Utils.Configurations```
BREAKING CHANGE: ```HtmlNormalize``` is no registerd in the DI container with its own extension method ```AddHtmlNormalizer()```. Calling ```AddEmailService()``` will no longer add the ```HtmlNormalizer```.

## v1.9.1
Initial publication on GitHub
