# ENetFramework [![Build Status](https://travis-ci.org/RandomCodeOrg/ENetFramework.svg?branch=master)](https://travis-ci.org/RandomCodeOrg/ENetFramework) [![License](http://img.shields.io/:license-apache-blue.svg)](http://www.apache.org/licenses/LICENSE-2.0.html)

The ENetFramework is an API that can be used to develop web applications in .Net using/borrowing your favorite JavaEE techniques.

Features currently supported by the reference implementation*
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
You can

&nbsp;&nbsp;create your own web application using [this tutorial](https://github.com/RandomCodeOrg/ENetFramework/wiki/Creating-an-ENetFramework-Web-Application)

&nbsp;&nbsp;or run the example application
1. Clone this repository
2. Open the solution
3. Run the contained project 'RandomCodeOrg.Mercury'

# Contributors Wanted
You think that this project has too little features and too many bugs? But you like the basic idea? Then don't hesitate to participate.
**All** types of contribution are very welcome. For example, you could
- create pull requests
- report bugs
- enhance the documentation
- make own suggestions for new features
- give us your feedback

Feel free to contact us at enetframework@outlook.com

## Disclaimer
As you might have noticed, this project is everything but ready for production. You are very welcome to use this framework for experimental/academic or "just for fun" purposes.

I would appreciate every kind of contribution and feedback. Maybe we could then someday recommend this framework for production use...
