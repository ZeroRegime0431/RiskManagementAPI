# Risk Management API

A RESTful API for risk intake, control management, and quarterly assessments. Built with ASP.NET Core Web API and Entity Framework Core.

## 📋 Table of Contents
- [Prerequisites](#prerequisites)
- [Setup Instructions](#setup-instructions)
- [Database Setup](#database-setup)
- [Running the API](#running-the-api)
- [Testing with Swagger](#testing-with-swagger)
- [API Endpoints](#api-endpoints)
- [Sample Data](#sample-data)
- [Business Rules](#business-rules)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)
- [Troubleshooting](#troubleshooting)
- [Decision Notes for Reviewer](#decision-notes-for-reviewer)

## 🚀 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8 SDK or later](https://dotnet.microsoft.com/download)
- [SQL Server 2022](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (optional, for viewing data)
- [Git](https://git-scm.com/) (for cloning the repository)

## 📦 Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/RiskManagementAPI.git
cd RiskManagementAPI
```

### 2. Update the Connection String

Open `appsettings.json` and update the `DefaultConnection` with YOUR SQL Server instance name:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=RiskManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

**How to find your SQL Server name:**

- Open SQL Server Management Studio (SSMS)
- In the login screen, look at the Server name field
- Or run this query in SSMS: `SELECT @@SERVERNAME`

**Common server name formats:**

- `localhost` (if using default instance)
- `.\SQLEXPRESS` (for SQL Server Express)
- `YOUR_COMPUTER_NAME\SQLEXPRESS`
- `YOUR_COMPUTER_NAME\MSSQLSERVER`

### 3. Install Required Packages

The project already has package references. Restore them with:

```bash
dotnet restore
```

### 4. Install EF Core Tools (if not already installed)

```bash
dotnet tool install --global dotnet-ef
```

### 5. Create the Database and Run Migrations

This will create the database and all tables (Risks, Controls, QuarterlyAssessments) with sample data:

```bash
dotnet ef database update
```

### 6. Verify Database Creation

Open SSMS and check that these items exist:

- Database: `RiskManagementDB`
- Tables: `Risks`, `Controls`, `QuarterlyAssessments`
- Data: 10 risks, 8 controls, 4 assessments (sample data)

## 🏃 Running the API

```bash
dotnet run
```

The API will start and show output like:

```
Now listening on: https://localhost:7118
Now listening on: http://localhost:5118
```

## 🧪 Testing with Swagger

Once the API is running, open your browser and navigate to:

```
https://localhost:7118/swagger
```

You'll see all available endpoints. You can test them directly from the browser.


## 📚 API Endpoints

### Risk Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/risks` | Get all risks (summary view) |
| GET | `/risks/{id}` | Get single risk with full details |
| POST | `/risks` | Create a new risk |
| PUT | `/risks/{id}` | Update a risk |
| DELETE | `/risks/{id}` | Delete a risk (only if Draft) |
| PATCH | `/risks/{id}/status` | Update risk status |

### Control Endpoints (nested under risks)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/risks/{riskId}/controls` | Get all controls for a risk |
| POST | `/risks/{riskId}/controls` | Add a control to a risk |
| PUT | `/risks/{riskId}/controls/{controlId}` | Update a control |
| DELETE | `/risks/{riskId}/controls/{controlId}` | Delete a control |

### Assessment Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/risks/{riskId}/assessments` | Get all assessments for a risk |
| GET | `/risks/{riskId}/assessments/latest` | Get most recent assessment |
| POST | `/risks/{riskId}/assessments` | Submit a new quarterly assessment |

## 📊 Sample Data

The database is seeded with:

### Risks (10 risks across different statuses)

| ID | Title | Category | Status | Inherent Score |
|----|-------|----------|--------|---|
| 1 | Vendor Data Breach | Compliance | Approved | 12 |
| 2 | Supply Chain Disruption | Operational | UnderReview | 10 |
| 3 | Currency Exchange Rate Volatility | Financial | Draft | 12 |
| 4 | Asia-Pacific Market Entry Failure | Strategic | Approved | 12 |
| 5 | GDPR Compliance Gaps | Compliance | Closed | 10 |
| 6 | Loss of Key Technical Leadership | Operational | UnderReview | 12 |
| 7 | Rising Interest Rates Impact on Loans | Financial | Draft | 12 |
| 8 | Ransomware Attack on Production Systems | Operational | Approved | 15 |
| 9 | AI-Powered Competitor Entering Market | Strategic | Draft | 16 |
| 10 | New Carbon Emission Regulations | Compliance | UnderReview | 12 |

### Controls (8 controls linked to risks)

- Quarterly Vendor Security Audit (Risk #1, Preventive)
- Vendor Contract Termination Clause (Risk #1, Preventive)
- Multi-Source Procurement Strategy (Risk #2, Corrective)
- Real-time Currency Hedging Program (Risk #3, Preventive)
- Weekly Currency Position Monitoring (Risk #3, Detective)
- Local Partnership Program (Risk #4, Preventive)
- 24/7 Security Monitoring (Risk #8, Detective)
- Offline Backups (Risk #8, Corrective)

### Assessments (4 assessments)

| ID | Risk | Quarter/Year | Residual Score | Risk Reduction |
|----|------|--------------|---|---|
| 1 | Vendor Data Breach | Q1 2025 | 6 | 6 |
| 2 | Vendor Data Breach | Q4 2024 | 12 | 0 |
| 3 | Asia-Pacific Market Entry | Q4 2024 | 6 | 6 |
| 4 | Ransomware Attack | Q1 2025 | 8 | 7 |


