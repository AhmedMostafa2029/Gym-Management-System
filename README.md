# 🏋️ Gym Management System

A web-based Gym Management System built with **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**, following **N-Tier Architecture**.

The system provides a centralized platform for managing members, trainers, sessions, membership plans, subscriptions, bookings, attendance, and health records while applying real-world business rules and clean software development practices.

---

## 📸 Screenshots

### Dashboard

![Dashboard](screenshots/dashboard.png)
![Dashboard](screenshots/dashboard2.png)
![Dashboard](screenshots/dashboard3.png)

### Member Management

![Members](screenshots/members.png)
![Members](screenshots/members1.png)

### Trainer Management

![Trainers](screenshots/trainers.png)

### Session Management

![Sessions](screenshots/sessions.png)

### Membership Plans

![Plans](screenshots/plans.png)
![Plans](screenshots/plans1.png)

### Membership Management

![Memberships](screenshots/memberships.png)

### Booking & Attendance Management

![Bookings](screenshots/bookings.png)

![Bookings](screenshots/bookings1.png)

![Attendance](screenshots/attendance.png)

---

## ✨ Features

### 👥 Member Management

- Create, view, update, and delete members
- Upload and update member profile photos
- Manage member health records
- Search members by name, email, or phone
- Filter members by city and gender
- Server-side pagination
- Prevent duplicate emails and phone numbers
- Prevent deletion of members with future bookings

### 🏋️ Trainer Management

- Create, view, update, and delete trainers
- Upload and manage trainer profile photos
- Assign trainers to training categories
- Manage trainer information

### 📅 Session Management

- Create and manage training sessions
- Assign trainers and categories
- Filter sessions
- Track session status
- Manage session capacity and available slots

### 💳 Membership & Plan Management

- Create and manage member subscriptions
- Assign membership plans
- Track membership start and expiration dates
- Activate and deactivate plans
- Prevent assigning inactive plans to new memberships
- Apply membership-related business rules

### 🗓️ Booking & Attendance

- Manage member session bookings
- View upcoming and ongoing sessions
- Track available session capacity
- Manage member attendance
- Prevent invalid bookings

---

## 🏗️ Architecture

The project follows **N-Tier Architecture** and is divided into three main layers:

### Data Access Layer (DAL)

- Entity Models
- Entity Configurations
- DbContext
- Entity Framework Core
- Generic Repository
- Unit of Work
- Database Migrations

### Business Logic Layer (BLL)

- Application Services
- Service Interfaces
- Business Rules
- ViewModels
- AutoMapper
- Result Pattern
- Pagination and Filtering
- File Attachment Services

### Presentation Layer (PL)

- MVC Controllers
- Razor Views
- Form Validation
- Responsive User Interface

---

## 🛠️ Technologies & Tools

### Backend

- C#
- ASP.NET Core MVC
- Entity Framework Core
- LINQ
- AutoMapper

### Database

- SQL Server
- EF Core Code First
- Database Migrations

### Frontend

- Razor Views
- HTML5
- CSS3
- Bootstrap
- Bootstrap Icons
- JavaScript

### Patterns & Practices

- N-Tier Architecture
- Generic Repository Pattern
- Unit of Work Pattern
- Dependency Injection
- Result Pattern
- ViewModel Pattern
- Asynchronous Programming
- CancellationToken
- Server-Side Pagination
- Filtering and Searching

---

## 📂 Project Structure

```text
GymSystem
│
├── GymSystem.DAL
│   ├── Contexts
│   ├── Models
│   ├── Configurations
│   ├── Repositories
│   └── Migrations
│
├── GymSystem.BLL
│   ├── Common
│   ├── Services
│   ├── Utilities
│   └── ViewModels
│
├── GymSystem.PL
│   ├── Controllers
│   ├── Views
│   ├── wwwroot
│   └── Program.cs
│
├── screenshots
├── GymSystem.sln
└── README.md
```

---

## ⚖️ Business Rules

The application implements several business rules to maintain data consistency:

- Duplicate member emails and phone numbers are prevented
- Members with future bookings cannot be deleted
- Inactive plans cannot be assigned to new memberships
- Plan modifications are restricted when active memberships exist
- Session capacity and available slots are tracked
- Member bookings are validated before creation
- Booking and attendance management are handled separately
- Member and trainer images are managed through a dedicated attachment service

---

## 🚀 Getting Started

### Prerequisites

- .NET SDK
- SQL Server
- Visual Studio

### Installation

1. Clone the repository:

```bash
git clone https://github.com/AhmedMostafa2029/Gym-Management-System.git
```

2. Navigate to the project directory:

```bash
cd Gym-Management-System
```

3. Configure the database connection string in `appsettings.json`.

4. Apply database migrations:

```bash
dotnet ef database update
```

5. Run the application using Visual Studio or:

```bash
dotnet run
```

---

## 🔮 Future Improvements

- ASP.NET Core Identity
- Role-Based Authorization
- Advanced Dashboard Analytics
- Email Notifications
- Membership Expiration Notifications
- Reporting and Data Export
- Automated Testing
- Global Exception Handling

---

## 👨‍💻 Author

**Ahmed Mostafa Mohamed**

Backend .NET Developer

GitHub: **AhmedMostafa2029**

---

## 📄 License

This project was developed for educational and portfolio purposes.
