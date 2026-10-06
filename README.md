# Hospital Management System

A C# application built with Visual Studio for managing day-to-day hospital operations such as patient records, doctor information, and appointments.

> **Note:** Items marked with `TODO` are placeholders. Update them to match what your project actually implements.

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Database Setup](#database-setup)
- [Screenshots](#screenshots)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)
- [Author](#author)

## Features

TODO: Keep, edit, or remove the items below so they match your code.

- Patient registration and record management
- Doctor and staff management
- Appointment scheduling
- Billing and payment tracking
- Search, update, and delete records
- Login and role-based access (Admin / Doctor / Receptionist)

## Tech Stack

| Layer     | Technology                                      |
| --------- | ----------------------------------------------- |
| Language  | C#                                              |
| IDE       | Visual Studio 2022                              |
| Framework | .NET (TODO: specify, e.g. .NET Framework 4.8 / .NET 8) |
| UI        | TODO: Windows Forms / WPF / ASP.NET / Console   |
| Database  | TODO: SQL Server / MySQL / SQLite               |

## Project Structure

```
HospitalManagementSystem/
├── HospitalManagementSystem.sln        # Visual Studio solution file
├── .gitignore
├── .gitattributes
└── HospitalManagementSystem/           # Main project
    ├── HospitalManagementSystem.csproj
    └── ...                             # Source files, forms, models, etc.
```

## Prerequisites

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Community edition or higher) with the **.NET desktop development** workload (or the workload matching your project type)
- The .NET SDK/runtime version targeted by the project
- A database server if the project uses one (see [Database Setup](#database-setup))

## Getting Started

1. **Clone the repository**

   ```bash
   git clone https://github.com/JebastinMichaelRaj/HospitalManagementSystem.git
   cd HospitalManagementSystem
   ```

2. **Open the solution**

   Double-click `HospitalManagementSystem.sln`, or open it from Visual Studio via **File > Open > Project/Solution**.

3. **Restore dependencies**

   Visual Studio restores NuGet packages automatically on build. To do it manually:

   ```bash
   dotnet restore
   ```

4. **Build and run**

   - In Visual Studio: press `F5` (debug) or `Ctrl + F5` (run without debugging).
   - From the command line:

     ```bash
     dotnet build
     dotnet run --project HospitalManagementSystem
     ```

## Usage

TODO: Describe the main workflow, for example:

1. Launch the application.
2. Log in with your credentials.
3. Use the menu to add patients, schedule appointments, or manage doctors.

## Database Setup

TODO: If the project uses a database, document the setup here.

1. Create a database named `HospitalDB` (or your chosen name).
2. Run the SQL script to create the tables (add the script to the repo, e.g. `/Database/schema.sql`).
3. Update the connection string in `App.config` / `appsettings.json`:

   ```xml
   <connectionStrings>
     <add name="HospitalDB"
          connectionString="Data Source=YOUR_SERVER;Initial Catalog=HospitalDB;Integrated Security=True"
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

## Screenshots

TODO: Add screenshots of the application.

<!--
![Login Screen](docs/images/login.png)
![Dashboard](docs/images/dashboard.png)
-->

## Roadmap

- [ ] Add unit tests
- [ ] Export reports to PDF/Excel
- [ ] Email/SMS appointment reminders
- [ ] Improved authentication and audit logging

## Contributing

Contributions are welcome.

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit your changes: `git commit -m "Add your feature"`
4. Push the branch: `git push origin feature/your-feature`
5. Open a Pull Request

## License

TODO: Choose a license (for example [MIT](https://choosealicense.com/licenses/mit/)) and add a `LICENSE` file. Until then, all rights are reserved by the author.

## Author

**Jebastin Michael Raj**
