﻿using AutoMapper;
using BackendAccess.BackendServices;
using BusinessLogic.Entities.BackendAccess;
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
    public async Task BackendAccess_GetUserTokenAsync_CallsMethod()
    {
        var systemUnderTest = new BackendAccess.API.BackendAccess(_mapper, _userWebApiServices);

        // Act
        await systemUnderTest.GetUserTokenAsync("username", "password");

        // Assert
        await _userWebApiServices.Received().GetUserTokenAsync("username", "password");
    }

    [Test]
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
    public async Task BackendAccess_UploadWorldAsync_CallsMethod()
    {
        var token = new UserToken("testToken");
        var systemUnderTest = new BackendAccess.API.BackendAccess(_mapper, _userWebApiServices);
        var mockProgress = Substitute.For<IProgress<int>>();

        // Act
        await systemUnderTest.UploadLearningWorldAsync(token, "testWorldName", "testWorldDescription", mockProgress);

        // Assert
        await _userWebApiServices.Received()
            .UploadLearningWorldAsync(token.Token, "testWorldName", "testWorldDescription", mockProgress);
    }
}