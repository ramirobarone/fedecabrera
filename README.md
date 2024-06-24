# TodoList

## Installation Instructions

1. Clone the repository.
2. Open the solution in Visual Studio
3. Compile and run the application.  (.NET 8)

## Comments 

Todo entity is defined as: ID, description
Example of description: "Sleep, go to play football"

![alt text](image.png)

## Comments about test

1. Test are in project TodoTestUnit
2. I use mocks for testing
3. Testing are for GET /api/todos

## Important

1. Used dependecy injection
2. Solid principles 
3. Used the Repository pattern
4. I also added Authcontroller for login with JWT and random users,  but for this instance y mark the controller TodoList as a [AllowAnonymous] for easiest check.
5. For this instance system only use a list in memory, not DB.
6. System have ILogger interface for logging errors.

## Solution

The solution have 4 projects: 

Entites: Project about the entites

Repository: Design pattern, acts as an intermediary layer between an application's business logic and controllers.

TodoList: API REST with controllers

TodoTestUnit: Project of TestUnit using XUnit framework


   