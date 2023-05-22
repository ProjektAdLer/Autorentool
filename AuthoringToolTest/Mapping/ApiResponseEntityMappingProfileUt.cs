using ApiAccess.BackendEntities;
using AuthoringTool.Mapping;
using AutoMapper;
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

        Assert.AreEqual(source.LmsUserName, dest.LmsUsername);
        Assert.AreEqual(source.IsAdmin, dest.IsLmsAdmin);
        Assert.AreEqual(source.UserId, dest.LmsId);
        Assert.AreEqual(source.UserEmail, dest.LmsEmail);
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

        Assert.AreEqual(source.LmsToken, dest.Token);
    }

    private static IMapper CreateTestableMapper()
    {
        var config = new MapperConfiguration(cfg => { ApiResponseEntityMappingProfile.Configure(cfg); });
        return config.CreateMapper();
    }
}