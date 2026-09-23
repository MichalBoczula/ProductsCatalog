using ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.MobilePhones.Commands;

public class MobilePhoneWriteFlowDescriptionTests
{
    [Fact]
    public void UpdateDescription_ShouldShowUnchangedBranchBeforeWrites()
    {
        var steps = new UpdateMobilePhoneCommandFlowDescribtor()
            .DescribeFlow(new UpdateMobilePhoneCommand(Guid.Empty, null!))
            .Steps.ToList();

        var branch = steps.FindIndex(step => step.Contains("information is unchanged"));
        branch.ShouldBeGreaterThan(0);
        branch.ShouldBeLessThan(steps.IndexOf("WriteHistoryToRepository"));
        branch.ShouldBeLessThan(steps.IndexOf("SaveChanges"));
    }

    [Fact]
    public void DeleteDescription_ShouldShowAlreadyInactiveBranchBeforeWrites()
    {
        var steps = new DeleteMobilePhoneCommandFlowDescribtor()
            .DescribeFlow(new DeleteMobilePhoneCommand(Guid.Empty))
            .Steps.ToList();

        var branch = steps.FindIndex(step => step.Contains("already inactive"));
        branch.ShouldBeGreaterThan(0);
        branch.ShouldBeLessThan(steps.IndexOf("WriteHistoryToRepository"));
        branch.ShouldBeLessThan(steps.IndexOf("SaveChanges"));
    }
}
