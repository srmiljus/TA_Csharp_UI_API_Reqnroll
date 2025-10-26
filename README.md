# TA_Csharp_Selenium_RestSharp

Modern **Test Automation Framework** built with **C# (.NET 8)**, combining **Selenium WebDriver** for UI testing, **RestSharp** for API testing, and **Reqnroll** (SpecFlow-style) for BDD scenarios.  
Enhanced with **Allure Reporting** and fully integrated into **GitHub Actions** CI/CD.

---

## Tech Stack

| Layer | Tools / Libraries |
|-------|--------------------|
| UI Testing | Selenium WebDriver |
| API Testing | RestSharp |
| BDD Framework | Reqnroll + NUnit |
| Reporting | Allure.Reqnroll + TRX |
| Platform | .NET 8.0 |

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Chrome / Edge / Firefox browser
- [Allure Commandline](https://docs.qameta.io/allure/)  
  Install globally:
  ```bash
  npm install -g allure-commandline
Run Tests
bash
Copy code
dotnet test                              # Run all tests
dotnet test UI_Automation/UI_Automation.csproj
dotnet test API_Automation/API_Automation.csproj
dotnet test --filter "Category=Smoke"    # Filter by tag
Allure Report
After running tests, reports are generated in allure-results/.
To create and open the report locally:

bash
Copy code
allure generate allure-results --clean -o allure-report
allure open allure-report
Input folder must be allure-results, not allure-report.

Highlights
Unified UI & API test layers

Page Object Model (POM) for maintainable UI tests

Reqnroll Hooks for driver and context management

Allure.Reqnroll integrated for BDD-driven reporting

GitHub Actions workflow for automated execution

Project Structure
swift
Copy code
Csharp_Automation_Task/
 ┣ 📁 API_Automation/
 ┣ 📁 UI_Automation/
 ┣ 📁 .github/workflows/
 ┣ 📄 Csharp_Automation_Task.sln
 ┣ 📄 README.md
 ┗ 📄 .gitignore
CI/CD Integration
The pipeline automatically:

Builds and tests both layers (UI + API)

Generates Allure, TRX, and HTML reports

Uploads them as GitHub Action artifacts

Workflow file: .github/workflows/automation-tests.yml

👤 Author
Srdjan Miljus — Senior QA Automation Architect
