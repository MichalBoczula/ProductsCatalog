#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "Restoring dotnet local tools..."
dotnet tool restore

echo "Generating OpenAPI document and client code..."
dotnet nswag run "${ROOT_DIR}/nswag.clients.json"

echo "Done. Outputs saved under ${ROOT_DIR}/clients."
