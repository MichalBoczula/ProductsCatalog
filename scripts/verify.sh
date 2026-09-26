#!/usr/bin/env bash
set -Eeuo pipefail

cd "$(dirname "$0")/.."
stage_name="Setup"
trap 'status=$?; echo "Verification stage $stage_name failed (exit $status)." >&2' ERR
stage() {
  stage_name="$1"
  echo "==> $stage_name"
}

results_dir="$PWD/artifacts/verification"
mkdir -p "$results_dir"
rm -rf "$results_dir/domain" "$results_dir/application" "$results_dir/infrastructure" \
  "$results_dir/external-providers" "$results_dir/acceptance" \
  "$results_dir/domain-coverage" "$results_dir/application-coverage" \
  "$results_dir/infrastructure-coverage"
rm -f "$results_dir/summary.md" "$results_dir/openapi.json"

stage "Source checks"
python3 scripts/check-acceptance-matrix.py
python3 scripts/check-description-links.py
python3 scripts/test-architecture.py
python3 scripts/check-architecture.py
python3 scripts/generate-operation-links.py --output "$results_dir/operation-links.json"
python3 scripts/test-operation-links.py
python3 scripts/test-openapi-contract.py

stage "Restore and build"
solution=ProductsCatalog.sln
dotnet restore "$solution"
dotnet build "$solution" --configuration Release --no-restore

stage "Format"
dotnet format "$solution" --verify-no-changes --no-restore

run_suite() {
  local name="$1" project="$2"
  shift 2
  dotnet test "$project" --configuration Release --no-restore \
    --logger "trx;LogFileName=$name.trx" --results-directory "$results_dir/$name" "$@"
  python3 scripts/summarize-trx.py "$name" "$results_dir/$name" "$results_dir/summary.md"
}

stage "Domain"
run_suite domain tests/ProductCatalog.Domain.UnitTests/ProductCatalog.Domain.UnitTests.csproj \
  --collect 'XPlat Code Coverage'
bash scripts/report-coverage.sh "$results_dir/domain" "$results_dir/domain-coverage" \
  ProductCatalog.Domain 70 "$results_dir/summary.md"

stage "Application"
run_suite application tests/ProductsCatalog.Application.UnitTests/ProductsCatalog.Application.UnitTests.csproj \
  --collect 'XPlat Code Coverage'
bash scripts/report-coverage.sh "$results_dir/application" "$results_dir/application-coverage" \
  ProductCatalog.Application 70 "$results_dir/summary.md"

stage "Infrastructure"
run_suite infrastructure tests/ProductsCatalog.Infrastructure.UnitTests/ProductsCatalog.Infrastructure.UnitTests.csproj \
  --collect 'XPlat Code Coverage'
bash scripts/report-coverage.sh "$results_dir/infrastructure" "$results_dir/infrastructure-coverage" \
  ProductCatalog.Infrastructure "" "$results_dir/summary.md"

stage "Acceptance"
run_suite acceptance tests/ProductCatalog.Acceptance.Tests/ProductCatalog.Acceptance.Tests.csproj

stage "OpenAPI"
bash scripts/validate-openapi.sh "$results_dir/openapi.json"

stage "Docker"
docker build -f src/ProductCatalog.Api/Dockerfile -t productcatalogapi:verify .
echo 'Local verification passed. CI also runs dependency, secret and image vulnerability checks.'
