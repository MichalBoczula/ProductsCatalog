Feature: Mobile phone write failures
  A controlled repository failure must produce a safe error without saving a phone or history.

  Scenario Outline: <operationId> returns a safe 500 without saving changes
    Given a "<operationId>" mobile phone write will fail at SaveChanges
    When I perform the failing "<operationId>" mobile phone write
    Then a safe error is returned and the phone and history are unchanged

    Examples:
      | operationId       | scenarioId                 |
      | CreateMobilePhone | REF07_CreateMobilePhone_03 |
      | UpdateMobilePhone | REF07_UpdateMobilePhone_03 |
      | DeleteMobilePhone | REF07_DeleteMobilePhone_03 |
