\# RaceDay Event Management System



\## Project Overview



RaceDay is an event management system designed to support the planning, administration, and participation of road running, walking, and cycling events.



The system is intended to provide a structured digital solution for managing event information, participant registrations, event categories, results, payments, supporting documents, and other information required during the lifecycle of an event.



The project is being developed as part of the RaceDay Portfolio of Evidence and follows a staged development approach. The initial stage focuses on the system's database design and RESTful API planning, with later stages extending the system into a functional web API and application.



\---



\## Project Objectives



The main objectives of the RaceDay system are to:



\* Manage users and their roles within the system.

\* Allow participants to register for events.

\* Allow organisers to create and manage events.

\* Manage event categories such as running, walking, and cycling.

\* Record participant enrolments.

\* Store participant results.

\* Manage event-related documents.

\* Record and manage payments.

\* Store weather information associated with events.

\* Provide a structured database for storing and retrieving system information.

\* Provide RESTful API endpoints for communication between the application and backend services.

\* Apply appropriate authentication and authorisation to protect system functionality.



\---



\# Part 1



Part 1 establishes the technical foundation of the RaceDay system.



The completed Part 1 work includes:



1\. Database design

2\. SQL database script

3\. Entity Relationship Diagram (ERD)

4\. RESTful API endpoint specification



These components provide the foundation that will be used during the implementation stages of the project.



\---



\## Database Design



The RaceDay database is designed using a relational database structure.



The database contains entities required to support the core functionality of the event management system, including:



\* Users

\* Events

\* Categories

\* Enrolments

\* Results

\* Event Documents

\* Payments

\* Weather Information



Relationships between these entities are represented in the Entity Relationship Diagram.



The database design aims to maintain data integrity, reduce unnecessary duplication, and provide appropriate relationships between users, events, participants, results, payments, and supporting information.



\---



\## Entity Relationship Diagram



The Entity Relationship Diagram provides a visual representation of the RaceDay database structure.



It identifies:



\* Database entities

\* Primary keys

\* Foreign keys

\* Relationships between entities

\* Attributes associated with the entities

\* The overall structure of the relational database



The completed ERD is available in the `docs` folder.



\*\*ERD file:\*\*



`docs/RaceDay\_ERD.png`



\---



\## SQL Database Script



The SQL database script contains the database definition required to create the RaceDay database structure.



The script includes the SQL statements required to create the relevant tables and database relationships.



\*\*Database script:\*\*



`docs/RaceDay\_Database.sql`



The script can be used as the database foundation for the later development stages of the RaceDay application.



\---



\# RESTful API Endpoint Specification



The RaceDay API Endpoint Plan defines how the future application will communicate with the backend system.



The endpoint specification documents the planned HTTP methods, routes, descriptions, request information, and access requirements for the system.



The API follows RESTful principles and uses HTTP methods such as:



\* `GET` – Retrieve information

\* `POST` – Create new information

\* `PUT` – Update existing information

\* `DELETE` – Remove information



The API planning covers functionality such as:



\* User authentication

\* User registration

\* Event management

\* Category management

\* Event enrolments

\* Results

\* Event documents

\* Payments

\* Weather information



\*\*API Endpoint Plan:\*\*



`docs/RaceDay\_API\_Endpoint\_Plan.docx`



\---



\# Planned Authentication and Authorisation



The RaceDay system includes role-based access control.



The primary system roles are:



\### Organiser



Organisers are responsible for managing events and related event information.



Planned organiser functionality includes:



\* Creating events

\* Updating events

\* Managing event information

\* Managing event categories

\* Viewing participant information

\* Managing event-related documents

\* Managing results and other event information



\### Participant



Participants use the system to interact with events.



Planned participant functionality includes:



\* Creating an account

\* Registering for events

\* Viewing available events

\* Viewing event information

\* Managing their enrolments

\* Viewing relevant results

\* Viewing payment-related information



