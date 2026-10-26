Feature: Mobile phone conditional writes

  @allure.label.scenarioId:REF08_UpdateMobilePhone_409_ConcurrentUpdate
  Scenario: REF08_UpdateMobilePhone_409_ConcurrentUpdate
    Given a phone write is interrupted by a concurrent update
    When the pending update is submitted
    Then the conditional phone write returns 409 and preserves only the winning history

  @allure.label.scenarioId:REF08_UpdateMobilePhone_409_ConcurrentDelete
  Scenario: REF08_UpdateMobilePhone_409_ConcurrentDelete
    Given a phone write is interrupted by a concurrent delete
    When the pending update is submitted
    Then the conditional phone write returns 409 and preserves only the winning history

  @allure.label.scenarioId:REF08_DeleteMobilePhone_409_ConcurrentUpdate
  Scenario: REF08_DeleteMobilePhone_409_ConcurrentUpdate
    Given a phone write is interrupted by a concurrent update
    When the pending delete is submitted
    Then the conditional phone write returns 409 and preserves only the winning history

  @allure.label.scenarioId:REF08_UpdateMobilePhone_404_RemovedAfterRead
  Scenario: REF08_UpdateMobilePhone_404_RemovedAfterRead
    Given a phone write is interrupted by a physical removal
    When the pending update is submitted
    Then the conditional phone write returns 404 and preserves only the winning history

  @allure.label.scenarioId:REF08_DeleteMobilePhone_404_RemovedAfterRead
  Scenario: Delete returns not found when the phone is removed after loading
    Given a phone write is interrupted by a physical removal
    When the pending delete is submitted
    Then the conditional phone write returns 404 and preserves only the winning history

  @allure.label.scenarioId:REF08_UpdateMobilePhone_200_Unchanged
  Scenario: REF08_UpdateMobilePhone_200_Unchanged
    Given a phone exists for an unchanged update
    When the unchanged update is submitted
    Then the conditional phone write returns 200 and preserves only the winning history

  @allure.label.scenarioId:REF08_DeleteMobilePhone_200_Repeated
  Scenario: REF08_DeleteMobilePhone_200_Repeated
    Given a phone exists for a repeated delete
    When the repeated delete is submitted
    Then the conditional phone write returns 200 and preserves only the winning history
