# C# UI + API Test Automation Framework

Modern **Test Automation Framework** built with **C# (.NET 8)** — combining
**Selenium WebDriver** for UI testing, **RestSharp** for API testing, and **Reqnroll** (SpecFlow-style) for BDD scenarios.
Enhanced with **Allure Reporting** and fully integrated into **GitHub Actions CI/CD**.

---

## ⚙️ Tech Stack

| Layer         | Tools / Libraries     |
| ------------- | --------------------- |
| UI Testing    | Selenium WebDriver    |
| API Testing   | RestSharp             |
| BDD Framework | Reqnroll + NUnit      |
| Reporting     | Allure.Reqnroll + TRX |
| Platform      | .NET 8.0              |

---

## 🚀 Getting Started

### 🧩 Prerequisites

* .NET SDK 8.0+
* Chrome / Edge / Firefox browser
* Allure Commandline
  Install globally:

  ```bash
  npm install -g allure-commandline
  ```

---

## ▶️ Run Tests

You can execute tests from the root or specific project level.

```bash
# Run all tests
dotnet test

# Run specific projects
dotnet test UI_Automation/UI_Automation.csproj
dotnet test API_Automation/API_Automation.csproj

# Run by category/tag (e.g. Smoke)
dotnet test --filter "Category=Smoke"
```

> 💡 Tip: Add `--logger "trx;LogFileName=test_results.trx"` to export TRX reports for CI/CD pipelines.

---

## 📊 Allure Report

After running tests, Allure result files are generated in the `allure-results/` folder.
You can generate and open the HTML report locally with:

```bash
allure generate allure-results --clean -o allure-report
allure open allure-report
```

> ⚠️ Make sure the input folder is **allure-results**, not **allure-report**.

**Allure integration notes:**

* Allure.Reqnroll automatically attaches scenario metadata and screenshots.
* Reports include: Feature → Scenario → Steps → Attachments → Logs.
* Works seamlessly with GitHub Actions for artifact uploads.

---

## 💡 Highlights

* Unified **UI** & **API** automation layers
* Page Object Model (**POM**) for clean, maintainable UI tests
* Reqnroll Hooks for driver, context, and test lifecycle management
* Integrated Allure.Reqnroll for rich BDD reporting
* Supports test categorization (Smoke, Regression, UI, API)
* GitHub Actions pipeline for continuous testing and reporting

---

## 🗁 Project Structure

```text
Csharp_Automation_Task/
├── UI_Automation/
│   ├── Features/
│   ├── Pages/
│   ├── Steps/
│   ├── Support/
│   └── UI_Automation.csproj
│
├── API_Automation/
│   ├── Client/
│   ├── Tests/
│   ├── Models/
│   └── API_Automation.csproj
│
├── .github/
│   └── workflows/
│       └── automation-tests.yml
│
├── Csharp_Automation_Task.sln
├── README.md
└── .gitignore
```

---

## 🔄 CI/CD Integration

GitHub Actions pipeline automatically:

* Builds and tests both **UI + API** layers
* Generates **Allure**, **TRX**, and optional HTML reports
* Uploads them as **GitHub Action artifacts** for download

**Workflow file:** `.github/workflows/automation-tests.yml`

Example snippet from workflow:

```yaml
- name: Run Tests
  run: dotnet test --logger "trx;LogFileName=test_results.trx"

- name: Generate Allure Report
  run: |
    allure generate allure-results --clean -o allure-report
    allure open allure-report
```

---

👤 **Author:** *Srdjan Miljus — QA Automation Engineer
