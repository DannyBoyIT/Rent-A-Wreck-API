# Introduction 
This component is part of the Rent a Wreck system. <br/>
It is a web API that runs on .NET and C#. It has the responsibility of managing bookings. It has a number of endpoints for managing bookings.<br/>

Local Swagger: [https://localhost:7079/swagger/index.html](https://localhost:7152/swagger/index.html)

# Getting Started
Follow the below steps for the local setup:
1. Install .NET 8
2. Open a terminal
3. Change the directory to the repository and the RentAWreckApi folder.
4. Run `dotnet test` to run all the tests in the solution.
5. Change the directory to the Api folder.
4. Run `dotnet run` to run the application.
7. Open the browser and surf to http://localhost:5049/swagger/index.html to see the Swagger documentation and test the API endpoints.

# Pipelines
The pipelines for this repository are written in YAML.<br/>
The pipelines build the solution and run all the unit and integration tests.<br/>
