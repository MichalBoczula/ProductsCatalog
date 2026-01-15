using System;
using Reqnroll;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public class GetMobilePhoneByIdSteps
    {
        [Given("an existing mobile phone id")]
        public void GivenAnExistingMobilePhoneId()
        {
            throw new PendingStepException();
        }

        [When("I request the mobile phone by id")]
        public void WhenIRequestTheMobilePhoneById()
        {
            throw new PendingStepException();
        }

        [Then("the mobile phone details are returned successfully")]
        public void ThenTheMobilePhoneDetailsAreReturnedSuccessfully()
        {
            throw new PendingStepException();
        }

        [Given("a mobile phone without specific id doesnt exists")]
        public void GivenAMobilePhoneWithoutSpecificIdDoesntExists()
        {
            throw new PendingStepException();
        }

        [When("I send request for mobile phone by not existed id")]
        public void WhenISendRequestForMobilePhoneByNotExistedId()
        {
            throw new PendingStepException();
        }

        [Then("response show not found error for mobile phone")]
        public void ThenResponseShowNotFoundErrorForMobilePhone()
        {
            throw new PendingStepException();
        }
    }
}
