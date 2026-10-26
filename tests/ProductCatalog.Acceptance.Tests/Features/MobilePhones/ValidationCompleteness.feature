Feature: Complete active validation evidence

  Scenario Outline: <method> rejects blank <field> before changing catalogue data
    A present but blank string reaches the phone policy rather than JSON binding.
    Each example isolates one validation error and preserves the saved phone and history.
    Given a saved baseline phone for the validation audit
    And the catalogue and history are recorded before the rejected request
    When I submit this blank phone field
      | Field  | Value    |
      | Method | <method> |
      | Field  | <field>  |
      | Value  |          |
    Then the audit rejection contains this exact validation error
      | Field                  | Value                                   |
      | StatusCode             | 400                                     |
      | ContentType            | application/problem+json                |
      | Code                   | validation_failed                       |
      | Title                  | Validation failed                       |
      | Detail                 | One or more validation errors occurred. |
      | ErrorCount             | 1                                       |
      | MissingPropertiesCount | 0                                       |
      | ErrorName              | <rule>                                  |
      | ErrorEntity            | MobilePhone                             |
      | ErrorMessage           | <message>                               |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

    Examples:
      | scenarioId                 | method | field                         | rule                                        | message                                   |
      | REF07_Create_NameBlank      | POST   | commonDescription.name        | MobilePhonesCommonDescriptionValidationRule | Name cannot be null or whitespace.        |
      | REF07_Create_BrandBlank     | POST   | commonDescription.brand       | MobilePhonesCommonDescriptionValidationRule | Brand cannot be null or whitespace.       |
      | REF07_Create_DescriptionBlank | POST | commonDescription.description | MobilePhonesCommonDescriptionValidationRule | Description cannot be null or whitespace. |
      | REF07_Create_CameraBlank    | POST   | camera                        | MobilePhonesStringValidationRule            | Camera cannot be null or whitespace.      |
      | REF07_Create_Description3Blank | POST | description3                 | MobilePhonesStringValidationRule            | Description3 cannot be null or whitespace. |
      | REF07_Update_Description2Blank | PUT  | description2                 | MobilePhonesStringValidationRule            | Description2 cannot be null or whitespace. |

  Scenario Outline: Filtering rejects <cause> without changing catalogue data
    A valid brand keeps this request inside price validation.
    A missing bound is null; when both bounds exist the minimum must be strictly smaller.
    Given a saved baseline phone for the validation audit
    And the catalogue and history are recorded before the rejected request
    When I submit this price filter for the validation audit
      | Field    | Value  |
      | JsonBody | <json> |
    Then the audit rejection contains this exact validation error
      | Field                  | Value                                   |
      | StatusCode             | 400                                     |
      | ContentType            | application/problem+json                |
      | Code                   | validation_failed                       |
      | Title                  | Validation failed                       |
      | Detail                 | One or more validation errors occurred. |
      | ErrorCount             | 1                                       |
      | MissingPropertiesCount | 0                                       |
      | ErrorName              | <rule>                                  |
      | ErrorEntity            | MobilePhoneFilterDto                    |
      | ErrorMessage           | <message>                               |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

    Examples:
      | scenarioId                   | cause            | json                                                           | rule                                    | message                                                  |
      | REF07_Filter_NegativeMinimum | negative minimum | {"brand":"Motorola","minimalPrice":-1,"maximalPrice":null} | MobilePhoneFilterPricesValidationRule    | Minimal price must be greater than or equal to zero.      |
      | REF07_Filter_NegativeMaximum | negative maximum | {"brand":"Motorola","minimalPrice":null,"maximalPrice":-1} | MobilePhoneFilterPricesValidationRule    | Maximal price must be greater than or equal to zero.      |
      | REF07_Filter_ReversedBounds  | reversed bounds  | {"brand":"Motorola","minimalPrice":10,"maximalPrice":5}    | MobilePhoneFilterPriceRangeValidationRule | Minimal price must be lower than maximal price.           |
      | REF07_Filter_EqualBounds     | equal bounds     | {"brand":"Motorola","minimalPrice":10,"maximalPrice":10}   | MobilePhoneFilterPriceRangeValidationRule | Minimal price must be lower than maximal price.           |

  @allure.label.scenarioId:REF07_Create_NullCommonDescription
  Scenario: Creating a phone rejects a null common description before persistence
    The required member is present as JSON null, so this differs from omitting it.
    Mapping produces the default description value and the phone policy rejects it.
    Given a saved baseline phone for the validation audit
    And the catalogue and history are recorded before the rejected request
    When I submit this blank phone field
      | Field     | Value             |
      | Method    | POST              |
      | Field     | commonDescription |
      | Value     |                   |
      | ValueKind | null              |
    Then the audit rejection contains this exact validation error
      | Field                  | Value                                       |
      | StatusCode             | 400                                         |
      | ContentType            | application/problem+json                    |
      | Code                   | validation_failed                           |
      | Title                  | Validation failed                           |
      | Detail                 | One or more validation errors occurred.     |
      | ErrorCount             | 1                                           |
      | MissingPropertiesCount | 0                                           |
      | ErrorName              | MobilePhonesCommonDescriptionValidationRule |
      | ErrorEntity            | MobilePhone                                 |
      | ErrorMessage           | Common description cannot be null.          |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |
