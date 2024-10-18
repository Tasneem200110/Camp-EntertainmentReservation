# Project Setup and Running Seed Data

This section outlines the commands to navigate to the project directory and run the seed data process.

## Commands

### 1. Navigate to the Project Directory

To change the current working directory to the `PL` folder, run the following command:

```bash
cd PL
```

### 2. Run Seed Data

```bash
dotnet run seeddata
```
This command executes the application with the seeddata argument, which typically initializes the database with predefined data. Make sure the database is set up and the connection string is correctly configured in the application settings.
