using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
// ReSharper disable MemberCanBePrivate.Global

namespace PresentationTest.Components.Forms;

[TestFixture]
public class FormDataContainerUt
{
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mapper = Substitute.For<IMapper>();
    }

    [Test]
    public void Constructor_SetsProperties()
    {
        var systemUnderTest = GetSystemUnderTest<TestForm, TestEntity>(_mapper);

        Assert.That(systemUnderTest.FormModel, Is.Not.Null);
        Assert.That(systemUnderTest.FormModel, Is.TypeOf<TestForm>());
    }

    [Test]
    public void GetMappedEntity_CallsMapperWithTEntity()
    {
        var testEntity = new TestEntity();
        _mapper.Map<TestForm, TestEntity>(Arg.Any<TestForm>()).Returns(testEntity);

        var systemUnderTest = GetSystemUnderTest<TestForm, TestEntity>(_mapper);

        var result = systemUnderTest.GetMappedEntity();

        Assert.That(result, Is.EqualTo(testEntity));
        _mapper.Received(1).Map<TestForm, TestEntity>(Arg.Any<TestForm>());
    }

    private FormDataContainer<TForm, TEntity> GetSystemUnderTest<TForm, TEntity>(IMapper? mapper = null)
        where TForm : new()
    {
        mapper ??= Substitute.For<IMapper>();
        return new FormDataContainer<TForm, TEntity>(mapper);
    }

    public class TestForm
    {
    }

    public class TestEntity
    {
    }
}