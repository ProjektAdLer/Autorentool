using BusinessLogic.Entities.BackendAccess;
using NUnit.Framework;

namespace BusinessLogicTest.Entities.ApiAccess;

public class UserInformationUt
{
    [Test]
    public void Constructor_SetsAllParameters()
    {
        var lmsUsername = "lmsUsername";
        var lmsId = 1;
        var lmsEmail = "lmsEmail";
        var isLmsAdmin = true;

        var systemUnderTest = new UserInformation(lmsUsername, isLmsAdmin, lmsId, lmsEmail);

        Assert.That(systemUnderTest.LmsUsername, Is.EqualTo(lmsUsername));
        Assert.That(systemUnderTest.LmsId, Is.EqualTo(lmsId));
        Assert.That(systemUnderTest.LmsEmail, Is.EqualTo(lmsEmail));
        Assert.That(systemUnderTest.IsLmsAdmin, Is.EqualTo(isLmsAdmin));
    }
}