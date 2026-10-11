# 🏃 RaceDay Event Management System

> **A digital event management system for running, walking and cycling events.**

---

## 📌 Project Overview

**RaceDay** is an event management system designed to support the planning, administration and participation of **road running, walking and cycling events**.

The system provides a structured digital solution for managing event information, participant registrations, categories, results, payments, supporting documents and other information required throughout the event lifecycle.

This project is being developed as part of a **Portfolio of Evidence (PoE)** using an incremental development approach.

---

## 🎯 Project Objectives

The main objectives of RaceDay are to:

* 👤 Manage user accounts and user roles
* 🏁 Create and manage sporting events
* 🏷️ Manage event categories
* 📝 Manage participant enrolments
* 🏆 Record and manage event results
* 📄 Manage event-related documents
* 💳 Record payment information
* 🌦️ Store event weather information
* 🔐 Provide secure authentication and authorisation
* 🌐 Provide RESTful API functionality
* 🗄️ Store application data using a relational database

---

# 📚 Part 1 — System Planning & Database Design

Part 1 establishes the technical foundation of the RaceDay system.

The completed work includes:

| Component        | Description                               | Status |
| ---------------- | ----------------------------------------- | :----: |
| 🗄️ Database     | Relational database design and SQL script |    ✅   |
| 🔗 ERD           | Entity Relationship Diagram               |    ✅   |
| 🌐 API Design    | RESTful API endpoint specification        |    ✅   |
| 📝 Documentation | Project documentation                     |    ✅   |
| 🐙 GitHub        | Version-controlled project repository     |    ✅   |

---

# 🗄️ Database Design

The RaceDay database uses a **relational database structure** to store and manage information required by the application.

### Main Entities

* 👤 **Users**
* 🏁 **Events**
* 🏷️ **Categories**
* 📝 **Enrolments**
* 🏆 **Results**
* 📄 **Event Documents**
* 💳 **Payments**
* 🌦️ **Weather Information**

The database relationships allow information to be connected between users, events, participants, categories, results, payments and other event-related information.

### Database Script

The SQL database script contains the SQL statements required to create the RaceDay database structure.

📁 **Location:**

```text
docs/RaceDay_Database.sql
```

---

# 🔗 Entity Relationship Diagram

The **Entity Relationship Diagram (ERD)** provides a visual representation of the RaceDay database structure.

It identifies:

* Entities
* Attributes
* Primary keys
* Foreign keys
* Relationships
* Database structure

The ERD was used as a reference when creating the SQL database.

📁 **Location:**

```text
docs/RaceDay_ERD.png
```

---

# 🌐 RESTful API

The RaceDay API has been planned using **RESTful API principles**.

The API provides a structured way for the application to communicate with the backend and database.

### HTTP Methods

| Method   | Purpose                     |
| -------- | --------------------------- |
| `GET`    | Retrieve information        |
| `POST`   | Create new information      |
| `PUT`    | Update existing information |
| `DELETE` | Remove information          |

### Planned API Areas

The endpoint specification covers functionality including:

* 🔐 Authentication
* 👤 User registration
* 🏁 Event management
* 🏷️ Category management
* 📝 Event enrolments
* 🏆 Results
* 📄 Event documents
* 💳 Payments
* 🌦️ Weather information

📁 **API Documentation:**

```text
docs/RaceDay_API_Endpoint_Plan.docx
```

---

# 🔐 Authentication & Authorisation

RaceDay is designed around role-based access.

## 👨‍💼 Organiser

Organisers will manage event-related functionality.

Planned functionality includes:

* Create events
* Update events
* Manage event information
* Manage event categories
* View participant information
* Manage event documents
* Manage event results

## 🏃 Participant

Participants will interact with available events.

Planned functionality includes:

* Create an account
* Register for events
* View available events
* View event information
* Manage enrolments
* View relevant results
* Access relevant payment information

Authentication and authorisation will be implemented during the API development stage.

---

# 🏗️ System Architecture

The planned RaceDay architecture follows a layered approach:

