Feature: Public API error contract
  Framework failures and unhandled exceptions expose a stable, safe problem shape.

  Scenario Outline: Framework failures have the same JSON problem contract
    When I trigger the Products REF-06 error case "<Case>"
    Then the Products REF-06 response has status <Status> and code "<Code>"

    Examples:
      | Case    | Status | Code                   | scenarioId                           |
      | route   | 404    | route_not_found        | REF06_Framework_RouteNotFound        |
      | method  | 405    | method_not_allowed     | REF06_Framework_MethodNotAllowed     |
      | media   | 415    | unsupported_media_type | REF06_Framework_UnsupportedMediaType |
      | json    | 400    | invalid_json           | REF06_Framework_InvalidJson          |
      | type    | 400    | invalid_json           | REF07_CreateMobilePhone_type         |
      | null    | 400    | invalid_json           | REF07_CreateMobilePhone_null         |
      | missing | 400    | invalid_json           | REF07_CreateMobilePhone_missing      |
      | body    | 400    | invalid_request        | REF06_Framework_EmptyBody            |
      | binding | 400    | invalid_request        | REF06_Framework_InvalidParameter     |
