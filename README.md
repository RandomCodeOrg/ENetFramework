# ENetFramework

The ENetFramework is an API that can be used to develop web applications in .Net using/borrowing your favorite JavaEE techniques.

Features currently supported by the reference implementation...*
* Context and Dependency Injection (CDI)
* Framework Views (Java Server Faces - JSF)
* Expression Statements (Expression Language - EL)

Planned features...
* Binding Validation (Bean/Faces Validation)
* RESTful Webservices

<sub>*) The corresponding Java technology is given in parentheses</sub>

## Anatomy of this solution
The Solution in this respository contains three projects. The one called 'RandomCodeOrg.ENetFramework' is the API, intended to be used by programmers developing a web application based on this framework. An example for such an application is given by the 'RandomCodeOrg.Mercury' project. The third project ('RandomCodeOrg.Pluto') is a reference implementation of an application container that can host ENetFramwork-based applications.

## Getting started
1. Clone this repository
2. Open the solution
3. Run the contained project 'RandomCodeOrg.Mercury'

## Disclaimer
As you might have noticed, this project is everything but ready for production. You are very welcome to use this framework for experimental/academic or "just for fun" purposes.

I would appreciate every kind of contribution and feedback. Maybe we could then someday recommend this framework for production use...
