# CleanArchitectureSolutionTemplate

Stig's design notes about why this solution template is designed as it is.

## Build

Publish docker container:

    dotnet publish -c Release --self-contained -r linux-x64



## 3rd. party libraries

The fewer 3rd. party libraries the better as it helps stayng in control.


Here are the 3rd part tools I consider using:

- Postgres SQL database. It is open source and has **no licensing costs** and 
  matches or trumps SQL server and feature and performance wise. 

- Fluent migrator for database migrations. If you prefer pure SQL 
  migrations, Fluent migrator supports this or you could consider using DbUp.

- Fluent Assertions

- NSubstitute. You should generally limit mocking in your tests in favor of 
  integration tests.


## Dependency Injection

Constructor injection of interfaces is generally used everywhere to help 
ensuring loose coupling of code. 

One exception is the Sys instance that is an ambient servier (a static async 
local class) that provides very common stuff like current user, current 
correlation id, system clock (as a testable interface) etc.


All dependencies must be registered some how. If done on the entry assembly 
(for example the executable web application), then it has to have references
to all other assemblies. This would violate the Clean Architecture principle
of references all ways going from outer to inner layers. If this principle
is broken to be able to make depency registrations, then it becomes easier
to introduce leaky abstractrations.

There are 2 ways to avoid this:

- Introduce a special bootstrapper project
- Use assembly scanning

### Special bootstrapper project
Introduce a special bootstrapper (a.k.a Composition Root) assembly whose sole 
purpose it to make dependency registrations.  

This assebmly will be allowed to have references to all other assemblies 
because these referecens are only used to call registration methods in the 
referenced assemblies.

### Assembly Scanning
If using assembly scanning it is important to do this in an explicit way and 
not rely on making dependency registrations by convention. To make the
assembly scanning explicit, attributes are used so that only types explicitly   
having certain attributes are included in dependency registrations.

StigsUtils.Depencency injection provides attbitues and extension methods to
do explicit assembly scanning of dependencies.

Alternatively convention based on naming og interfaces and classes could be
used, but this can lead to unclear 

## Automated Testing

I use xUnit and FluentAssertions when writing tests. I prefer larger integration
tests over isolated unit tests.


### Use dependency injection in test code !

I avoid newing up mocks in test and instead do overwriting 
dependency registration of test doubles (mocks, fakes, etc.). This helps
keeping test code maintainable and easy to write and as a bonus also
helps testing the wiring up of dependencies.
