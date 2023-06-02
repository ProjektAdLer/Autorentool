using BusinessLogic.Entities.BackendAccess;
using NUnit.Framework;

namespace BusinessLogicTest.Entities.ApiAccess;

public class UserTokenUt
{
    [Test]
    public void Constructor_SetsAllParameters()
    {
        var userToken = "foobar";

        var systemUnderTest = new UserToken(userToken);

        Assert.That(systemUnderTest.Token, Is.EqualTo(userToken));
    }
}