using AuthoringTool;
using AutoMapper;
using NUnit.Framework;

namespace AuthoringToolTest;

[TestFixture]
public class MappingProfileUt
{
    [Test]
    public void Constructor_TestConfigurationIsValid()
    {
        var systemUnderTest = new MappingProfile();
        var mapper = new MapperConfiguration(cfg =>
            cfg.AddProfile(systemUnderTest));

        Assert.That(() => mapper.AssertConfigurationIsValid(), Throws.Nothing);
    }
}