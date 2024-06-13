# BlogShare Service, by DW
An Azure Functions project that creates endpoints for sharing blog posts from davidwesst.com.

## Endpoints

## Normalize
- `/api/normalize`

### OpenAPI / Swagger
- `/api/openapi/{version}.{json|yaml}`
   - The OpenAPI version specification, supports `v2` (Swagger) and `v3` OpenAPI
   - Supports both `json` and `yaml`
- `/api/swagger.json`
    - Swagger (OpenAPI v2) specification
    - Equivalent to `/api/openapi/v2.{extension}`
- `/api/swagger/ui`
    - Swagger/OpenAPI human-consumable documenatation