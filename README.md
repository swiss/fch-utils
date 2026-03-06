# Introduction

This library contains utils that can be used to integrate with the Redhat Openshift cloud plattform of the Federal Office of Information Technology, Systems and Telecommunication FOITT.

Additionally, it offers some general helpers
- ```IEmailService``` based on MailKit
- Serilog Formatter for optimal integration with Splunk in the FOITT
- An ```HtmlNormalizer``` that can streamline HTML by removing styles, images, tags and more based on what is configured in ```HtmlNormalizerOptions``` .
- Some converters 

The latest NuGet package is published at https://www.nuget.org/packages/Swiss.FCh.Utils.

# Usage

## E-Mail Service
Use ```IServiceCollection.AddEmailService()``` to register the ```Swiss.FCh.Utils.Services.IEmailService``` in your DI container.

A configuration section like the following must be present in your app settings.

```json
{
    "EmailService": {
	    "Host": "my_smtp_server",
		"Port": "my_smtp_port"
	}
}
```

### Sending an E-Mail
Sending an e-mail could then be implemented as follows.

```
var message = new Email
{
    From = new EmailAddress { Address = "from@example.com", Name = "From" },
    To = [new EmailAddress {Address = "to@example.com", Name = "To"}],
    Subject = "Test Subject", 
    TextMessage = "Hello World"
};

await service.Send(message);
```

## Serilog Formatter
In your ```appsettings.json```, configure Serilog as follows.

```json
  "Serilog": {
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "formatter": "Swiss.FCh.Utils.Logging.SerilogFormatter, Swiss.FCh.Utils"
        }
      }
    ]
  }
```

## FOITT Redhat Openshift Tooling
- Use ```IHostApplicationBuilder.AddRhosConfigurations("YOUR_STAGE")``` to read environment configurations.
- Add PGSQL DB secrets like this: ```IHostApplicationBuilder.AddRhosPostgresConfiguration("YOUR_STAGE", "path-to-your/pg-database-credentials.json")```
- Add S3 secrets like this: ```IHostApplicationBuilder.AddRhosS3Configuration("YOUR_STAGE", "path-to-your/s3-credentials.json")```

## HTML Normalizer
Use ```IServiceCollection.AddHtmlNormalizer()``` to register the ```Swiss.FCh.Utils.Services.IHtmlNormalizer``` in your DI container.
Then call ```Swiss.FCh.Utils.Services.IHtmlNormalizer.Normalize("your HTML", new HtmlNormalizerOptions { ... })```.

The ```HtmlNormalizerOptions``` allow you to configure the following:
- ```RemoveClasses``` removes all classes from the HTML (e.g. ```<div class="my-class">``` will result in ```<div>```).
- ```RemoveStyles``` removes all styles from the HTML (e.g. ```<div style="display: block">``` will result in ```<div>```).
- ```RemoveImages``` removes all ```<img>``` tags from the HTML.
- ```RemoveEmptyAnchors``` removes all ```<a>``` tags with no ```href``` (it will keep the content of the ```<a>``` tag as content of its parent).
- ```RemoveSpans``` removes all ```<span>``` tags and adds their content to its parent. This is only applied, if ```RemoveStyles```, ```ReplaceItalicWithEmphasis``` and ```ReplaceBoldWithStrong``` are enabled.
- ```ReplaceItalicWithEmphasis``` replaces ```<i>``` with ```<em>```.
- ```ReplaceBoldWithStrong``` replaces ```<b>``` with ```<strong>```.

# Contribution
See: https://github.com/swiss/fch-urils/blob/main/CONTRIBUTING.md

# Security
See: https://github.com/swiss/fch-utils/blob/main/SECURITY.md

# Development Workflow

To publish a new version of the NuGet package, proceed as follows.

* apply and push your changes
* define and describe the new version in ```CHANGELOG.md```
* push the corresponding label with ```git tag vx.x.x``` and ```git push origin vx.x.x```
* go to GitHub -> Actions -> 'Build and Publish to NuGet.org' and trigger a run while specifying the correct GIT label
