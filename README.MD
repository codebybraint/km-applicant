## Karya Murni - Rad4m Backend Applicant Test, Todo List Api
## Table of contents
* [Introduction](#introduction)
* [Building](#building)
* [Docker](#docker)
* [Documentation](#documentation)
  
### Introduction
This Todo API application is made to complete the coding test by Karya Murni - Rad4m, by applicants : Ardianto Widjanarko.

This Application run on .NET 5, using SQLite as the Database, and Docker as container.

### Building
Additional NuGet packages, which used to run this Application :
* Microsoft.EntityFrameworkCore.Sqlite (5.0.0)
* Moq (4.17.2)
* FluentAssertion (5.0.0)

### Docker
To run docker, make sure your working directory is the km-applicant-backend folder.
First, build the docker file : (version is optional)

```
$ docker build -t [name]/[tag]:[version] .
```
Check the built images 
```
$ docker images
```
Run the docker, we need to expose port, in order to be accessed by host, in this example we use port 8080
```
$ docker run -p 8080:80 [name]/[tag]:[version]
```
### Documentation
Here [API Call Documentation](https://github.com/codebybraint/km-applicant/blob/master/API%20Call%20Tables.md)
