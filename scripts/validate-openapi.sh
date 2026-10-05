#!/usr/bin/env bash
set -euo pipefail

cd "$(dirname "$0")/.."
output="${1:-artifacts/verification/openapi.json}"
mkdir -p "$(dirname "$output")"
output="$(cd "$(dirname "$output")" && pwd)/$(basename "$output")"
exports_dir="$(dirname "$output")"
rm -f "$output" "$exports_dir/flows.json" "$exports_dir/validation-policies.json"

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

curl --fail --silent --show-error http://127.0.0.1:5050/products-documentation/flow \
  --output "$exports_dir/flows.json"
curl --fail --silent --show-error http://127.0.0.1:5050/products-documentation/validation-policies \
  --output "$exports_dir/validation-policies.json"
python3 - "$exports_dir" <<'CHECK'
import json
from pathlib import Path
import sys
for name in ["flows.json", "validation-policies.json"]:
    value = json.loads((Path(sys.argv[1]) / name).read_text())
    if not isinstance(value, (dict, list)) or not value:
        raise SystemExit(f"Missing structured documentation: {name}")
CHECK
