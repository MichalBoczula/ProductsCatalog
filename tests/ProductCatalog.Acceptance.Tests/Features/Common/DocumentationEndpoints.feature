Feature: Product documentation endpoints
  Documentation responses list registered flow and validation descriptors and hide dependency failures.

  Scenario Outline: <operationId> returns complete documentation
    When I request the "<operationId>" documentation endpoint
    Then the "<operationId>" documentation lists registered descriptors

    Examples:
      | operationId                |
      | DescribeAllFlows           |
      | DescribeValidationPolicies |

  Scenario Outline: <operationId> returns a safe 500 when a descriptor fails
    Given the "<operationId>" documentation descriptor fails
    When I request the "<operationId>" documentation endpoint
    Then the documentation error is safe and the descriptor was called

    Examples:
      | operationId                |
      | DescribeAllFlows           |
      | DescribeValidationPolicies |
