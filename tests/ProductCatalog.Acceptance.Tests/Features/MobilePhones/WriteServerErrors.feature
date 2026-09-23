Feature: Mobile phone write failures
  A controlled repository failure must produce a safe error without saving a phone or history.

  Scenario Outline: <operationId> returns a safe 500 without saving changes
    Given a "<operationId>" mobile phone write will fail at SaveChanges
    When I perform the failing "<operationId>" mobile phone write
    Then a safe error is returned and the phone and history are unchanged

    Examples:
      | operationId       |
      | CreateMobilePhone |
      | UpdateMobilePhone |
      | DeleteMobilePhone |
