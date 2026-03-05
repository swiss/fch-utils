# Introduction

This library contains utils that can be used to integrate with the Redhat Openshift cloud plattform of the Federal Office of Information Technology, Systems and Telecommunication FOITT.

Additionally, it offers some general helpers
- ```IEmailService``` based on MailKit
- Serilog Formatter for optimal integration with Splunk in the FOITT
- Some converters 

The latest NuGet package is published at https://www.nuget.org/packages/Swiss.FCh.Utils.

# Usage

## E-Mail Service
Use ```IServiceCollection.AddEmailService()``` to register the ```Swiss.FCh.Utils.Services.IEmailService``` in your DI container.

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
