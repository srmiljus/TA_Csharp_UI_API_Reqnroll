TA_Csharp_Selenium_RestSharp

Modern Test Automation Framework built with C# (.NET 8) — combining
Selenium WebDriver for UI testing, RestSharp for API testing, and Reqnroll (SpecFlow-style) for BDD scenarios.
Enhanced with Allure Reporting and fully integrated into GitHub Actions CI/CD.

⚙️ Tech Stack
Layer	Tools / Libraries
UI Testing	Selenium WebDriver
API Testing	RestSharp
BDD Framework	Reqnroll + NUnit
Reporting	Allure.Reqnroll + TRX
Platform	.NET 8.0
🚀 Getting Started
Prerequisites

.NET SDK 8.0+

Chrome / Edge / Firefox browser

Allure Commandline (npm install -g allure-commandline)

Run Tests
# Run all tests
dotnet test

# Run specific projects
dotnet test UI_Automation/UI_Automation.csproj
dotnet test API_Automation/API_Automation.csproj

# Run by category
dotnet test --filter "Category=Smoke"

📊 Allure Report

After running tests, results are stored in allure-results/.

Generate and open the report locally:

allure generate allure-results --clean -o allure-report
allure open allure-report


Make sure the input folder is allure-results, not allure-report.

💡 Highlights

Unified UI & API test layers

Page Object Model (POM) for maintainable UI tests

Reqnroll Hooks for driver & context management

Allure.Reqnroll for BDD-driven reporting

GitHub Actions workflow for automated execution

📁 Project Structure
Csharp_Automation_Task/
├── 📂 UI_Automation/
│   ├── Features/
│   ├── Pages/
│   ├── Steps/
│   ├── Support/
│   └── UI_Automation.csproj
│
├── 📂 API_Automation/
│   ├── Client/
│   ├── Tests/
│   ├── Models/
│   └── API_Automation.csproj
│
├── 📂 .github/
│   └── workflows/
│       └── automation-tests.yml
│
├── 📄 Csharp_Automation_Task.sln
├── 📄 README.md
└── 📄 .gitignore

🔄 CI/CD Integration

GitHub Actions pipeline automatically:

Builds and tests both UI + API layers

Generates Allure, TRX, and HTML reports

Uploads reports as GitHub Action artifacts

Workflow file:
.github/workflows/automation-tests.yml

👤 Author: Srdjan Miljus — Senior QA Automation Architect
