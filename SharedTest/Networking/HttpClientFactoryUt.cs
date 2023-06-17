using NUnit.Framework;
using Shared.Networking;

namespace SharedTest.Networking;

[TestFixture]
public class HttpClientFactoryUt
{
    [Test]
    public void CreateClient_NoParameters_ReturnsHttpClient()
    {
        var systemUnderTest = GetSystemUnderTest();

        var result = systemUnderTest.CreateClient();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<HttpClient>());
    }
    
    [Test]
    public void CreateClient_WithHandler_ReturnsHttpClient()
    {
        var systemUnderTest = GetSystemUnderTest();
        var handler = new HttpClientHandler();

        var result = systemUnderTest.CreateClient(handler);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<HttpClient>());
    }
    
    private HttpClientFactory GetSystemUnderTest()
    {
        return new HttpClientFactory();
    }
}