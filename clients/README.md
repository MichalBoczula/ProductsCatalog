# Client generation with NSwag

This repository ships an NSwag configuration that generates:

- An Angular-friendly TypeScript client under `clients/angular/product-catalog-client.ts`.
- A reusable .NET client under `clients/dotnet/ProductCatalogClient.cs`.
- An OpenAPI snapshot used for the generation step under `clients/openapi.json`.

## Prerequisites

- .NET SDK that can build the API project (currently targeting `net10.0`).
- The `dotnet` local tools manifest in `.config/dotnet-tools.json` (restored automatically in the steps below).
- NSwag CLI 14.6.3 currently requires the `"runtime": "Net80"` setting in `nswag.clients.json`; the generated clients still target the API’s `net10.0` outputs.

## Regenerating the clients

From the repository root:

1. Restore the local NSwag tool:

   ```bash
   dotnet tool restore
   ```

2. Generate the OpenAPI document and both clients:

   ```bash
   dotnet nswag run nswag.clients.json
   ```

3. Consume the generated outputs:

   - **Angular**: import `ProductCatalogClient` from `clients/angular/product-catalog-client.ts` and provide the base URL through the generated factory method or Angular dependency injection.
   - **.NET**: reference `clients/dotnet/ProductCatalogClient.cs` from another project and pass an `HttpClient` pointing at the Product Catalog API base address.

To customize the build configuration or environment used for the OpenAPI export, adjust `nswag.clients.json`.
