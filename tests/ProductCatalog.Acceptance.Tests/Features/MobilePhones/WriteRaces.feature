Feature: Mobile phone conditional writes
  Competing writes are applied after the pending request loads its phone.

  @allure.label.scenarioId:REF08_UpdateMobilePhone_409_ConcurrentUpdate
  Scenario: A stale update cannot overwrite a competing update
    A competing update commits after the pending update loads the old phone. The stale write and its history are rejected.
    Given an active phone exists for a conditional write
      | Field         | Value          |
      | Name          | Original Phone |
      | Brand         | Brand          |
      | MainPhoto     | original.jpg   |
      | PriceAmount   | 799.99         |
      | PriceCurrency | USD            |
    And a competing update will commit after the pending write loads the phone
      | Field | Value        |
      | Name  | Winner Phone |
    When I submit the pending phone update
      | Field | Value        |
      | Name  | Losing Phone |
    Then the pending phone write returns this response
      | Field                  | Value                                                              |
      | StatusCode             | 409                                                                |
      | ContentType            | application/problem+json                                           |
      | Code                   | concurrency_conflict                                               |
      | Title                  | Conflict.                                                          |
      | ErrorCount             | 0                                                                  |
      | MissingPropertiesCount | 0                                                                  |
      | Detail                 | The mobile phone changed during this request. Reload it and retry. |
    And the conditional write leaves this saved state and history
      | Field                   | Value             |
      | CurrentRows             | 1                 |
      | CurrentName             | Winner Phone      |
      | CurrentIsActive         | true              |
      | HistoryRows             | 2                 |
      | HistoryOperations       | Inserted, Updated |
      | LatestHistoryName       | Winner Phone      |
      | LatestHistoryIsActive   | true              |
      | CompetingActions        | 1                 |
      | PendingCurrentUnchanged | true              |
      | PendingHistoryUnchanged | true              |

  @allure.label.scenarioId:REF08_UpdateMobilePhone_409_ConcurrentDelete
  Scenario: A stale update cannot reactivate a concurrently deleted phone
    A competing soft delete commits after the pending update loads the active phone. The stale update cannot reactivate it.
    Given an active phone exists for a conditional write
      | Field         | Value          |
      | Name          | Original Phone |
      | Brand         | Brand          |
      | MainPhoto     | original.jpg   |
      | PriceAmount   | 799.99         |
      | PriceCurrency | USD            |
    And a competing soft delete will commit after the pending write loads the phone
    When I submit the pending phone update
      | Field | Value        |
      | Name  | Losing Phone |
    Then the pending phone write returns this response
      | Field                  | Value                                                              |
      | StatusCode             | 409                                                                |
      | ContentType            | application/problem+json                                           |
      | Code                   | concurrency_conflict                                               |
      | Title                  | Conflict.                                                          |
      | ErrorCount             | 0                                                                  |
      | MissingPropertiesCount | 0                                                                  |
      | Detail                 | The mobile phone changed during this request. Reload it and retry. |
    And the conditional write leaves this saved state and history
      | Field                   | Value             |
      | CurrentRows             | 1                 |
      | CurrentName             | Original Phone    |
      | CurrentIsActive         | false             |
      | HistoryRows             | 2                 |
      | HistoryOperations       | Inserted, Deleted |
      | LatestHistoryName       | Original Phone    |
      | LatestHistoryIsActive   | false             |
      | CompetingActions        | 1                 |
      | PendingCurrentUnchanged | true              |
      | PendingHistoryUnchanged | true              |

  @allure.label.scenarioId:REF08_DeleteMobilePhone_409_ConcurrentUpdate
  Scenario: A stale delete cannot deactivate a concurrently updated phone
    A competing update commits after the pending delete loads the old phone. The stale delete cannot replace the winning state.
    Given an active phone exists for a conditional write
      | Field         | Value          |
      | Name          | Original Phone |
      | Brand         | Brand          |
      | MainPhoto     | original.jpg   |
      | PriceAmount   | 799.99         |
      | PriceCurrency | USD            |
    And a competing update will commit after the pending write loads the phone
      | Field | Value        |
      | Name  | Winner Phone |
    When I submit the pending phone delete
    Then the pending phone write returns this response
      | Field                  | Value                                                              |
      | StatusCode             | 409                                                                |
      | ContentType            | application/problem+json                                           |
      | Code                   | concurrency_conflict                                               |
      | Title                  | Conflict.                                                          |
      | ErrorCount             | 0                                                                  |
      | MissingPropertiesCount | 0                                                                  |
      | Detail                 | The mobile phone changed during this request. Reload it and retry. |
    And the conditional write leaves this saved state and history
      | Field                   | Value             |
      | CurrentRows             | 1                 |
      | CurrentName             | Winner Phone      |
      | CurrentIsActive         | true              |
      | HistoryRows             | 2                 |
      | HistoryOperations       | Inserted, Updated |
      | LatestHistoryName       | Winner Phone      |
      | LatestHistoryIsActive   | true              |
      | CompetingActions        | 1                 |
      | PendingCurrentUnchanged | true              |
      | PendingHistoryUnchanged | true              |

  @allure.label.scenarioId:REF08_UpdateMobilePhone_404_RemovedAfterRead
  Scenario: An update returns not found when its loaded phone is physically removed
    The row is physically removed after loading and before saving. Only the original inserted history remains.
    Given an active phone exists for a conditional write
      | Field         | Value          |
      | Name          | Original Phone |
      | Brand         | Brand          |
      | MainPhoto     | original.jpg   |
      | PriceAmount   | 799.99         |
      | PriceCurrency | USD            |
    And the phone row will be physically removed after the pending write loads it
    When I submit the pending phone update
      | Field | Value        |
      | Name  | Losing Phone |
    Then the pending phone write returns this response
      | Field                  | Value                    |
      | StatusCode             | 404                      |
      | ContentType            | application/problem+json |
      | Code                   | resource_not_found       |
      | Title                  | Resource not found.      |
      | ErrorCount             | 0                        |
      | MissingPropertiesCount | 0                        |
      | DetailContains         | {PhoneId}                |
    And the conditional write leaves this saved state and history
      | Field                   | Value          |
      | CurrentRows             | 0              |
      | HistoryRows             | 1              |
      | HistoryOperations       | Inserted       |
      | LatestHistoryName       | Original Phone |
      | LatestHistoryIsActive   | true           |
      | CompetingActions        | 1              |
      | PendingCurrentUnchanged | true           |
      | PendingHistoryUnchanged | true           |

  @allure.label.scenarioId:REF08_DeleteMobilePhone_404_RemovedAfterRead
  Scenario: A delete returns not found when its loaded phone is physically removed
    The row is physically removed after loading and before saving. The pending delete must not add deletion history.
    Given an active phone exists for a conditional write
      | Field         | Value          |
      | Name          | Original Phone |
      | Brand         | Brand          |
      | MainPhoto     | original.jpg   |
      | PriceAmount   | 799.99         |
      | PriceCurrency | USD            |
    And the phone row will be physically removed after the pending write loads it
    When I submit the pending phone delete
    Then the pending phone write returns this response
      | Field                  | Value                    |
      | StatusCode             | 404                      |
      | ContentType            | application/problem+json |
      | Code                   | resource_not_found       |
      | Title                  | Resource not found.      |
      | ErrorCount             | 0                        |
      | MissingPropertiesCount | 0                        |
      | DetailContains         | {PhoneId}                |
    And the conditional write leaves this saved state and history
      | Field                   | Value          |
      | CurrentRows             | 0              |
      | HistoryRows             | 1              |
      | HistoryOperations       | Inserted       |
      | LatestHistoryName       | Original Phone |
      | LatestHistoryIsActive   | true           |
      | CompetingActions        | 1              |
      | PendingCurrentUnchanged | true           |
      | PendingHistoryUnchanged | true           |

  @allure.label.scenarioId:REF08_UpdateMobilePhone_200_Unchanged
  Scenario: Submitting unchanged phone details preserves the stored change time and history
    The submitted profile matches the saved phone. A successful no-op returns the same details and changes no stored field.
    Given an active phone exists for a conditional write
      | Field         | Value          |
      | Name          | Original Phone |
      | Brand         | Brand          |
      | MainPhoto     | original.jpg   |
      | PriceAmount   | 799.99         |
      | PriceCurrency | USD            |
    When I submit the saved phone profile unchanged
    Then the pending phone write returns this response
      | Field       | Value            |
      | StatusCode  | 200              |
      | ContentType | application/json |
      | Name        | Original Phone   |
      | IsActive    | true             |
    And the conditional write leaves this saved state and history
      | Field                   | Value          |
      | CurrentRows             | 1              |
      | CurrentName             | Original Phone |
      | CurrentIsActive         | true           |
      | HistoryRows             | 1              |
      | HistoryOperations       | Inserted       |
      | LatestHistoryName       | Original Phone |
      | LatestHistoryIsActive   | true           |
      | CompetingActions        | 0              |
      | PendingCurrentUnchanged | true           |
      | PendingHistoryUnchanged | true           |

  @allure.label.scenarioId:REF08_DeleteMobilePhone_200_Repeated
  Scenario: Repeating a soft delete preserves the first deletion state and history
    The first delete deactivates the phone. Repeating it returns the same inactive details without changing the stored timestamp or history.
    Given an active phone exists for a conditional write
      | Field         | Value          |
      | Name          | Original Phone |
      | Brand         | Brand          |
      | MainPhoto     | original.jpg   |
      | PriceAmount   | 799.99         |
      | PriceCurrency | USD            |
    When I delete the phone and repeat the delete
    Then the pending phone write returns this response
      | Field       | Value            |
      | StatusCode  | 200              |
      | ContentType | application/json |
      | Name        | Original Phone   |
      | IsActive    | false            |
    And the conditional write leaves this saved state and history
      | Field                   | Value             |
      | CurrentRows             | 1                 |
      | CurrentName             | Original Phone    |
      | CurrentIsActive         | false             |
      | HistoryRows             | 2                 |
      | HistoryOperations       | Inserted, Deleted |
      | LatestHistoryName       | Original Phone    |
      | LatestHistoryIsActive   | false             |
      | CompetingActions        | 0                 |
      | PendingCurrentUnchanged | true              |
      | PendingHistoryUnchanged | true              |
