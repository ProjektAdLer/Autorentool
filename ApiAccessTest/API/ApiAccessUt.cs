using NUnit.Framework;

namespace ApiAccessTest.API;

[TestFixture]
public class ApiAccessUt
{
    [Test]
    public void  ApiAccess_Standard_HasAPIClass()
    {
        var apiAccess = new ApiAccess.API.ApiAccess();
        Assert.That(apiAccess, Is.Not.Null);
    }
}