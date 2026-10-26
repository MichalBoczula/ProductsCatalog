Feature: Public API error contract
  Invalid HTTP inputs return safe problems before business processing.

  @allure.label.scenarioId:REF06_Framework_RouteNotFound
  @allure.label.branchId:products.branch.http.route
  Scenario: An unknown route returns a route-not-found problem
    No endpoint matches the requested URL, so routing returns 404.
    Given the rejected HTTP request has these details
      | Field  | Value             |
      | Method | GET               |
      | Path   | /ref-06-not-found |
    And the catalogue and history are recorded before the rejected request
    When I send the rejected HTTP request
    Then the rejected HTTP response has this safe problem
      | Field                  | Value                               |
      | StatusCode             | 404                                 |
      | Code                   | route_not_found                     |
      | ContentType            | application/problem+json            |
      | Title                  | Route not found.                    |
      | Detail                 | The requested route does not exist. |
      | ErrorCount             | 0                                   |
      | MissingPropertiesCount | 0                                   |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF06_Framework_MethodNotAllowed
  @allure.label.branchId:products.branch.http.method
  Scenario: An unsupported method returns a method-not-allowed problem
    The top route supports GET; sending POST returns 405 before business processing.
    Given the rejected HTTP request has these details
      | Field  | Value              |
      | Method | POST               |
      | Path   | /mobile-phones/top |
    And the catalogue and history are recorded before the rejected request
    When I send the rejected HTTP request
    Then the rejected HTTP response has this safe problem
      | Field                  | Value                                                |
      | StatusCode             | 405                                                  |
      | Code                   | method_not_allowed                                   |
      | ContentType            | application/problem+json                             |
      | Title                  | Method not allowed.                                  |
      | Detail                 | The requested method is not supported by this route. |
      | ErrorCount             | 0                                                    |
      | MissingPropertiesCount | 0                                                    |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF06_Framework_UnsupportedMediaType
  @allure.label.branchId:products.branch.http.media
  Scenario: Creating a phone with text media returns an unsupported-media-type problem
    The create endpoint requires JSON; an explicit text/plain body returns 415.
    Given the rejected HTTP request has these details
      | Field       | Value          |
      | Method      | POST           |
      | Path        | /mobile-phones |
      | ContentType | text/plain     |
    And the rejected HTTP request has this raw body
      """
      {}
      """
    And the catalogue and history are recorded before the rejected request
    When I send the rejected HTTP request
    Then the rejected HTTP response has this safe problem
      | Field                  | Value                                      |
      | StatusCode             | 415                                        |
      | Code                   | unsupported_media_type                     |
      | ContentType            | application/problem+json                   |
      | Title                  | Unsupported media type.                    |
      | Detail                 | The request content type is not supported. |
      | ErrorCount             | 0                                          |
      | MissingPropertiesCount | 0                                          |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF06_Framework_InvalidJson
  @allure.label.branchId:products.branch.http.json-binding
  Scenario: Creating a phone with malformed JSON returns an invalid-JSON problem
    The malformed request text is preserved as evidence, while the response hides its internal marker.
    Given the rejected HTTP request has these details
      | Field       | Value            |
      | Method      | POST             |
      | Path        | /mobile-phones   |
      | ContentType | application/json |
    And the rejected HTTP request has this raw body
      """
      {INTERNAL_FAILURE_MARKER
      """
    And the catalogue and history are recorded before the rejected request
    When I send the rejected HTTP request
    Then the rejected HTTP response has this safe problem
      | Field                  | Value                               |
      | StatusCode             | 400                                 |
      | Code                   | invalid_json                        |
      | ContentType            | application/problem+json            |
      | Title                  | Invalid JSON payload.               |
      | Detail                 | The request body is not valid JSON. |
      | ErrorCount             | 0                                   |
      | MissingPropertiesCount | 0                                   |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF07_CreateMobilePhone_type
  Scenario: Creating a phone rejects a string in a boolean JSON member
    All required members are present; fingerPrint contains a JSON string instead of a boolean.
    Given the rejected HTTP request has these details
      | Field       | Value            |
      | Method      | POST             |
      | Path        | /mobile-phones   |
      | ContentType | application/json |
    And an otherwise valid create body has this JSON member replaced
      | Field     | Value           |
      | Member    | fingerPrint     |
      | JsonValue | "not-a-boolean" |
    And the catalogue and history are recorded before the rejected request
    When I send the rejected HTTP request
    Then the rejected HTTP response has this safe problem
      | Field                  | Value                               |
      | StatusCode             | 400                                 |
      | Code                   | invalid_json                        |
      | ContentType            | application/problem+json            |
      | Title                  | Invalid JSON payload.               |
      | Detail                 | The request body is not valid JSON. |
      | ErrorCount             | 0                                   |
      | MissingPropertiesCount | 0                                   |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF07_CreateMobilePhone_null
  Scenario: Creating a phone rejects null in a boolean JSON member
    All required members are present; fingerPrint is null and cannot bind to a nonnullable boolean.
    Given the rejected HTTP request has these details
      | Field       | Value            |
      | Method      | POST             |
      | Path        | /mobile-phones   |
      | ContentType | application/json |
    And an otherwise valid create body has this JSON member replaced
      | Field     | Value       |
      | Member    | fingerPrint |
      | JsonValue | null        |
    And the catalogue and history are recorded before the rejected request
    When I send the rejected HTTP request
    Then the rejected HTTP response has this safe problem
      | Field                  | Value                               |
      | StatusCode             | 400                                 |
      | Code                   | invalid_json                        |
      | ContentType            | application/problem+json            |
      | Title                  | Invalid JSON payload.               |
      | Detail                 | The request body is not valid JSON. |
      | ErrorCount             | 0                                   |
      | MissingPropertiesCount | 0                                   |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF07_CreateMobilePhone_missing
  Scenario: Creating a phone reports an omitted required JSON member
    An otherwise valid create body omits commonDescription, isolating the missing-member cause.
    Given the rejected HTTP request has these details
      | Field       | Value            |
      | Method      | POST             |
      | Path        | /mobile-phones   |
      | ContentType | application/json |
    And an otherwise valid create body omits this JSON member
      | Field  | Value             |
      | Member | commonDescription |
    And the catalogue and history are recorded before the rejected request
    When I send the rejected HTTP request
    Then the rejected HTTP response has this safe problem
      | Field                  | Value                                                                                            |
      | StatusCode             | 400                                                                                              |
      | Code                   | invalid_json                                                                                     |
      | ContentType            | application/problem+json                                                                         |
      | Title                  | Invalid JSON payload.                                                                            |
      | Detail                 | JSON payload for CreateMobilePhoneExternalDto is missing required properties: commonDescription. |
      | ErrorCount             | 0                                                                                                |
      | MissingPropertiesCount | 1                                                                                                |
      | MissingProperties      | commonDescription                                                                                |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF06_Framework_EmptyBody
  @allure.label.branchId:products.branch.http.request-binding
  Scenario: Creating a phone with an empty JSON body returns an invalid-request problem
    An explicit zero-length application/json body cannot bind to the required create DTO.
    Given the rejected HTTP request has these details
      | Field       | Value            |
      | Method      | POST             |
      | Path        | /mobile-phones   |
      | ContentType | application/json |
    And the rejected HTTP request has an empty body
    And the catalogue and history are recorded before the rejected request
    When I send the rejected HTTP request
    Then the rejected HTTP response has this safe problem
      | Field                  | Value                               |
      | StatusCode             | 400                                 |
      | Code                   | invalid_request                     |
      | ContentType            | application/problem+json            |
      | Title                  | Invalid request.                    |
      | Detail                 | The request could not be processed. |
      | ErrorCount             | 0                                   |
      | MissingPropertiesCount | 0                                   |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF06_Framework_InvalidParameter
  @allure.label.branchId:products.branch.http.request-binding
  Scenario: Browsing with a nonnumeric amount returns an invalid-request problem
    The amount query value cannot bind to an integer; it never reaches the positive-amount policy.
    Given the rejected HTTP request has these details
      | Field  | Value                              |
      | Method | GET                                |
      | Path   | /mobile-phones?amount=not-a-number |
    And the catalogue and history are recorded before the rejected request
    When I send the rejected HTTP request
    Then the rejected HTTP response has this safe problem
      | Field                  | Value                               |
      | StatusCode             | 400                                 |
      | Code                   | invalid_request                     |
      | ContentType            | application/problem+json            |
      | Title                  | Invalid request.                    |
      | Detail                 | The request could not be processed. |
      | ErrorCount             | 0                                   |
      | MissingPropertiesCount | 0                                   |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |
