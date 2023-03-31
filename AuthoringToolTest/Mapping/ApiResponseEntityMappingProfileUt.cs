using ApiAccess.BackendEntities;
using AuthoringTool.Mapping;
using AutoMapper;
using BusinessLogic.Entities.ApiElements;
using NUnit.Framework;

namespace AuthoringToolTest.Mapping;

public class ApiResponseEntityMappingProfileUt
{
    [Test]
    public void TestMapping()
    {
        var config = new MapperConfiguration(cfg => { ApiResponseEntityMappingProfile.Configure(cfg); });
        var mapper = config.CreateMapper();

        var source = new UserInformationBE
        {
            LmsUserName = "LmsUserName",
            IsAdmin = true,
            UserId = 1,
            UserEmail = "UserEmail"
        };

        var dest = mapper.Map<UserInformation>(source);

        Assert.AreEqual(source.LmsUserName, dest.LmsUsername);
        Assert.AreEqual(source.IsAdmin, dest.IsLmsAdmin);
        Assert.AreEqual(source.UserId, dest.LmsId);
        Assert.AreEqual(source.UserEmail, dest.LmsEmail);
    }
}