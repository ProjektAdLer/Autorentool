using AuthoringTool.Mapping;
using AutoMapper;
using BackendAccess.BackendEntities;
using BusinessLogic.Entities.BackendAccess;
using NUnit.Framework;

namespace AuthoringToolTest.Mapping;

public class ApiResponseEntityMappingProfileUt
{
    [Test]
    public void Map_UserInformationBE_UserInformation()
    {
        var systemUnderTest = CreateTestableMapper();

        var source = new UserInformationBE
        {
            LmsUserName = "LmsUserName",
            IsAdmin = true,
            UserId = 1,
            UserEmail = "UserEmail"
        };

        var dest = systemUnderTest.Map<UserInformation>(source);

        Assert.Multiple(() =>
        {
            Assert.That(dest.LmsUsername, Is.EqualTo(source.LmsUserName));
            Assert.That(dest.IsLmsAdmin, Is.EqualTo(source.IsAdmin));
            Assert.That(dest.LmsId, Is.EqualTo(source.UserId));
            Assert.That(dest.LmsEmail, Is.EqualTo(source.UserEmail));
        });
    }

    // Test for UserTokenBE to UserToken
    [Test]
    public void Map_UserTokenBE_UserToken()
    {
        var systemUnderTest = CreateTestableMapper();

        var source = new UserTokenBE
        {
            LmsToken = "testToken"
        };

        var dest = systemUnderTest.Map<UserToken>(source);

        Assert.That(dest.Token, Is.EqualTo(source.LmsToken));
    }

    private static IMapper CreateTestableMapper()
    {
        var config = new MapperConfiguration(cfg => { ApiResponseEntityMappingProfile.Configure(cfg); });
        return config.CreateMapper();
    }
}