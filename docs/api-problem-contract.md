# Public error contract (REF-06)

ProductsCatalog and ECommerceStoreUsers use the same `application/problem+json` shape for controlled HTTP failures:

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
  "title": "Validation failed",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/mobile-phones",
  "code": "validation_failed",
  "traceId": "request-trace-id",
  "errors": [],
  "missingProperties": []
}
```

`status` matches HTTP, `instance` is the request path without query values, and `traceId` is an opaque request identifier. Clients branch on `code`, never on `title`, `detail`, or framework exception text. `errors` contains existing `{message, name, entity}` validation entries only for domain validation; `missingProperties` contains JSON member names only when they can be inferred from the request contract and a valid JSON body. Other failures return empty arrays. The shared shape is retained; service-specific codes are identified below:

| Code | Status | Meaning |
| --- | --- | --- |
| `validation_failed` | 400 | A domain policy rejected the request. |
| `invalid_json` | 400 | JSON syntax, types, or required members are invalid. |
| `invalid_request` | 400 | Other framework body or parameter binding error. |
| `resource_not_found` | 404 | A matched endpoint did not find its resource. |
| `route_not_found` | 404 | No matching route exists. |
| `method_not_allowed` | 405 | Route exists, method does not match. |
| `concurrency_conflict` | 409 | Products conditional write lost to another writer; reload before retry. |
| `resource_conflict` | 409 | Known business conflict (currently Users). |
| `unsupported_media_type` | 415 | Unsupported request content type. |
| `internal_error` | 500 | Unexpected failure, safe public detail; underlying error only in server logs. |
| `service_unavailable` | 503 | Empty technical failure response. |

Other empty HTTP error responses use the deterministic `http_{status}` code. JSON contract inspection is limited to buffered bodies of at most 64 KiB. Large or streamed bodies still receive the same safe error and code; `missingProperties` may be empty. JSON names come from serialization metadata, never from parsing framework exception messages.

Health/readiness endpoints keep their built-in text response when they already write a body, including an unhealthy 503. This is a documented technical exception to the JSON contract. OpenAPI describes the problem media type on declared operation error responses; BDD-01/BDD-07 verify operation/status/schema coverage for the executed cases. Products update/delete retain validation 400 for an ID missing before load, and return resource-not-found 404 when a loaded row is physically removed before save.
