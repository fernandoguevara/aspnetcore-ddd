# ASP.NET Core Web API Domain Driven Design Example

Just an example project where ddd is implemented with some other great patterns and architecture learnt from eShopOnContainers.

The use case is just a simple web api which saves notes in a database and send some emails when they are created.

Background task project which generates a simple xlsx that you send by email.

## Architecture
* .NET CORE 3.1 LTS(waiting for 5 tls)
* Domain driven design
* Mediator Pattern
* CQRS Pattern (Commands and Queries)
* Swagger API documentation
* Entity Framework Migrations
* Multiple Database Vendors (specifically sql server and postgres)
* Domain Events
* Fluent Validation
* Background task for report generation
* Repository Pattern
* Unit of Work Pattern
* Keycloak JWT User Roles for Web API Validation
* Elastic Search Logging
* Pipeline Registration
* Dockerfile to create Docker Images
* Swagger Implicit Authentication with Keycloak

## How to Run
Basically you need to create a database(sql server or postgres), keycloak, elasticsearch , and kibana if you want data visualization.

You can use the Vagrantfile if you use got Vagrant installed on your machine, it's nice and easy to use for demo projects, the virtual machine will need at least 3-4GB RAM if you intend to use kibana and elasticsearch.

## DISCLAIMER!
It's just a demo project , not intended be used in production, remember there's not silver bullet.
