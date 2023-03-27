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
    public void ApiAccess_DefaultConstructor_AllParametersSet()
    {
        //Arrange
        _mapper = Substitute.For<IMapper>();
        _userWebApiServices = Substitute.For<IUserWebApiServices>();

        // Act
        var apiAccess = new BackendAccess(_mapper, _userWebApiServices);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(apiAccess.Mapper, Is.EqualTo(_mapper));
            Assert.That(apiAccess.UserWebApiServices, Is.EqualTo(_userWebApiServices));
        });
    }

    [Test]
    public void ApiAccess_GetUserTokenAsync_CallsMethod()
    {
        // Arrange
        _mapper = Substitute.For<IMapper>();
        _userWebApiServices = Substitute.For<IUserWebApiServices>();
        var apiAccess = new BackendAccess(_mapper, _userWebApiServices);

        // Act
        var userToken = apiAccess.GetUserTokenAsync("username", "password");

        // Assert
        _userWebApiServices.Received().GetUserTokenAsync("username", "password");
    }
}