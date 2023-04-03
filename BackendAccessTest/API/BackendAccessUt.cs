using ApiAccess.API;
using ApiAccess.WebApi;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;

namespace BackendAccessTest.API;

[TestFixture]
public class ApiAccessUt
{
    private IMapper _mapper;
    private IUserWebApiServices _userWebApiServices;

    [Test]
    public void BackendAccess_DefaultConstructor_AllParametersSet()
    {
        //Arrange
        _mapper = Substitute.For<IMapper>();
        _userWebApiServices = Substitute.For<IUserWebApiServices>();

        // Act
        var systemUnderTest = new BackendAccess(_mapper, _userWebApiServices);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Mapper, Is.EqualTo(_mapper));
            Assert.That(systemUnderTest.UserWebApiServices, Is.EqualTo(_userWebApiServices));
        });
    }

    [Test]
    public void BackendAccess_GetUserTokenAsync_CallsMethod()
    {
        // Arrange
        _mapper = Substitute.For<IMapper>();
        _userWebApiServices = Substitute.For<IUserWebApiServices>();
        var systemUnderTest = new BackendAccess(_mapper, _userWebApiServices);

        // Act
        var userToken = systemUnderTest.GetUserTokenAsync("username", "password");

        // Assert
        _userWebApiServices.Received().GetUserTokenAsync("username", "password");
    }

    [Test]
    public void BackendAccess_GetUserInformationAsync_CallsMethod()
    {
        // Arrange
        _mapper = Substitute.For<IMapper>();
        _userWebApiServices = Substitute.For<IUserWebApiServices>();
        var systemUnderTest = new BackendAccess(_mapper, _userWebApiServices);

        // Act
        var userToken = systemUnderTest.GetUserInformationAsync("token");

        // Assert
        _userWebApiServices.Received().GetUserInformationAsync("token");
    }

    [Test]
    public async Task BackendAccess_UploadWorldAsync_CallsMethod()
    {
        // Arrange
        _mapper = Substitute.For<IMapper>();
        _userWebApiServices = Substitute.For<IUserWebApiServices>();
        var systemUnderTest = new BackendAccess(_mapper, _userWebApiServices);

        // Act
        var response =
            await systemUnderTest.UploadLearningWorldAsync("testToken", "testWorldName", "testWorldDescription");

        // Assert
        _userWebApiServices.Received().UploadLearningWorldAsync("testToken", "testWorldName", "testWorldDescription");
    }
}