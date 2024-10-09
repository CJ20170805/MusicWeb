# MusicApp

**MusicApp** is a project under development aimed at managing music files, including song uploads and cover image handling. The application follows the Domain-Driven Design (DDD) pattern and serves as a platform for organizing and playing music tracks.

## Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Status](#status)

---

## Project Overview

**MusicApp** is being developed as a music management system where users can upload their favorite songs and add album covers. The system is structured using the Domain-Driven Design (DDD) architecture to manage different parts of the app effectively. This project is still in progress, so additional features and improvements will be implemented over time.

## Features

### Completed Features

#### Admin System
- **Authentication and Authorization**: Secure login for admins to manage the system.
- **User Management**: Create, update, and delete user accounts.
- **Track Management**: Add, update, and delete music tracks.
- **Playlist Management**: Create playlists and add tracks.

#### Frontend
- **Login and Authentication**: Users can log in to access the system.

### Features Under Development

#### Admin System
- **Album Management**: Manage albums by adding, updating, and deleting them.
- **File Upload**: Upload music tracks and album cover images.

#### Frontend
- **Audio Player**: Play music tracks directly from the application.



## Tech Stack

- **ASP.NET Core 8**: The core framework for building the application.
- **DDD (Domain-Driven Design)**: Architectural pattern for domain logic.
- **Entity Framework Core**: ORM for managing database access.
- **Identity Framework**: Used for handling authentication and authorization.
- **FluentValidation**: For input validation and ensuring proper data handling.
- **MediatR**: Facilitates communication between various parts of the application using the CQRS pattern.
- **Blazor**: Used for building the user interface for the admin section.
- **Angular**: The framework for developing the frontend.

## Project Structure

The project follows a layered architecture pattern, organized by responsibility into different modules:

- **MusicApp.Admin**: The Blazor Server project for the admin system, handling UI for managing tracks, playlists, users, etc.
- **MusicApp.Application**: Contains the application logic, including use cases, services, and business logic implementation.
- **MusicApp.Domain**: The domain layer where business rules, entities, and value objects are defined.
- **MusicApp.Infrastructure**: The infrastructure layer, responsible for database access, external services, and implementing repository patterns.
- **MusicApp.WebAPI**: The ASP.NET Web API that serves as the backend for the front-end applications, providing endpoints for managing music data.
- **MusicApp.Frontend**: The frontend application developed using Angular.

## Status

**Under Development**: This project is still a work in progress. Expect additional features and bug fixes to be added in future updates.
