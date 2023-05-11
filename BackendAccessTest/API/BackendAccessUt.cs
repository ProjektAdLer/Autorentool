using ApiAccess.API;
using ApiAccess.WebApi;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;

namespace BackendAccessTest.API;

[TestFixture]
public class ApiAccessUt
{
    private IMapper _mapper = null!;
    private IUserWebApiServices _userWebApiServices = null!;

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
    public async Task BackendAccess_GetUserTokenAsync_CallsMethod()
    {
        // Arrange
        _mapper = Substitute.For<IMapper>();
        _userWebApiServices = Substitute.For<IUserWebApiServices>();
        var systemUnderTest = new BackendAccess(_mapper, _userWebApiServices);

        // Act
        await systemUnderTest.GetUserTokenAsync("username", "password");

        // Assert
        await _userWebApiServices.Received().GetUserTokenAsync("username", "password");
    }

    [Test]
    public async Task BackendAccess_GetUserInformationAsync_CallsMethod()
    {
        // Arrange
        _mapper = Substitute.For<IMapper>();
        _userWebApiServices = Substitute.For<IUserWebApiServices>();
        var systemUnderTest = new BackendAccess(_mapper, _userWebApiServices);

        // Act
        await systemUnderTest.GetUserInformationAsync("token");

        // Assert
        await _userWebApiServices.Received().GetUserInformationAsync("token");
    }

    [Test]
    public async Task BackendAccess_UploadWorldAsync_CallsMethod()
    {
        // Arrange
        _mapper = Substitute.For<IMapper>();
        _userWebApiServices = Substitute.For<IUserWebApiServices>();
        var systemUnderTest = new BackendAccess(_mapper, _userWebApiServices);

        // Act
        await systemUnderTest.UploadLearningWorldAsync("testToken", "testWorldName", "testWorldDescription");

        // Assert
        await _userWebApiServices.Received().UploadLearningWorldAsync("testToken", "testWorldName", "testWorldDescription");
    }
}