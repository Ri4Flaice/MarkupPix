# MarkupPix
This project is a ASP.NET A basic web API designed for document management and verification. 
It has an internal infrastructure that includes database integration, Redis, and Docker containerization. 
The API allows administrators to manage user data. For some roles, you can create documents and organize the pages associated with these documents.

## Requirements
- Docker

## Installation
1. Download or clone the project to your PC.
2. Open the "Backend" folder, which is located in the project, open a command prompt in this folder and run docker.
3. Enter the command "docker-compose up --build -d" and wait for the project to start. If the project did not start the first time, run the "docker-compose stop" command and then the "docker-compose up --build -d" command.
4. After launching the project, open the browser and enter the url "http://localhost:8080/swagger ".

## Languages and technologies
- C#
- ASP.NET Core WebAPI
- .NET 8
- Docker
- Redis
- Entity Framework Core
- MySQL
