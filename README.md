# AspireAppDemo

## Overview

AspireAppDemo is a minimal Web API and static front-end that demonstrates file upload and invoice extraction workflows. It includes a simple HTML frontend for uploading images and PDFs and a .NET Web API that accepts file uploads and returns parsed invoice data models.

## Features

- Upload images (JPEG/PNG) and PDFs from a browser.
- Preview images and PDFs in the browser before uploading.
- Server API endpoint to accept multipart file uploads and return parsed invoice data.
- Invoice model with rows and attached files for demonstration and testing.

## Prerequisites

- Windows, macOS, or Linux
- Visual Studio 2026 (recommended) or Visual Studio Code
- .NET 10 SDK
- Git

Before you begin, ensure the __.NET SDK__ is installed and available on your PATH. In Visual Studio, confirm the active project is using __.NET 10__ under __Project Properties > Application__.

## Setup & Run (local)

1. Clone the repository:

````````

2. Restore and build the solution (CLI):
````````

3. Run the Web API (from the solution or CLI):
````````

In Visual Studio 2026, set the `WebApi` project as the startup project and start debugging or run without debugging.

4. Open the static front-end in a browser:

- If the app serves static files, navigate to `https://localhost:{port}/index.html` (replace `{port}` with the running port).
- Or open `WebApi/wwwroot/index.html` directly in a browser for static testing (server-backed upload requires the API to be running).

## API

- POST `/api/upload`
- Accepts `multipart/form-data` with a `file` field.
- Returns JSON representing parsed `Invoice` objects or an error object.

Example curl:

````````

## Front-end

The front-end is located at `WebApi/wwwroot/index.html`. It provides file selection, preview (images and PDF), and uploads the file to `/api/upload`. The response JSON is pretty-printed in the UI.

## Development Notes

- The project targets C# 14 and .NET 10. Ensure tools are configured accordingly.
- Follow project coding standards defined in `CONTRIBUTING.md` and `.editorconfig` (when present).
- Use the `IAIService` abstraction for invoice reading/parsing. Implementations should return a `List<Invoice>` given `List<InvoiceFile>` inputs.
- Static files live in `WebApi/wwwroot`. API controllers live in `WebApi/Controllers`.

## Testing

/Tests.csproj
- Includes unit tests for core parsing logic.
- One integration test for the upload API endpoint is provided.
- No automated tests are included by default. Add unit tests for parsing and API behaviors in a `tests` project.
- For quick manual tests use the provided `index.html` UI and sample invoice files.

## Contributing

Contributions are welcome. Open issues or pull requests. Include tests for new parsing logic and follow repository coding conventions.

## License & Contact

- This project is provided as a demo. Check repository settings for license information.
- For questions or issues, open an issue on the GitHub repository: https://github.com/badrod/AspireAppDemo