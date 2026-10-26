Feature: Product documentation endpoints
  Published descriptors report source registrations. Publishing a policy does not establish that a business flow enforces it.

  Scenario Outline: Reading <operationId> returns the registered descriptor inventory
    The selected endpoint returns its exact registered names and nonempty flow steps or policy rules.
    Given the "<operationId>" documentation endpoint is selected
    When I request the "<operationId>" documentation endpoint
    Then the documentation response has this metadata
      | Field       | Value            |
      | StatusCode  | 200              |
      | ContentType | application/json |
    And the "<operationId>" documentation contains exactly its registered descriptors
      | Operation                  | Descriptor                           |
      | DescribeAllFlows           | CreateMobilePhoneCommand             |
      | DescribeAllFlows           | UpdateMobilePhoneCommand             |
      | DescribeAllFlows           | DeleteMobilePhoneCommand             |
      | DescribeAllFlows           | GetMobilePhoneByIdQuery              |
      | DescribeAllFlows           | GetMobilePhoneByIdsQuery             |
      | DescribeAllFlows           | GetMobilePhoneHistoryQuery           |
      | DescribeAllFlows           | GetMobilePhonesQuery                 |
      | DescribeAllFlows           | GetFilteredMobilePhonesQuery         |
      | DescribeAllFlows           | GetTopMobilePhonesQuery              |
      | DescribeValidationPolicies | AmountValidationPolicy               |
      | DescribeValidationPolicies | ElectronicDetailsValidationPolicy    |
      | DescribeValidationPolicies | MobilePhoneFilterValidationPolicy    |
      | DescribeValidationPolicies | MobilePhonesValidationPolicy         |
      | DescribeValidationPolicies | PaginationParametersValidationPolicy |

    Examples:
      | operationId                | scenarioId                          |
      | DescribeAllFlows           | REF07_DescribeAllFlows_01           |
      | DescribeValidationPolicies | REF07_DescribeValidationPolicies_01 |

  Scenario Outline: A <operationId> descriptor failure returns a safe documentation problem
    A controlled Describe exception returns a traceable problem while hiding its internal exception detail.
    Given the "<operationId>" documentation descriptor fails
    When I request the "<operationId>" documentation endpoint
    Then the documentation error matches this safe problem
      | Field                  | Value                         |
      | StatusCode             | 500                           |
      | ContentType            | application/problem+json      |
      | Code                   | internal_error                |
      | Title                  | Server error                  |
      | Detail                 | An unexpected error occurred. |
      | ErrorCount             | 0                             |
      | MissingPropertiesCount | 0                             |
      | DescriptorCalls        | 1                             |

    Examples:
      | operationId                | scenarioId                          |
      | DescribeAllFlows           | REF07_DescribeAllFlows_02           |
      | DescribeValidationPolicies | REF07_DescribeValidationPolicies_02 |
