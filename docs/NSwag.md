# NSwag client generation

This repository is configured to generate an Angular-friendly TypeScript client from the API’s OpenAPI document using NSwag.

## Prerequisites
- .NET 8 SDK installed locally or on your build agent (required for `dotnet tool restore`).
- The API project builds successfully (the NSwag step builds the API by default).

## How to generate locally
1. Restore the local tools (manifest now pins `nswag.consolecore` 14.6.3):
   ```bash
   dotnet tool restore
   ```
2. Run NSwag:
   ```bash
   dotnet nswag run nswag.json
   ```

The generated client is written to `clients/angular/api-client.ts`. Adjust the path in `nswag.json` if your Angular app lives elsewhere.

## CI/CD suggestion
1. Build the API (optional if you keep `noBuild: false` in the NSwag config):
   ```bash
   dotnet build src/ProductCatalog.Api/ProductCatalog.Api/ProductCatalog.Api.csproj -c Release
   ```
2. Restore tools (if not already done):
   ```bash
   dotnet tool restore
   ```
3. Generate the client:
   ```bash
   dotnet nswag run nswag.json
   ```
4. Publish the generated client as an npm package (e.g., to Azure Artifacts) or commit the generated file, depending on your workflow.

## Customizing the OpenAPI source
- The current configuration uses `aspNetCoreToOpenApi` to build the project and emit OpenAPI directly from the API codebase.
- If you prefer to consume a running Swagger endpoint instead, swap the `documentGenerator` section with:
  ```json
  "documentGenerator": {
    "fromSwagger": { "url": "https://localhost:5001/swagger/v1/swagger.json" }
  }
  ```

## Auth and base URL
- The generated client uses `API_BASE_URL` as an Angular injection token for the base URL. Provide this token in your Angular module or a dedicated provider.
- Add your auth handling (e.g., an `HttpInterceptor` for bearer tokens) in the consuming Angular app.
