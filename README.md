# James.ServiceStubs

[![NuGet version (James.ServiceStubs)](https://badge.fury.io/nu/James.ServiceStubs.svg)](https://www.nuget.org/packages/James.ServiceStubs/)

## Overview

James.ServiceStubs is a configurable, template-based HTTP service stub host/generator that you can use to self-host service stubs for testing your .NET HTTP client code in an automated fashion.  Some of the challenges that this library can solve are:

* Dependent Service Inaccessibility
* Testing Sad Case Scenarios
* Performance Testing Isolation

### Dependent Service Inaccessibility

In many cases, the service that you want to test with will not be available during your development of the HTTP client code.  This could be due to many reasons.

If you practice TDD (test-driven development) or the group creating the client code is different from that that will implement the service code, the only thing that may have been worked out is the contract for the interface (url template, request/response schema, etc.).  But, the service may not exist for some time.

In other cases, you might be working with a 3rd party that does not provide a pre-production version of their service, or you are working with an internal service that is not guaranteed to be up and running when you decide to run an automated test or demo your software.

### Testing Sad Case Scenarios

Another challenge with testing your HTTP client code is validating how your code will behave in the case of bad responses such as a 400-BadRequest or a 500-InternalServerError.  These kinds of errors are sometimes hard to create manually or by sending the right data.  They can be intermittent, and yet, you need to make sure that your code will be reliable in these situations.

### Performance Testing Isolation

In performance/load testing scenarios, you might need to ensure that the response from a given service does not add any response time to your testing, so that you can better understand the performance of the existing HTTP client code and whatever else it does.  If you are tied to real services, it is not possible to control this variable.

Whatever the reason, James.ServiceStubs will allow you to host that service either in-process using self hosting by installing the Nuget package to your project or by downloading the James.ServiceStubs.zip from [here]() and running it from the command line.

## Getting Started - Automated Integration Tests

To get started, you will need to install the Nuget package to your current integration testing project.

```ps
PM> install-package James.ServiceStubs
```

This will not only install the various dependent libraries that James.ServiceStubs requires, but it will also set your project up with the routing configuration and a sample template file.  Below is what your project sturcture will look like after the install:

```
Project
├── routes.json
├── Templates
|   ├── Sample.template
```

The routes.json file will hold all of the configuration for the routes that you want to support for your HTTP client code.  For instance, the default configuration is below:

```json
[
  {
    "type": "Get",
    "path": "/api/Sample/{id}",
    "template": "Templates/Sample.template",
    "delayInMilliseconds": [ 1000, 2000, 3000 ],
    "delayStrategy": "RoundRobin",
    "status": "OK"
  }
]
```

The default endpoint that you will host on would be http://localhost:1234, so if you made a request of GET to http://localhost/api/Sample/1, James.ServiceStubs would return a 200-OK response with a body that is found in the template in Templates/Sample.template of the project.  That template is as follows:

```json
{
  "message": "Hello, world.  This is sample @Model.Id."
}
```

You will notice that the template is going to return the {id} that was sent in the url based on the tokenization found in the configuration of the route.

The next step is to start up the ServiceStubsHost.

```csharp
using (var host = new ServiceStubsHost("http://localhost:1234"))
{
  host.Start();
}
```

Once you have the host started, you can open your browser and go to the configured route (```http://localhost:1234/api/Sample/1```) and see the response.

## Getting Started - Performance/Manual Tests ##

If you just want to host a service stub without using the Visual Studio IDE, there is a command-line version that you can use.  To install, make sure you have chocolatey installed.  (If you have questions about installing chocolatey, visit [their site](http://chocolatey.org) to get more info.)

Once chocolatey is installed on your local machine, download the [latest package](https://github.com/QualitySoftwareMatters/James.ServiceStubs/blob/master/src/James.ServiceStubs/build/servicestubs.0.4.11.nupkg?raw=true) and run the following command from the same location:

```
>choco install servicestubs.0.4.11.nupkg
```

If everything was installed successfully, you should be able to run the following at the command line from any location:

```
>servicestubs
```

The host should start and say that it is listening on "http://localhost:1234" with a warning that you do not have a routes.config.  The host is listening, but you will not be able to return anything without the routes.config file and any referenced templates.  You can manually create one based on the existing one found [here](https://github.com/QualitySoftwareMatters/James.ServiceStubs/blob/master/src/James.ServiceStubs/James.ServiceStubs/routes.json).

## Route Configuration

The routes.json file holds all of the configuration for the routes that you want to support for your testing scenarios.  This file is set up as an array, so you can hold as many routes as you would like.

```json
[
  {
    "type" : "Get",
    "path" : "/url/to/path1",
    "template" : "path/to/template1.template" 
  },
  { 
    "type" : "Get",
    "path" : "/url/to/path2",
    "template" : "path/to/template2.template" 
  },
  { 
    "type" : "Get",
    "path" : "/url/to/path3",
    "template" : "path/to/template3.template" 
  }
]
```

For each route, there are a number of properties that you can configure.  If the property does not exist, the framework will use the default value.

| Property | Description | Default | Comments |
| -------- | ----------- | ------- | -------- |
| type     | The various request types:  Get, Post, Put, Delete | Get | |
| path | The url path for matching requests to responses | '' | The base path is http://localhost:[port]/ |
| template | The path to the template that should be used for the response | n/a | |
| delayInMilliseconds | The delay that the framework should wait for returning a response | 0 | You can specify 1 without the array syntax or multiple as an array when specifying this value. | 
| delayStrategy | The strategy that the framework should use for delaying the response | RoundRobin | |
| status | The status code description that should be returned with the response | OK | |

### Delays

The framework allows for delaying the response for a configurable milliseconds to aid in simulation for performance testing.  In the sample provided earlier (```json "delayInMilliseconds": [ 1000, 2000, 3000 ]```), the framework will return the first response in 1000 ms.  Because the delayType is 'RoundRobin', the framework will return the next request in 2000 ms and the 3rd in 3000 ms.  The very next request will be returned in 1000 ms and the pattern will continue on as long as the host is alive.

The other strategy for delays is Random.  In that case, the framework will randomly choose from the values that have been provided in the delayInMilliseconds and wait accordingly every time a request comes in.

### Templates

If you want to add templates to the system, you can do that by adding them anywhere relative to the project.  As suggested by the installation, it would make sense to store them in the Templates folder off of the root of your project.  When you reference the template path in the routes.json file, they should be relative to the root.

You will also need to make sure to set the Copy To Output to true for the given file.  You can do this by right-clicking on the file and selecting Properties and filling in this value in the Properties pane.

When rendering templates, all of the data contained within the request will be made available to the template under the @Model keyword.  This includes:

| Item | Description | Syntax | Request Example |
| ---- | ----------- | ------ | --------------- |
| url parameters | Come from tokens that are place in the url template | @Model.id | /api/Samples/{id} |
| querystring parameters | Come from the querystring | @Model.query.id | /api/Customers?id=1 |
| headers | Come from the headers collection | @Model.headers.contenttype | Content-Type: application/json |
| form parameters | Come from the body of a url encoded form that is posted | @Model.form.id | (body) id=1&firstName=Todd |
| body | Come from the body of the request.  Framework supports nested properties. | @Model.Body.address.city | (body) { "Address" : { "City" : "Frisco" }} |
