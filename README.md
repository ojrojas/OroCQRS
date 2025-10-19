# OroCQRS

OroCQRS is a .NET-based project that implements the CQRS (Command Query Responsibility Segregation) pattern with the goal of replacing the functionalities provided by the Mediatr library. This project offers a lightweight and extensible solution for handling commands, queries, and notifications in .NET applications.

## Key Features

- **Separation of Responsibilities**: Divides write operations (commands) and read operations (queries) into different models to improve scalability and maintainability.
- **Custom Handlers**: Defines interfaces and base classes for implementing command, query, and notification handlers.
- **Dependency Injection Extensions**: Provides extension methods to easily register handlers in dependency injection containers.
- **Practical Examples**: Includes console applications that demonstrate how to use the core library to implement commands, queries, and notifications.

## Project Structure

- **Core Library (`src/Core`)**: Contains the interfaces, base classes, and extensions needed to implement the CQRS pattern.
  - **Interfaces**: Defines key interfaces such as `ICommand`, `IQuery`, `INotification`, and their respective handlers.
  - **Extensions**: Helper methods for registering handlers in DI containers.
- **Examples (`Examples/ConsoleProjects`)**: Example projects showcasing how to use the Core library.
  - **CreateUserCommandApp**: Example implementation of a command to create a user.
  - **GetUserQueryApp**: Example implementation of a query to retrieve user information.
  - **UserCreatedNotificationApp**: Example implementation of a notification for user-created events.

## Getting Started

### Prerequisites
- .NET SDK 10.0 or higher.

### Building the Project
Run the following command in the root directory:
```bash
 dotnet build OroCQRS.sln
```

### Running Examples
Navigate to the desired example project directory and execute:
```bash
 dotnet run
```
For example, to run `CreateUserCommandApp`:
```bash
 cd Examples/ConsoleProjects/CreateUserCommandApp
 dotnet run
```
## License
This project is licensed under the MIT License. See the `LICENSE` file for details.

---

OroCQRS is a lightweight and flexible alternative to Mediatr, designed for developers seeking a customized solution to implement the CQRS pattern in their .NET applications.

## Informe de Cobertura de Código

Se ha generado un informe de cobertura de código utilizando Coverlet. A continuación, se presenta un resumen:

- **Líneas cubiertas**: 11
- **Líneas válidas**: 11
- **Porcentaje de cobertura de líneas**: 100%
- **Ramas cubiertas**: 0
- **Ramas válidas**: 0
- **Porcentaje de cobertura de ramas**: 0%

El informe detallado se encuentra disponible en el archivo `coverage.cobertura.xml` generado en la carpeta de resultados de pruebas.