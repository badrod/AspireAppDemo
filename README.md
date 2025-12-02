# AspireAppDemo

[![Build & Test](https://github.com/badrod/AspireAppDemo/actions/workflows/build.yml/badge.svg)](https://github.com/badrod/AspireAppDemo/actions/workflows/build.yml)
[![codecov](https://codecov.io/gh/badrod/AspireAppDemo/branch/main/graph/badge.svg)](https://codecov.io/gh/badrod/AspireAppDemo)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=AspireAppDemo&metric=alert_status)](https://sonarcloud.io/dashboard?id=AspireAppDemo)

---

## Overview

AspireAppDemo is a minimal Web API and static front‑end that demonstrates file upload and invoice extraction workflows.  
It includes a simple HTML frontend for uploading images and PDFs and a .NET Web API that accepts file uploads and returns parsed invoice data models.

---

## Features

- 📤 Upload images (JPEG/PNG) and PDFs from a browser
- 👀 Preview images and PDFs before uploading
- ⚙️ Server API endpoint to accept multipart file uploads and return parsed invoice data
- 🧾 Invoice model with rows and attached files for demonstration and testing

---

## Prerequisites

- Windows, macOS, or Linux
- Visual Studio 2026 (recommended) or Visual Studio Code
- .NET 10 SDK
- Git

Before you begin, ensure the **.NET SDK** is installed and available on your PATH.  
In Visual Studio, confirm the active project is using **.NET 10** under **Project Properties > Application**.

---

## Setup & Run (local)

1. Clone the repository:

   ```bash
   git clone https://github.com/badrod/AspireAppDemo.git
   cd AspireAppDemo

2. Restore and build the solution:

    ```bash
    dotnet restore 
    dotnet build --configuration Release
    ```

3. Run the Web API:

   ```bash
   cd AspireAppDemo
   dotnet run --configuration Release
   ```

   The API will be available at `http://localhost:5013`.
4. Open the static front‑end in a browser:

- Navigate to <https://localhost:5013/index.html> (replace {port} with the running port).
- Or open WebApi/wwwroot/index.html directly for static testing (server‑backed upload requires the API running).

## API

- POST /api/upload
- Accepts multipart/form-data with a file field
- Returns JSON representing parsed Invoice objects or an error object
  Example:
  
    ```bash
  curl -X POST <https://localhost:{port}/api/upload> \
    -F "file=@invoice.pdf"
    ```

## Front‑end

  The front‑end is located at WebApi/wwwroot/index.html.
  It provides file selection, preview (images and PDF), and uploads the file to /api/upload.
  The response JSON is pretty‑printed in the UI.

## Development Notes

- Targets C# 14 and .NET 10
- Coding standards defined in CONTRIBUTING.md and .editorconfig
- Use the IAIService abstraction for invoice parsing (List<Invoice> from List<InvoiceFile>)
- Static files → WebApi/wwwroot
- API controllers → WebApi/Controllers

## Feature Toggle

  We are using Microsoft Microsoft.FeatureManagement

```bash  
  appsettings.json
    "FeatureManagement": {
    "EnableNewFeature": true,
    "UseGoogleAI": true
  },
  ```

This enable us to toggle features without restarting the application.

We can allso use FeatureGate to add/remove endpoints dynamically

```bash  
// Feature-gated endpoint 
// Returns 404 if EnableNewFeature is false, no need for app restart if toggled
app.MapGet("/feature", () =>
{
    // Some new feature logic

}).WithFeatureGate("EnableNewFeature");
```

  Google AI Service
  AspireAppDemo integrates with Google Gemini for invoice extraction.
  By default, the AI service is enabled and used for parsing uploaded invoices.
  Configuration

- The service reads its settings from appsettings.json or environment variables.
- Required values:
- GoogleAI:Enabled → To use OpenAI set to false
- GoogleAI:ApiKey → your Gemini API key set it in env vars with

  ```bash
    dotnet user-secrets init
    dotnet user-secrets set "GoogleAISettings:ApiKey" "your-secret-key"
  ```

Example appsettings.json:
{
  "GoogleAI": {
    "Enabled": true,
    "ApiKey": "YOUR_API_KEY"
  }
}

Runtime Behavior

- When Enabled = true, the application uses GeminiInvoiceService to call the Gemini API.
- If you explicitly set Enabled = false, the application falls back to MockInvoiceService for demo parsing (no external calls).

Testing

- /Tests.csproj includes unit tests for core parsing logic
- One integration test for the upload API endpoint
- Add more unit tests for parsing and API behaviors in a tests project
- For manual tests, use index.html UI and sample invoice files

CI/CD Visibility

- ✅ Build & Test status via GitHub Actions
- 📊 Coverage reports via Codecov
- 🛡️ Quality gate via SonarCloud
Artifacts (coverage reports) are uploaded in each workflow run and can be downloaded from the Actions tab.

Contributing
Contributions are welcome!
Open issues or pull requests. Include tests for new parsing logic and follow repository coding conventions.

License & Contact

- This project is provided as a demo. Check repository settings for license information.
- For questions or issues, open an issue on GitHub: AspireAppDemo
