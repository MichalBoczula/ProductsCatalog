#!/usr/bin/env python3
"""Regression checks for declared, documented and exported response coverage."""

import importlib.util
import unittest
from pathlib import Path
from tempfile import TemporaryDirectory
from unittest.mock import patch


spec = importlib.util.spec_from_file_location("contract", Path(__file__).with_name("check-openapi-contract.py"))
contract = importlib.util.module_from_spec(spec)
spec.loader.exec_module(contract)

matrix_spec = importlib.util.spec_from_file_location("matrix", Path(__file__).with_name("check-acceptance-matrix.py"))
matrix = importlib.util.module_from_spec(matrix_spec)
matrix_spec.loader.exec_module(matrix)


class SourceResponseCoverageTests(unittest.TestCase):
    def test_statuses_before_and_after_name_include_nested_generic_dtos(self):
        code = '''var group = app.MapGroup("/phones");
group.MapGet("", () => Results.Ok())
    .WithName("ListPhones")
    .Produces<IReadOnlyList<PhoneDto>>(StatusCodes.Status200OK)
    .Produces<ApiProblemDetails>(StatusCodes.Status500InternalServerError);
group.MapDelete("/{id:guid}", () => Results.NoContent())
    .Produces(StatusCodes.Status204NoContent)
    .WithName("DeletePhone");
'''
        with TemporaryDirectory() as directory:
            root = Path(directory)
            (root / "PhonesEndpoints.cs").write_text(code, encoding="utf-8")
            with patch.object(matrix, "API", root):
                self.assertEqual(matrix.operations(), {
                    "ListPhones": ("GET", "/phones", {"200", "500"}),
                    "DeletePhone": ("DELETE", "/phones/{id:guid}", {"204"})})

    def test_named_operation_without_parsed_statuses_fails(self):
        with TemporaryDirectory() as directory:
            root = Path(directory)
            (root / "PhonesEndpoints.cs").write_text(
                'var group = app.MapGroup("/phones");\n'
                'group.MapGet("", () => Results.Ok()).WithName("ListPhones");', encoding="utf-8")
            with patch.object(matrix, "API", root), self.assertRaisesRegex(
                    AssertionError, "No declared response statuses parsed: ListPhones"):
                matrix.operations()

    def test_removing_delete_404_matrix_case_fails(self):
        text = matrix.MATRIX.read_text(encoding="utf-8")
        scenario = "REF08_DeleteMobilePhone_404_RemovedAfterRead"
        self.assertIn(scenario, text)
        text = "\n".join(line for line in text.splitlines() if not line.startswith(scenario + "\t")) + "\n"
        with TemporaryDirectory() as directory:
            path = Path(directory) / "acceptance-matrix.tsv"
            path.write_text(text, encoding="utf-8")
            with patch.object(matrix, "MATRIX", path), self.assertRaisesRegex(
                    AssertionError, "Missing documented response status/operation links.*DeleteMobilePhone.*404"):
                matrix.verify()


class ContractDriftTests(unittest.TestCase):
    def setUp(self):
        self.links = contract.links.generate()
        self.document = {"paths": {}, "components": {"schemas": {
            "ApiProblemDetails": {"type": "object", "properties": {
                "title": {"type": "string"}, "status": {"type": "integer"},
                "detail": {"type": "string"}, "type": {"type": "string"}}}}}}
        for item in self.links["operations"]:
            responses = {}
            for case in item["scenarios"]:
                status = case["status"]
                if status == 204:
                    responses[str(status)] = {"description": "No content"}
                else:
                    media = "application/problem+json" if status >= 400 else "application/json"
                    schema = ({"$ref": "#/components/schemas/ApiProblemDetails"} if status >= 400
                              else {"type": "object", "properties": {"id": {"type": "string"}}})
                    responses[str(status)] = {"content": {media: {"schema": schema}}}
            path = contract.normalize(item["path"])
            self.document["paths"].setdefault(path, {})[item["method"].lower()] = {
                "operationId": item["operationId"], "responses": responses}
        contract.check(self.document, self.links)

    def test_removed_status_fails(self):
        item = next(item for item in self.links["operations"] if any(case["status"] >= 400 for case in item["scenarios"]))
        response = self.document["paths"][contract.normalize(item["path"])][item["method"].lower()]["responses"]
        status = str(next(case["status"] for case in item["scenarios"] if case["status"] >= 400))
        del response[status]
        with self.assertRaisesRegex(AssertionError, "Missing OpenAPI status"):
            contract.check(self.document, self.links)

    def test_wrong_problem_media_type_fails(self):
        item = next(item for item in self.links["operations"] if any(case["status"] >= 400 for case in item["scenarios"]))
        response = self.document["paths"][contract.normalize(item["path"])][item["method"].lower()]["responses"]
        status = str(next(case["status"] for case in item["scenarios"] if case["status"] >= 400))
        response[status]["content"]["application/json"] = response[status]["content"].pop("application/problem+json")
        with self.assertRaisesRegex(AssertionError, r"application/problem\+json"):
            contract.check(self.document, self.links)

    def test_exported_delete_404_without_scenario_fails(self):
        item = next(item for item in self.links["operations"] if item["operationId"] == "DeleteMobilePhone")
        item["scenarios"] = [case for case in item["scenarios"] if case["status"] != 404]
        with self.assertRaisesRegex(AssertionError, "OpenAPI response without acceptance case: DeleteMobilePhone.*404"):
            contract.check(self.document, self.links)


if __name__ == "__main__":
    unittest.main()
