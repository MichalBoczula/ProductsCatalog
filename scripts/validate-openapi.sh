#!/usr/bin/env bash
set -euo pipefail

cd "$(dirname "$0")/.."
output="${1:-artifacts/verification/openapi.json}"
mkdir -p "$(dirname "$output")"
output="$(cd "$(dirname "$output")" && pwd)/$(basename "$output")"
rm -f "$output"

ASPNETCORE_URLS=http://127.0.0.1:5050 \
ConnectionStrings__ProductCatalogDb='Server=127.0.0.1;Database=ProductsDb;User Id=sa;Password=NotUsed123!;TrustServerCertificate=True' \
Database__ApplyMigrations=false \
  dotnet run --project src/ProductCatalog.Api/ProductCatalog.Api.csproj \
  --configuration Release --no-build --no-launch-profile &
api_pid=$!
cleanup() {
  kill "$api_pid" 2>/dev/null || true
  wait "$api_pid" 2>/dev/null || true
}
trap cleanup EXIT

for attempt in {1..30}; do
  if curl --fail --silent http://127.0.0.1:5050/swagger/v1/swagger.json \
    --output "$output"; then break; fi
  sleep 1
done
test -s "$output"
npx --yes @redocly/cli@2.53.3 lint "$output" --extends=spec
python3 scripts/check-openapi-contract.py "$output"
