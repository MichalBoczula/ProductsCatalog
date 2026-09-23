#!/usr/bin/env bash
# Run from any directory: bash scripts/verify.sh (from the repository root).
set -euo pipefail

cd "$(dirname "$0")/.."
python3 scripts/check-acceptance-matrix.py
python3 scripts/check-description-links.py
results_dir="$PWD/artifacts/verification"
mkdir -p "$results_dir"

dotnet restore ProductsCatalog.sln
dotnet build ProductsCatalog.sln --configuration Release --no-restore
dotnet format ProductsCatalog.sln --verify-no-changes --no-restore

verify_coverage() {
  local suite="$1" assembly="$2" summary="$results_dir/$1-coverage/Summary.txt" percent
  local tool="$results_dir/tools/reportgenerator"
  if [[ ! -x "$tool" ]]; then
    dotnet tool install dotnet-reportgenerator-globaltool --tool-path "$results_dir/tools" --version 5.4.7
  fi
  "$tool" -reports:"$results_dir/$suite/**/coverage.cobertura.xml" \
    -targetdir:"$results_dir/$suite-coverage" -reporttypes:TextSummary \
    -assemblyfilters:"+$assembly"
  percent="$(sed -n 's/^[[:space:]]*Line coverage:[[:space:]]*\([0-9.]*\)%.*/\1/p' "$summary" | head -n 1)"
  [[ "$percent" =~ ^[0-9]+([.][0-9]+)?$ ]] || { echo "Missing $suite coverage" >&2; exit 1; }
  awk -v value="$percent" 'BEGIN { exit !(value + 0 >= 70) }' || {
    echo "$suite coverage $percent% is below 70%" >&2
    exit 1
  }
}

dotnet test tests/ProductCatalog.Domain.UnitTests/ProductCatalog.Domain.UnitTests.csproj \
  --configuration Release --no-restore --logger 'trx;LogFileName=domain.trx' \
  --results-directory "$results_dir/domain" --collect:'XPlat Code Coverage'
verify_coverage domain ProductCatalog.Domain

dotnet test tests/ProductsCatalog.Application.UnitTests/ProductsCatalog.Application.UnitTests.csproj \
  --configuration Release --no-restore --logger 'trx;LogFileName=application.trx' \
  --results-directory "$results_dir/application" --collect:'XPlat Code Coverage'
verify_coverage application ProductCatalog.Application

dotnet test tests/ProductsCatalog.Infrastructure.UnitTests/ProductsCatalog.Infrastructure.UnitTests.csproj \
  --configuration Release --no-restore --logger 'trx;LogFileName=infrastructure.trx' \
  --results-directory "$results_dir/infrastructure" --collect:'XPlat Code Coverage'
"$results_dir/tools/reportgenerator" -reports:"$results_dir/infrastructure/**/coverage.cobertura.xml" \
  -targetdir:"$results_dir/infrastructure-coverage" '-reporttypes:Html;TextSummary' \
  -assemblyfilters:'+ProductCatalog.Infrastructure'
test -s "$results_dir/infrastructure-coverage/Summary.txt"
dotnet test tests/ProductCatalog.Acceptance.Tests/ProductCatalog.Acceptance.Tests.csproj \
  --configuration Release --no-restore --logger 'trx;LogFileName=acceptance.trx' \
  --results-directory "$results_dir/acceptance"

ASPNETCORE_URLS=http://127.0.0.1:5050 \
ConnectionStrings__ProductCatalogDb='Server=127.0.0.1;Database=ProductsDb;User Id=sa;Password=NotUsed123!;TrustServerCertificate=True' \
Database__ApplyMigrations=false \
  dotnet run --project src/ProductCatalog.Api/ProductCatalog.Api.csproj \
  --configuration Release --no-build --no-launch-profile &
api_pid=$!
trap 'kill "$api_pid" 2>/dev/null || true' EXIT
for attempt in {1..30}; do
  if curl --fail --silent http://127.0.0.1:5050/swagger/v1/swagger.json \
    --output "$results_dir/openapi.json"; then break; fi
  sleep 1
done
test -s "$results_dir/openapi.json"
npx --yes @redocly/cli@2.53.3 lint "$results_dir/openapi.json" --extends=spec
kill "$api_pid" 2>/dev/null || true
wait "$api_pid" 2>/dev/null || true
trap - EXIT

docker build -f src/ProductCatalog.Api/Dockerfile -t productcatalogapi:verify .
echo 'Local verification passed. CI also runs dependency, secret and image vulnerability checks.'