```text
┌──────────────────────────┐
│     Client Application   │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│     RESTful Web API      │
│       ASP.NET Core       │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│    Application Logic     │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│     Entity Framework     │
│           Core           │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│      SQL Server DB       │
└──────────────────────────┘
```

This structure separates the client, API, application logic and database responsibilities.

---

# 🛠️ Technologies

The RaceDay project uses or is planned to use:

| Technology                   | Purpose                                |
| ---------------------------- | -------------------------------------- |
| 💻 **C#**                    | Backend programming language           |
| 🌐 **ASP.NET Core Web API**  | RESTful backend                        |
| 🗄️ **Microsoft SQL Server** | Relational database                    |
| 🔗 **Entity Framework Core** | Database access and ORM                |
| 🔐 **Authentication**        | User authentication and access control |
| 🧪 **Unit Testing**          | Testing application functionality      |
| 🐙 **Git**                   | Version control                        |
| 🐙 **GitHub**                | Source code repository                 |
| ⚙️ **GitHub Actions**        | CI/CD automation                       |

---

# 📂 Repository Structure

The current repository is organised as follows:

```text
RaceDay/
│
├── 📁 docs/
│   ├── 📄 RaceDay_API_Endpoint_Plan.docx
│   ├── 🗄️ RaceDay_Database.sql
│   └── 🖼️ RaceDay_ERD.png
│
└── 📄 README.md
```

The repository will expand as additional development stages are completed.

---

# 🔄 Development Roadmap

## ✅ Part 1 — Planning & Database

* [x] Database design
* [x] SQL database script
* [x] Entity Relationship Diagram
* [x] RESTful API endpoint specification
* [x] Git repository
* [x] GitHub repository
* [x] Project documentation

## 🚧 Part 2 — API Development

Planned development includes:

* [ ] ASP.NET Core Web API
* [ ] Entity Framework Core
* [ ] SQL Server integration
* [ ] Authentication
* [ ] Authorisation
* [ ] User management
* [ ] Event management
* [ ] Category management
* [ ] Enrolment management
* [ ] Results management
* [ ] Payment functionality
* [ ] Document management
* [ ] Weather information
* [ ] Unit testing
* [ ] CI/CD pipeline

## 🔮 Future Development

Future stages will continue expanding RaceDay into a complete event management solution.

This may include additional application functionality, validation, security improvements, testing, deployment and user-interface development according to the project requirements.

---

# 📦 Part 1 Documentation

All current Part 1 documentation is available inside the `docs` directory.

| File                                | Description                        |
| ----------------------------------- | ---------------------------------- |
| 📄 `RaceDay_API_Endpoint_Plan.docx` | RESTful API endpoint specification |
| 🗄️ `RaceDay_Database.sql`          | RaceDay database SQL script        |
| 🖼️ `RaceDay_ERD.png`               | Entity Relationship Diagram        |

---

# 🌱 Version Control

Git is used throughout the RaceDay project to maintain a history of development changes.

Meaningful commits are used to record major project additions and changes.

### Current Commit History

```text
Add comprehensive RaceDay project documentation
Add RaceDay Part 1 database, ERD and API specification
```

The repository will continue to be updated as each development stage is completed.

---

# 📊 Current Project Status

### 🟢 Part 1 — Completed

The current RaceDay repository contains the completed Part 1 planning and database documentation.

**Completed:**

* ✅ Database design
* ✅ SQL database script
* ✅ Entity Relationship Diagram
* ✅ RESTful API endpoint specification
* ✅ Git version control
* ✅ GitHub repository
* ✅ Project README

**Next:**

> 🚀 Begin implementation of the RaceDay ASP.NET Core Web API.

---

# 👨‍💻 Developer

**Faizaan Emmam**

RaceDay Event Management System
Portfolio of Evidence Project

---

## 📜 Project Repository

This repository contains the development work, documentation and source code for the **RaceDay Event Management System**.

The repository will be maintained and updated throughout the remaining development stages of the project.

---

**🏃 RaceDay — Manage Events. Manage Participants. Manage Results.**


