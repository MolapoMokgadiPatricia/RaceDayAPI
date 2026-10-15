RaceDay API
A RESTful Web API for the RaceDay event management system — a platform for managing road running, walking, and cycling events in South Africa.

Built with ASP.NET Core 8, Entity Framework Core, and SQL Server.

What It Does
RaceDay lets organisers create events and participants enter them. There are two user roles:

Organiser — creates/edits/deletes events, manages categories, captures results, views enrolments

Participant — browses events, enters events, views their own enrolments and results

Role-based access is enforced at the API level. Organisers can't access Participant-only endpoints and vice versa.

Tech Stack
ASP.NET Core 8 Web API

Entity Framework Core 8 (Code-First)

SQL Server

Session-based authentication

Swagger / OpenAPI

xUnit for testing

GitHub Actions for CI

Project Structure
text
RaceDayAPISolution/
├── .github/workflows/dotnet.yml     # CI workflow
├── docs/
│   ├── RaceDay.png                  # ERD diagram
│   └── RaceDayDatabase.sql          # Database script
├── RaceDayAPI/                      # API project
│   ├── Attributes/
│   ├── Controllers/
│   ├── Data/
│   ├── Extensions/
│   ├── Models/
│   ├── Services/
│   ├── Program.cs
│   └── RaceDayAPI.csproj
├── RaceDayAPI.Tests/                # Unit tests
└── RaceDayAPI.sln
How to Run It
1. Requirements
.NET 8 SDK

SQL Server Express or LocalDB

Visual Studio 2022 (or VS Code)

Git

2. Clone the repo
bash
git clone https://github.com/MolapoMokgadiPatricia/RaceDayAPI.git
cd RaceDayAPI
3. Set up the database
Open RaceDayAPI/appsettings.json and change the connection string to match your SQL Server:

json
"Server=YOUR_SERVER_NAME\\SQLEXPRESS;Database=RaceDayDB;Trusted_Connection=True;TrustServerCertificate=True;"
Then run the SQL script in docs/RaceDayDatabase.sql in SQL Server Management Studio (SSMS).

4. Run the API
bash
cd RaceDayAPI
dotnet run
5. Open Swagger
Go to https://localhost:7XXX/swagger in your browser (check the console for the exact port).

You can test every endpoint from there.

API Endpoints
Auth
Method	Route	Role	Description
POST	/api/auth/register	Public	Register as Organiser or Participant
POST	/api/auth/login	Public	Log in
POST	/api/auth/logout	Any	Log out
GET	/api/auth/me	Any	Get current user
Events
Method	Route	Role	Description
GET	/api/events	Public	List all events
GET	/api/events/{id}	Public	Get one event
POST	/api/events	Organiser	Create event
PUT	/api/events/{id}	Organiser	Update own event
DELETE	/api/events/{id}	Organiser	Delete own event
Categories
Method	Route	Role	Description
GET	/api/categories	Public	List all
GET	/api/categories/{id}	Public	Get one
GET	/api/categories/event/{eventId}	Public	Categories for event
POST	/api/categories	Organiser	Create
PUT	/api/categories/{id}	Organiser	Update own
DELETE	/api/categories/{id}	Organiser	Delete own
Enrolments
Method	Route	Role	Description
GET	/api/enrolments	Organiser	Own events
GET	/api/enrolments/{id}	Any	Get one
GET	/api/enrolments/participant/{participantId}	Any	Participant's enrolments
GET	/api/enrolments/event/{eventId}	Organiser	Event enrolments
POST	/api/enrolments	Participant	Enrol in event
PUT	/api/enrolments/{id}	Any	Update
DELETE	/api/enrolments/{id}	Any	Delete
Results
Method	Route	Role	Description
GET	/api/results	Any	View results
GET	/api/results/{id}	Any	Get one
GET	/api/results/enrolment/{enrolmentId}	Any	Result for enrolment
POST	/api/results	Organiser	Capture result
PUT	/api/results/{id}	Organiser	Update
DELETE	/api/results/{id}	Organiser	Delete
Users
Method	Route	Role	Description
GET	/api/users	Organiser	List all users
GET	/api/users/{id}	Any	Own profile
PUT	/api/users/{id}	Any	Update own profile
Authentication
Register at /api/auth/register — choose "Organiser" or "Participant"

Log in at /api/auth/login — session is created

Session cookie handles subsequent requests

Passwords are hashed with PBKDF2-SHA256

Testing
bash
dotnet test
Tests cover:

Password hashing and verification

Registration (valid, duplicate, invalid)

Login (correct, wrong password, unknown user)

Role enforcement on all controllers

CRUD operations and ownership checks

CI/CD
GitHub Actions runs on every push. The workflow:

Builds the solution

Runs all unit tests

Verifies the /docs folder has the ERD and SQL script

Green Build Screenshot
https://docs/ci-screenshot.png

Video Demo
Watch the walkthrough: https://youtu.be/YOUR_VIDEO_ID_HERE

Author
Name: Molapo Mokgadi Patricia
Student Number: ST10487723
Module: PROG6212 — Programming 2B
Institution: The Independent Institute of Education (IIE)GitHub: @MolapoMokgadiPatricia
Year: 2026


AI tools used:

Claude (Anthropic) — for code review, debugging assistance, and guidance on ASP.NET Core best practices.


References
Anthropic, 2026. Claude AI assistant [Online]. Available at: https://claude.ai [Accessed 15 October 2026].

GitHub, 2026. GitHub Actions Documentation [Online]. Available at: https://docs.github.com/en/actions [Accessed 15 October 2026].

Kalpin, N. and Lander, R., 2023. Pro ASP.NET Core 7. 10th ed. New York: Apress.

Microsoft, 2026a. ASP.NET Core documentation [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/ [Accessed 15 October 2026].

Microsoft, 2026b. Entity Framework Core documentation [Online]. Available at: https://learn.microsoft.com/en-us/ef/core/ [Accessed 15 October 2026].

Microsoft, 2026c. Session and state management in ASP.NET Core [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state [Accessed 15 October 2026].

Moriarty, K., Kalpin, N. and Lander, R., 2022. .NET MAUI and ASP.NET Core: Modern Web Development. Sebastopol, CA: O'Reilly Media.

Mozilla, 2026. HTTP authentication [Online]. Available at: https://developer.mozilla.org/en-US/docs/Web/HTTP/Authentication [Accessed 15 October 2026].

OWASP Foundation, 2024. Password Storage Cheat Sheet [Online]. Available at: https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html [Accessed 15 October 2026].

RFC Editor, 2000. RFC 2898: PKCS #5: Password-Based Cryptography Specification Version 2.0 [Online]. Available at: https://datatracker.ietf.org/doc/html/rfc2898 [Accessed 15 October 2026].

Swagger, 2026. OpenAPI Specification [Online]. Available at: https://swagger.io/specification/ [Accessed 15 October 2026].

xUnit.net, 2026. xUnit.net documentation [Online]. Available at: https://xunit.net/ [Accessed 15 October 2026].

