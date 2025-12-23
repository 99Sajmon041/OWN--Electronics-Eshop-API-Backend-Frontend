\# Electronics E-Shop



\## Project Description

This project is a full-featured e-shop application focused on backend architecture and real-world business workflows. The main goal was to design a clean, maintainable and scalable solution with a strong separation of concerns and proper use of design patterns.



The application allows customers to browse products, manage a shopping cart, place orders and manage their accounts. An administrative section provides order management, order status updates and overall control of the order lifecycle.



---



\## Key Features

\- User registration, authentication and account management

\- Shopping cart functionality (add items, update quantity, remove items)

\- Order creation and processing

\- Order lifecycle management (new, paid, shipped, completed, canceled)

\- Administrative order management

\- Email sending (e.g. support contact)

\- Input validation and error handling



---



\## Technologies \& Architecture

\- ASP.NET Core

\- Clean Architecture

\- CQRS

\- MediatR

\- Entity Framework Core

\- Repository and Unit of Work patterns

\- ASP.NET Identity (authentication \& authorization)

\- REST API

\- Logging and custom application exceptions



---



\## Architectural Overview

The solution is divided into logical layers:

\- \*\*Domain\*\* – entities, enums and interfaces

\- \*\*Application\*\* – business logic, commands, handlers and validation

\- \*\*Infrastructure\*\* – database access, repositories and external services

\- \*\*API / Web\*\* – application interface



This structure ensures testability, scalability and long-term maintainability.



---



\## Project Status

The project is completed. All core features have been implemented, tested and serve as a reference backend e-commerce application.

