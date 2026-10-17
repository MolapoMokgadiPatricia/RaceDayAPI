# RaceDay API

A RESTful Web API for the **RaceDay** event management system — a platform for managing road running, walking, and cycling events in South Africa.

Built with **ASP.NET Core 8**, **Entity Framework Core**, and **SQL Server**.

## Description

RaceDay allows event organisers to create and manage sporting events, define categories, and capture participant results. Participants can browse upcoming events, enrol in categories, and view their personal race history.

This repository contains **Part 2** of the POE: the fully functional RESTful API that powers the system. Part 1 planning documents (ERD, endpoint plan, SQL script) are in the `/docs` folder.

## The Two Roles

The system supports two distinct user types:

###  Organiser
- Create, update, and delete their own events
- Define age or distance categories for events
- View all enrolments for their own events
- Capture finish times and finishing positions for participants
- Cannot access Participant-only functionality

###  Participant
- Register and log in
- Browse all upcoming events
- Enrol in an event by selecting a category
- View their own enrolments
- View their personal results and race history
- Cannot access Organiser-only functionality

**Role-based access is enforced at the API level** using a custom `[AuthorizeRole]` attribute.

## Tech Stack

- ASP.NET Core 8 Web API
- Entity Framework Core 8 (Code-First)
- SQL Server
- Session-based authentication
- Swagger / OpenAPI 3
- xUnit + EF Core InMemory for testing
- GitHub Actions for CI/CD

## Project Structure

```text
RaceDayAPISolution/
├── .github/workflows/dotnet.yml      # CI/CD workflow
├── docs/
│   ├── RaceDay.png                   # ERD diagram
│   ├── RaceDayDatabase.sql           # Database creation + seed script
│   └── ci-screenshot.png             # GitHub Actions green build screenshot
├── RaceDayAPI/                       # Main API project
│   ├── Attributes/
│   │   └── AuthorizeRoleAttribute.cs
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── CategoriesController.cs
│   │   ├── EnrolmentsController.cs
│   │   ├── EventsController.cs
│   │   ├── ResultsController.cs
│   │   └── UsersController.cs
│   ├── Data/
│   │   └── RaceDayContext.cs
│   ├── Extensions/
│   │   └── SessionExtensions.cs
│   ├── Models/
│   │   ├── Category.cs
│   │   ├── Enrolment.cs
│   │   ├── Event.cs
│   │   ├── Organiser.cs
│   │   ├── Participant.cs
│   │   ├── Result.cs
│   │   └── User.cs
│   ├── Services/
│   │   └── PasswordService.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── RaceDayAPI.csproj
├── RaceDayAPI.Tests/                 # xUnit test project
│   ├── Helpers/
│   │   └── TestHelpers.cs
│   ├── AuthControllerTests.cs
│   ├── CategoriesControllerTests.cs
│   ├── EnrolmentsControllerTests.cs
│   ├── EventsControllerTests.cs
│   ├── PasswordServiceTests.cs
│   ├── ResultsControllerTests.cs
│   └── RaceDayAPI.Tests.csproj
├── .gitattributes
├── .gitignore
├── README.md
└── RaceDayAPI.sln

## Setup and Run Instructions

### Prerequisites

Before running this project, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or LocalDB
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recommended) or Visual Studio Code
- Git

### Step 1: Clone the repository

Open your terminal and clone the repository:

```bash
git clone https://github.com/MolapoMokgadiPatricia/RaceDayAPI.git
cd RaceDayAPI


Method	Route	Role	Description
POST	/api/auth/register	Public	Register as Organiser or Participant
POST	/api/auth/login	Public	Log in and start a session
POST	/api/auth/logout	Any	End the current session
GET	/api/auth/me	Any	Get the current user's profile
Events — /api/events
Method	Route	Role	Description
GET	/api/events	Public	List all events
GET	/api/events/{id}	Public	Get one event with categories
POST	/api/events	Organiser	Create a new event
PUT	/api/events/{id}	Organiser	Update own event
DELETE	/api/events/{id}	Organiser	Delete own event
Categories — /api/categories
Method	Route	Role	Description
GET	/api/categories	Public	List all categories
GET	/api/categories/{id}	Public	Get one category
GET	/api/categories/event/{eventId}	Public	Categories for an event
POST	/api/categories	Organiser	Create category for own event
PUT	/api/categories/{id}	Organiser	Update own category
DELETE	/api/categories/{id}	Organiser	Delete own category
Enrolments — /api/enrolments
Method	Route	Role	Description
GET	/api/enrolments	Organiser	All enrolments for own events
GET	/api/enrolments/{id}	Any	Get one enrolment (owner only)
GET	/api/enrolments/participant/{id}	Any	Enrolments for a participant
GET	/api/enrolments/event/{id}	Organiser	Enrolments for own event
POST	/api/enrolments	Participant	Enrol in an event
PUT	/api/enrolments/{id}	Any	Update own enrolment
DELETE	/api/enrolments/{id}	Any	Delete own enrolment
Results — /api/results
Method	Route	Role	Description
GET	/api/results	Any	Results (own or owned events)
GET	/api/results/{id}	Any	Get one result
GET	/api/results/enrolment/{id}	Any	Result for an enrolment
POST	/api/results	Organiser	Capture a result
PUT	/api/results/{id}	Organiser	Update a result
DELETE	/api/results/{id}	Organiser	Delete a result
Users — /api/users
Method	Route	Role	Description
GET	/api/users	Organiser	List all users
GET	/api/users/{id}	Any	Get own profile
PUT	/api/users/{id}	Any	Update own profile
Authentication
The API uses session-based authentication:

