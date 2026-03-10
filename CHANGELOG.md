# Changelog

## v2.0.1
Distinction between different dotnet versions corrected

## v2.0.0
BREAKING CHANGE: ```HtmlNormalizerOptions``` moved from ```Swiss.FCh.Utils.Models``` to ```Swiss.FCh.Utils.Configurations```
BREAKING CHANGE: ```HtmlNormalize``` is no registerd in the DI container with its own extension method ```AddHtmlNormalizer()```. Calling ```AddEmailService()``` will no longer add the ```HtmlNormalizer```.

## v1.9.1
Initial publication on GitHub
