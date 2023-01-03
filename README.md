# HTMX LAB
![img](https://htmx.org/img/memes/original.png)

An example of using asp.net MVC razor and jquery to create equivalent effects of single-page-apps in a fraction of the surface area.

## Goal
The project tests the feasibility of HTMX as a replacement for AngularJS/ReactJS for both learning purposes and common "complex" business use cases.

## Rationale

HTMX poses a significant disruption to front-end development for many business use cases.
The key benefit is in the code you DONT write:

 - Remove the need for a separate/independent "web tier" application layer including the need to host and secure web-tier assets.
 - Remove the need for javascript package managers and downloading hundreds of javascript packages to build a front-end application.
 - Remove duplicative data-transfer-objects on both client-side and server-side.
 - Remove duplicative authorization checks on client-side and server-side.
 - Remove duplicative form validation logic on client-side and server-side.
 - Remove the work/effort of client-side unit testing.
 - Remove the need for complex client-side state-management and return to managing state in DOM.
 - Case studies [indicate 66% codebase reduction](https://htmx.org/essays/a-real-world-react-to-htmx-port/) for non-trivial apps.

HTMX is more than just code reduction. There's noteworthy benefits this paradigm shift offers above and beyond SPAs:
 - Through use of partial-sharing, support for sharing UI "components" between adjacent apps and microservices becomes trivial. Interchangable partials drastically simplifies the logistics of application UI sharing, integrations and microservice UIs.
 - Reactjs applications can reach or exceed 1MB in size. HTMX comes as a single 11kb file thus improving load time.
 - Remove the need for virtual doms required by reactjs thus improving performance on client-side and allowing larger page content. The end result is a snappier UI performance.
 - Jquery and MVC-razor technologies are over a decade old. Finding competent teammates capable of developing server-side is more feasible. Development among smaller teams with less tiers takes significantly less time and people.
 - Hypermedia guides developers to use more intuitive RESTful structures which includes data and methods. Single-page-apps tend to read-write JSON which has a tendency to lean towards RPC. For more information on this point see [this essay regarding HATEOAS](https://htmx.org/essays/hateoas/).

## Caveats

While HTMX poses a superior option to SPA for many use cases there are places where SPA remains the most sensible choice:

 - Apps with requirements to support offline mode. HTMX leans heavily on server-side randering. Apps truly requiring offline-content are rare and tend to be more thick-client applications in practice.
 - Pages which require significant amount of client-side state. An example would be google spreadsheets or office 365 word editor which are more like thick clients which happen to run on a web page.

## TODOs
At the time of this writing only 5 hours of development time has been invested into this project. This code is not production-ready but serves as a foundation for proof-of-concept and feasibility study for future development.
More needs to be done to study feasibility of migrating existing SPAs into HTMX and how this can be done gradually.

## References
 - Many of these examples are distilled and simplified versions of an original tutorial [published by Jetbrains in 2021](https://www.jetbrains.com/dotnet/guide/tutorials/htmx-aspnetcore/what-is-htmx/).
 - [HTMX lab heroku site](https://htmxlab.herokuapp.com/)
 - [HTMX official site and documentation](https://htmx.org/)
