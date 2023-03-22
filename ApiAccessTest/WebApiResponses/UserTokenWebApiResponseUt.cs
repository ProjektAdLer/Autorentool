using ApiAccess.ApiResponses;
using NUnit.Framework;

namespace ApiAccessTest.WebApiResponses;

public class UserTokenWebApiResponseUt
{
    [Test]
    public void Constructor_SetsAllParameters()
    {
        var token = "asdf";
        var systemUnderTest = new UserTokenWebApiResponse(token);
        Assert.That(systemUnderTest.LmsToken, Is.EqualTo(token));
    }
}