Register at POST /api/auth/register — select "Organiser" or "Participant"

Log in at POST /api/auth/login — session is created

Session cookie (.RaceDay.Session) is returned to the client

Subsequent requests include the cookie automatically

The [AuthorizeRole] attribute checks session on every protected endpoint

Passwords are hashed with PBKDF2-SHA256 using a per-user random salt (100,000 iterations). Plaintext passwords are never stored.

Testing
The test project uses xUnit with EF Core InMemory for isolated unit tests.

Run all tests:

bash
dotnet test
Test coverage:

Area	Tests
Password hashing	Hash uniqueness, verify correct/incorrect, malformed hash handling
Registration	Valid participant, valid organiser, duplicate email, missing fields, invalid role
Login	Correct credentials, wrong password, unknown email, session values
Events	Public listing, organiser CRUD, ownership enforcement
Categories	Organiser create/delete, ownership enforcement, validation
Enrolments	Participant enrolment, duplicate prevention, role checks
Results	Organiser capture, participant view, ownership enforcement
All 45 tests pass locally and through GitHub Actions CI.

CI/CD
GitHub Actions runs on every push and pull request to main, master, part-2, and part-3 branches.

The workflow file (.github/workflows/dotnet.yml) performs two jobs:

build-and-test — Sets up .NET 8, restores, builds, and runs all 45 unit tests

validate-docs — Verifies the /docs folder contains the ERD image and SQL script

# Green Build Screenshot
<img width="1358" height="671" alt="image" src="https://github.com/user-attachments/assets/5848c9f6-e159-4c88-91eb-0ecd825f0fa4" />


The screenshot above shows a successful CI run with all jobs passing.

# Video Demonstration
An unlisted YouTube video demonstrating the running API is available here:

Watch the Part 2 Demo: https://youtu.be/YOUR_VIDEO_ID_HERE

The video covers:

Project structure and code walkthrough

Running the API and testing endpoints via Swagger

Session-based login and role enforcement demonstration

Running unit tests locally (45 passing)

Overview of the CI/CD pipeline and green build

Author
Name: Molapo Mokgadi Patricia
Student Number: ST10487723
Module: PROG6212 — Programming 2B
GitHub: @MolapoMokgadiPatricia
Year: 2026

AI Usage Declaration
In line with the IIE's Intellectual Integrity and Property Rights Policy (IIE023) and the requirements of this POE, the use of AI tools during this project is disclosed as follows:

AI tools used:

Claude (Anthropic) — for code review, debugging assistance, and guidance on ASP.NET Core best practices.

How AI was used:

Reviewing controller structure and suggesting improvements to role-based authorization patterns.

Assisting with debugging build and CI/CD pipeline errors.

Providing guidance on xUnit test structure and Entity Framework Core relationship configuration.

Formatting and structuring this README.

How AI was NOT used:

No AI-generated code was submitted as the student's own original work.

All code was reviewed, understood, and where appropriate, modified by the student.

The system design, ERD, database schema, and API endpoint plan were produced by the student as part of Part 1.

AI was not used to generate entire files without the student's direct involvement and understanding.

Declaration:
I confirm that I understand all code submitted in this repository, that the design decisions reflect my own work, and that AI assistance has been limited to guidance, review, and debugging support as described above. I take full responsibility for the correctness and originality of this submission.

References
Anthropic, 2026. Claude AI assistant [Online]. Available at: https://claude.ai [Accessed 17 October 2026].

GitHub, 2026. GitHub Actions documentation [Online]. Available at: https://docs.github.com/en/actions [Accessed 17 October 2026].

Kalpin, N. and Lander, R., 2023. Pro ASP.NET Core 7. 10th ed. New York: Apress.

Microsoft, 2026a. ASP.NET Core documentation [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/ [Accessed 17 October 2026].

Microsoft, 2026b. Entity Framework Core documentation [Online]. Available at: https://learn.microsoft.com/en-us/ef/core/ [Accessed 17 October 2026].

Microsoft, 2026c. Session and state management in ASP.NET Core [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state [Accessed 17 October 2026].

Mozilla, 2026. HTTP authentication [Online]. Available at: https://developer.mozilla.org/en-US/docs/Web/HTTP/Authentication [Accessed 17 October 2026].

OWASP Foundation, 2024. Password Storage Cheat Sheet [Online]. Available at: https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html [Accessed 17 October 2026].

RFC Editor, 2000. RFC 2898: PKCS #5: Password-Based Cryptography Specification Version 2.0 [Online]. Available at: https://datatracker.ietf.org/doc/html/rfc2898 [Accessed 17 October 2026].

Swagger, 2026. OpenAPI Specification [Online]. Available at: https://swagger.io/specification/ [Accessed 17 October 2026].

xUnit.net, 2026. xUnit.net documentation [Online]. Available at: https://xunit.net/ [Accessed 17 October 2026].

