using AutoMapper;
using BackendAccess.BackendServices;
using BusinessLogic.Entities.BackendAccess;
using BusinessLogic.ErrorManagement.BackendAccess;
using NSubstitute;
using NUnit.Framework;

namespace BackendAccessTest.API;

[TestFixture]
public class ApiAccessUt
{
    [SetUp]
    public void Setup()
    {
        //Arrange
        _mapper = Substitute.For<IMapper>();
        _userWebApiServices = Substitute.For<IUserWebApiServices>();
        _userWebApiServices.GetApiHealthcheck().Returns(true);
    }

    private IMapper _mapper = null!;
    private IUserWebApiServices _userWebApiServices = null!;

    [Test]
    public void BackendAccess_DefaultConstructor_AllParametersSet()
    {
        // Act
        var systemUnderTest = new BackendAccess.API.BackendAccess(_mapper, _userWebApiServices);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Mapper, Is.EqualTo(_mapper));
            Assert.That(systemUnderTest.UserWebApiServices, Is.EqualTo(_userWebApiServices));
        });
    }

    [Test]
    // ANF-ID: [AHO21]
    public async Task BackendAccess_GetUserTokenAsync_CallsMethod()
    {
        var systemUnderTest = new BackendAccess.API.BackendAccess(_mapper, _userWebApiServices);

        // Act
        await systemUnderTest.GetUserTokenAsync("username", "password");

        // Assert
        await _userWebApiServices.Received().GetUserTokenAsync("username", "password");
    }

    [Test]
    // ANF-ID: [AHO21]
    public void BackendAccess_GetUserTokenAsync__ReturnsFalse_ThrowsException()
    {
        _userWebApiServices.GetApiHealthcheck().Returns(false);
        var systemUnderTest = new BackendAccess.API.BackendAccess(_mapper, _userWebApiServices);

        // Act & Assert
        var ex = Assert.ThrowsAsync<BackendApiUnreachableException>(async () =>
            await systemUnderTest.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message, Is.EqualTo("API healthcheck failed, not reachable"));
    }

    [Test]
    // ANF-ID: [AHO21]
    public async Task BackendAccess_GetUserInformationAsync_CallsMethod()
    {
        var token = new UserToken("testToken");
        var systemUnderTest = new BackendAccess.API.BackendAccess(_mapper, _userWebApiServices);

        // Act
        await systemUnderTest.GetUserInformationAsync(token);

        // Assert
        await _userWebApiServices.Received().GetUserInformationAsync(token.Token);
    }

    [Test]
    // ANF-ID: [AHO21]
    public void BackendAccess_GetUserInformationAsync_ReturnsFalse_ThrowsException()
    {
        var token = new UserToken("testToken");
        _userWebApiServices.GetApiHealthcheck().Returns(false);
        var systemUnderTest = new BackendAccess.API.BackendAccess(_mapper, _userWebApiServices);

        // Act & Assert
        var ex = Assert.ThrowsAsync<BackendApiUnreachableException>(async () =>
            await systemUnderTest.GetUserInformationAsync(token));
        Assert.That(ex!.Message, Is.EqualTo("API healthcheck failed, not reachable"));
    }

    [Test]
    // ANF-ID: [AHO22]
    public async Task BackendAccess_UploadWorldAsync_CallsMethod()
    {
        var token = new UserToken("testToken");
        var systemUnderTest = new BackendAccess.API.BackendAccess(_mapper, _userWebApiServices);
        var mockProgress = Substitute.For<IProgress<int>>();

        // Act
        await systemUnderTest.UploadLearningWorldAsync(token, "testWorldName", "testWorldDescription",
            progress: mockProgress);

        // Assert
        await _userWebApiServices.Received()
            .UploadLearningWorldAsync(token.Token, "testWorldName", "testWorldDescription", mockProgress);
    }

    [Test]
    // ANF-ID: [AHO23, AHO24]
    public async Task GetLmsWorldList_CallsUserWebApiServices()
    {
        var token = new UserToken("testToken");
        var systemUnderTest = new BackendAccess.API.BackendAccess(_mapper, _userWebApiServices);

        await systemUnderTest.GetLmsWorldList(token, 1);

        await _userWebApiServices.Received().GetLmsWorldList(token.Token, 1);
    }

    [Test]
    // ANF-ID: [AHO24]
    public async Task DeleteLmsWorld_CallsUserWebApiServices()
    {
        var token = new UserToken("testToken");
        var systemUnderTest = new BackendAccess.API.BackendAccess(_mapper, _userWebApiServices);
        var world = new LmsWorld();

        await systemUnderTest.DeleteLmsWorld(token, world);

        await _userWebApiServices.Received().DeleteLmsWorld(token.Token, world.WorldId);
    }
}