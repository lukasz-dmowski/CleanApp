# CleanApp

Desktop application for managing laundry orders and customers, built with C#, WPF and MVVM.

![Dashboard](Screenshots/02%20dashboard.jpg)

## Overview

CleanApp is a portfolio project created to practice desktop application development using modern .NET technologies and clean architecture rules.

The application is designed to support small laundry businesses by providing tools for managing orders, items, users and business information while storing data in a local SQLite database.

## Technologies

- C#
- .NET
- WPF
- MVVM Pattern
- Entity Framework Core
- SQLite
- Resource Localization (PL / EN)
- Dependency Injection

## Features

### Authentication
- User login
- User roles

### Order Management
- Create orders
- Edit orders
- Create items allocated to orders
- Edit items
- Track order status
- Track item status
- Filter and search orders
- Schedule orders and manage calendar
- Create PDF with order data (ready to print)

### Application Features
- Auto-generated random orders and items for learning purposes
- Polish and English language support
- SQLite database storage
- Automatic database backup
- Validation and error handling
- Visual charts and filter shortcuts supporting user experience
- Manageable laundry info (to be displayed on PDF)
- Two-way status automation (order -> item & item -> order)
- JSON settings file creation in local app data folder

## Getting Started

On first launch the application creates two default accounts.
Please use one of these:

### Administrator

```text
Username: admin
Password: admin
```

### Employee

```text
Username: user
Password: user
```

Employee username and password can be changed later in Settings view.

## Screenshots

### Login View

![Login](Screenshots/01%20login.jpg)

### Add Order

![Add Order](Screenshots/03%20add.jpg)

### Order Details

![Order Details](Screenshots/04%20view.jpg)

### Information View

![Information](Screenshots/05%20info.jpg)

### Settings

![Settings](Screenshots/06%20settings.jpg)

## Development Journey

This project started as a simple WPF learning application and over months evolved into a larger desktop application focused on managing laundry operations.

### 05 2026
- Setting application core structure and views according to MVVM, basic navigation between views and basic buttons
- Creating collections with orders and items for initial testing
- Filtering and search functionalities
- Intoducing auto-generated test data

### 06 2026
- Adding dashboard functionalities and charts
- Basing application UI on external library / icons rather than text
- Navigation and window controls improvements

### 07 2026
- Adding PDF function
- Reorganisation of project / rethinking viewmodels + creating more services
- Adding authentication system

### 09 2026
- Moving an application to SQLite and EF Core / building SQL data structure (1 Enterprise -> N Orders -> N Items)
- Adding Settings and Info views
- Adding user roles and password management functionality
- Adding enterprise info management functionality
- Introducing database backup functionality

### 10 2026
- Adding Async/await where needed
- Implementing language versions
- Final code cleanup and improvements

## What I Learned

During the development of this project I gained practical experience with:

- MVVM architecture
- Entity Framework Core
- SQLite databases
- Dependency Injection
- Localization
- WPF user interface design
- Asynchronous programming with async/await
- Application refactoring and maintainability
- Designing relational database structures

## Future Improvements

- Advanced reporting (log)

## Author

Łukasz Dmowski

GitHub: https://github.com/lukasz-dmowski
