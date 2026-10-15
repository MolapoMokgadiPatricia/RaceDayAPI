RaceDay API

RaceDay API is a RESTful Web API developed for an event management system that supports road running, walking, and cycling events in South Africa.

The system allows organisers to manage events, categories, participant enrolments, and race results. Participants can browse events, enrol in events, and view their enrolments and results.

The API is built using ASP.NET Core 8, Entity Framework Core 8, and SQL Server, with role-based access control to protect authorised operations.

Features
Organiser
Create, view, update, and delete events.
Manage event categories.
View enrolments for events.
Capture, update, and delete race results.
View registered users.
Participant
Browse available events and categories.
Enrol in events.
View personal enrolments.
View race results.
Manage personal profile information.
Additional Features
RESTful API architecture.
Session-based authentication.
Role-based authorisation.
Password hashing using PBKDF2-SHA256.
Database management using Entity Framework Core.
API documentation using Swagger/OpenAPI.
Automated unit testing using xUnit.
Continuous integration using GitHub Actions.
Technology Stack
Backend: ASP.NET Core 8 Web API
Programming Language: C#
Database: Microsoft SQL Server
ORM: Entity Framework Core 8 (Code-First)
Authentication: Session-based authentication
API Documentation: Swagger/OpenAPI
Testing: xUnit
CI/CD: GitHub Actions
Version Control: Git and GitHub
Project Structure
RaceDayAPISolution/
├── .github/
│   └── workflows/
│       └── dotnet.yml
├── docs/
│   ├── RaceDay.png
│   └── RaceDayDatabase.sql
├── RaceDayAPI/
│   ├── Attributes/
│   ├── Controllers/
│   ├── Data/
│   ├── Extensions/
│   ├── Models/
│   ├── Services/
│   ├── Program.cs
│   └── RaceDayAPI.csproj
├── RaceDayAPI.Tests/
└── RaceDayAPI.sln

API Endpoints
Module	Method	Endpoint	Access
Authentication	POST	/api/auth/register	Public
Authentication	POST	/api/auth/login	Public
Authentication	POST	/api/auth/logout	Authenticated
Authentication	GET	/api/auth/me	Authenticated
Events	GET	/api/events	Public
Events	GET	/api/events/{id}	Public
Events	POST	/api/events	Organiser
Events	PUT	/api/events/{id}	Organiser
Events	DELETE	/api/events/{id}	Organiser
Categories	GET	/api/categories	Public
Categories	POST	/api/categories	Organiser
Categories	PUT	/api/categories/{id}	Organiser
Categories	DELETE	/api/categories/{id}	Organiser
Enrolments	GET	/api/enrolments	Organiser
Enrolments	GET	/api/enrolments/{id}	Authenticated
Enrolments	POST	/api/enrolments	Participant
Enrolments	GET	/api/enrolments/event/{eventId}	Organiser
Results	GET	/api/results	Authenticated
Results	GET	/api/results/{id}	Authenticated
Results	POST	/api/results	Organiser
Results	PUT	/api/results/{id}	Organiser
Results	DELETE	/api/results/{id}	Organiser
Users	GET	/api/users	Organiser
Users	GET	/api/users/{id}	Authenticated
Users	PUT	/api/users/{id}	Authenticated

Note: The access levels shown represent the intended permissions and should match the application's implemented authorisation and ownership checks.

Authentication and Security

RaceDay API uses session-based authentication to manage user sessions.

Users can register as either an Organiser or a Participant.
Authenticated sessions are maintained using session cookies.
Passwords are hashed using PBKDF2-SHA256.
Role-based authorisation restricts access to protected API endpoints.
Ownership checks help protect user-specific resources.
Testing

The project uses xUnit to test key application functionality, including:

Password hashing and verification.
User registration and duplicate registration handling.
Login validation.
Role-based access control.
CRUD operations.
Resource ownership and authorisation checks.

Run the automated test suite using dotnet test.

Continuous Integration

GitHub Actions automates the project's quality checks whenever code is pushed to the repository.

The workflow:

Builds the .NET solution.
Runs the automated unit tests.
Verifies the required database documentation files.

Workflow file: .github/workflows/dotnet.yml

Documentation
Entity Relationship Diagram (ERD): docs/RaceDay.png
Database SQL Script: docs/RaceDayDatabase.sql
CI Build Screenshot: Add the verified screenshot link here.
Video Demonstration: Add the actual YouTube video link here.
Author

Name: Molapo Mokgadi Patricia
Student Number: ST10487723
Module: PROG6212 — Programming 2B
Institution: The Independent Institute of Education (IIE)
GitHub: @MolapoMokgadiPatricia
Year: 2026

AI Tools Used

Claude (Anthropic) was used for code review, debugging assistance, and guidance on ASP.NET Core best practices.

References

Anthropic, 2026. Claude AI assistant [Online]. Available at: https://claude.ai [Accessed 15 October 2026].

GitHub, 2026. GitHub Actions documentation [Online]. Available at: https://docs.github.com/en/actions [Accessed 15 October 2026].

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
