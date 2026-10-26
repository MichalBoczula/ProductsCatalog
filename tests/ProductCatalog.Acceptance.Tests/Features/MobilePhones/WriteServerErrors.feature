Feature: Mobile phone write failures
  The repository throws before EF SaveChanges executes. Queued phone and history changes must remain unsaved.

  Scenario Outline: A <operationId> save failure returns a safe error without persisting changes
    The write reaches SaveChanges after staging history; a controlled repository exception must leave every saved field unchanged.
    Given a "<operationId>" mobile phone write will fail at SaveChanges
    And the catalogue and history are recorded before the rejected request
    When I perform the failing "<operationId>" mobile phone write
    Then the failing phone write returns this safe problem
      | Field                  | Value                         |
      | StatusCode             | 500                           |
      | ContentType            | application/problem+json      |
      | Code                   | internal_error                |
      | Title                  | Server error                  |
      | Detail                 | An unexpected error occurred. |
      | ErrorCount             | 0                             |
      | MissingPropertiesCount | 0                             |
      | SaveCalls              | 1                             |
      | HistoryCalls           | 1                             |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

    Examples:
      | operationId       | scenarioId                 |
      | CreateMobilePhone | REF07_CreateMobilePhone_03 |
      | UpdateMobilePhone | REF07_UpdateMobilePhone_03 |
      | DeleteMobilePhone | REF07_DeleteMobilePhone_03 |