Authentication and authorisation will be implemented during the application development stages.



\---



\# API Design



The API is planned as a backend service that allows clients to communicate with the RaceDay database.



The general structure follows:



```text

Client Application

&#x20;      |

&#x20;      v

RaceDay RESTful API

&#x20;      |

&#x20;      v

Application Services

&#x20;      |

&#x20;      v

Entity Framework Core

&#x20;      |

&#x20;      v

SQL Server Database

```



The API will provide controlled access to the application's data and functionality rather than allowing clients to communicate directly with the database.



\---



\# Project Structure



The current repository is organised as follows:



```text

RaceDay/

│

├── docs/

│   ├── RaceDay\_API\_Endpoint\_Plan.docx

│   ├── RaceDay\_Database.sql

│   └── RaceDay\_ERD.png

│

└── README.md

```



The repository structure will be expanded as development continues.



\---



\# Technologies



The RaceDay project uses or is planned to use the following technologies:



\* \*\*C#\*\*

\* \*\*ASP.NET Core Web API\*\*

\* \*\*Entity Framework Core\*\*

\* \*\*Microsoft SQL Server\*\*

\* \*\*RESTful API architecture\*\*

\* \*\*Git\*\*

\* \*\*GitHub\*\*

\* \*\*JWT / session-based authentication as specified by the project requirements\*\*

\* \*\*Unit testing\*\*

\* \*\*GitHub Actions for CI/CD\*\*



The specific technologies and implementation details may expand as subsequent portfolio stages are completed.



\---



\# Development Approach



The RaceDay system is being developed incrementally.



The project development process includes:



\### Part 1 – Planning and Database Design



\* Analyse the system requirements.

\* Design the database.

\* Create the ERD.

\* Create the SQL database script.

\* Design the RESTful API endpoints.



\### Part 2 – API Development



The planned next stage includes:



\* Creating the ASP.NET Core Web API.

\* Implementing the database using Entity Framework Core.

\* Connecting the API to SQL Server.

\* Implementing authentication and authorisation.

\* Implementing the planned API endpoints.

\* Creating unit tests.

\* Configuring continuous integration and deployment processes.



\### Future Development



Later stages may extend the system with additional application functionality, improved user interfaces, validation, security, testing, deployment, and other requirements specified in the RaceDay portfolio.



\---



\# Repository Documentation



All current Part 1 documentation is stored in the `docs` directory.



| File                             | Description                                          |

| -------------------------------- | ---------------------------------------------------- |

| `RaceDay\_ERD.png`                | Entity Relationship Diagram for the RaceDay database |

| `RaceDay\_Database.sql`           | SQL database creation and structure script           |

| `RaceDay\_API\_Endpoint\_Plan.docx` | RESTful API endpoint specification                   |



\---



\# Version Control



Git is used to track changes throughout the development of the RaceDay system.



The GitHub repository provides version history for the project and will be updated throughout the development process.



Meaningful commits are used to document major additions and changes to the project.



Current initial commit:



```text

Add RaceDay Part 1 database, ERD and API specification

```



\---



\# Project Status



\*\*Current status: Part 1 completed\*\*



\### Completed



\* \[x] Database design

\* \[x] SQL database script

\* \[x] Entity Relationship Diagram

\* \[x] RESTful API endpoint specification

\* \[x] Git repository setup

\* \[x] GitHub repository setup

\* \[x] Initial project commit

\* \[x] Project documentation README



\### Planned



\* \[ ] ASP.NET Core Web API

\* \[ ] Entity Framework Core implementation

\* \[ ] SQL Server integration

\* \[ ] Authentication and authorisation

\* \[ ] API endpoint implementation

\* \[ ] Unit testing

\* \[ ] CI/CD pipeline

\* \[ ] Further RaceDay application development



\---



\# Repository



This repository contains the development work for the RaceDay Event Management System and will be updated as additional portfolio stages are completed.



