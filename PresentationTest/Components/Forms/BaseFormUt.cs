using System;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Forms;

[TestFixture]
public class BaseFormUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _validator = Substitute.For<IValidationWrapper<TestEntity>>();
        _snackbar = Substitute.For<ISnackbar>();
        _testContext.Services.AddSingleton(_validator);
        _testContext.Services.AddSingleton(_snackbar);
        _testContext.ComponentFactories.AddStub<MudCardActions>();
        _testContext.ComponentFactories.AddStub<MudButtonGroup>();
        //cannot stub mudform because we @ref it in BaseForm
        //_testContext.ComponentFactories.AddStub<MudForm>();
        _testContext.ComponentFactories.AddStub<MudAlert>();
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
        _snackbar.Dispose();
    }

    private TestContext _testContext;
    private IValidationWrapper<TestEntity> _validator;
    private ISnackbar _snackbar;

    [Test]
    public void Constructor_SetsParameters()
    {
        _testContext.ComponentFactories.AddStub<MudCardContent>();
        var onValidSubmit = EventCallback.Factory.Create<TestForm>(this, () => { });
        var snackbarMessage = "SnackbarMessage";
        var formDataContainer = Substitute.For<IFormDataContainer<TestForm, TestEntity>>();
        var fields = new RenderFragment(builder => builder.AddMarkupContent(0, "Fields"));
        var headerButtons = new RenderFragment(builder => builder.AddMarkupContent(0, "HeaderButtons"));
        var footerButtons = new RenderFragment(builder => builder.AddMarkupContent(0, "FooterButtons"));

        var systemUnderTest = GetRenderedComponent(onValidSubmit, snackbarMessage, formDataContainer, fields,
            headerButtons, footerButtons);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.OnValidSubmit, Is.EqualTo(onValidSubmit));
            Assert.That(systemUnderTest.Instance.SnackbarMessage, Is.EqualTo(snackbarMessage));
            Assert.That(systemUnderTest.Instance.FormDataContainer, Is.EqualTo(formDataContainer));
            Assert.That(systemUnderTest.Instance.Fields, Is.EqualTo(fields));
            Assert.That(systemUnderTest.Instance.HeaderButtons, Is.EqualTo(headerButtons));
            Assert.That(systemUnderTest.Instance.FooterButtons, Is.EqualTo(footerButtons));
        });
    }

    [Test]
    public void Render_RendersFieldsAndHeaderAndFooterButtons()
    {
        _testContext.ComponentFactories.AddStub<MudCardContent>();
        var fields = new RenderFragment(builder => builder.AddMarkupContent(0, "Fields"));
        var headerButtons = new RenderFragment(builder => builder.AddMarkupContent(0, "HeaderButtons"));
        var footerButtons = new RenderFragment(builder => builder.AddMarkupContent(0, "FooterButtons"));

        var systemUnderTest = GetRenderedComponent(fields: fields, headerButtons: headerButtons,
            footerButtons: footerButtons);

        var mudCardActions = systemUnderTest.FindComponent<Stub<MudCardActions>>();
        var mudCardActionsChild =
            _testContext.Render((RenderFragment)mudCardActions.Instance.Parameters["ChildContent"]);
        var mudButtonGroup = mudCardActionsChild.FindComponent<Stub<MudButtonGroup>>();
        var mudButtonGroupChild =
            _testContext.Render((RenderFragment)mudButtonGroup.Instance.Parameters["ChildContent"]);
        mudButtonGroupChild.MarkupMatches("HeaderButtons");

        var mudCardContent = systemUnderTest.FindComponent<Stub<MudCardContent>>();
        var mudCardContentChild =
            _testContext.Render((RenderFragment)mudCardContent.Instance.Parameters["ChildContent"]);
        mudCardContentChild.MarkupMatches("Fields");

        mudCardActions = systemUnderTest.FindComponents<Stub<MudCardActions>>()[1];
        mudCardActionsChild =
            _testContext.Render((RenderFragment)mudCardActions.Instance.Parameters["ChildContent"]);
        mudButtonGroup = mudCardActionsChild.FindComponent<Stub<MudButtonGroup>>();
        mudButtonGroupChild =
            _testContext.Render((RenderFragment)mudButtonGroup.Instance.Parameters["ChildContent"]);
        mudButtonGroupChild.MarkupMatches("FooterButtons");
    }

    [Test]
    public async Task SubmitAsync_WhenValid_CallsOnValidSubmit_AndCallsSnackbarWithSnackbarMessage()
    {
        var formModel = new TestForm();
        var onValidSubmitCallCounter = 0;
        var snackbarMessage = "MySnackbarMessage";
        var onValidSubmit = EventCallback.Factory.Create<TestForm>(this, receivedFormModel =>
        {
            Assert.That(receivedFormModel, Is.EqualTo(formModel));
            onValidSubmitCallCounter++;
        });
        var formModelContainer = Substitute.For<IFormDataContainer<TestForm, TestEntity>>();
        formModelContainer.FormModel.Returns(formModel);

        var systemUnderTest = GetRenderedComponent(onValidSubmit, formDataContainer: formModelContainer,
            snackbarMessage: snackbarMessage);

        await systemUnderTest.Instance.SubmitAsync();

        Assert.That(onValidSubmitCallCounter, Is.EqualTo(1));
        _snackbar.Received(1).Add(Arg.Is(snackbarMessage));
    }

    [Test]
    public async Task SubmitAsync_ThrowsException_ShowsMessageAsMudAlert()
    {
        var formModel = new TestForm();
        var onValidSubmit =
            EventCallback.Factory.Create<TestForm>(this, _ => throw new Exception("The message"));
        var formModelContainer = Substitute.For<IFormDataContainer<TestForm, TestEntity>>();
        formModelContainer.FormModel.Returns(formModel);

        var systemUnderTest = GetRenderedComponent(onValidSubmit, formDataContainer: formModelContainer);

        Assert.That(() => systemUnderTest.Find("div.form-error-message"), Throws.TypeOf<ElementNotFoundException>());

        await systemUnderTest.Instance.SubmitAsync();
        systemUnderTest.Render();

        Assert.That(() => systemUnderTest.Find("div.form-error-message"), Throws.Nothing);
        var mudAlert = systemUnderTest.FindComponent<Stub<MudAlert>>();
        Assert.That(mudAlert.Instance.Parameters["ChildContent"], Is.Not.Null);
        Assert.That(systemUnderTest.Instance.SubmitErrorMessage,
            Is.EqualTo("An error has occured trying to submit the form: The message"));
    }


    private IRenderedComponent<BaseForm<TestForm, TestEntity>> GetRenderedComponent(
        EventCallback<TestForm>? onValidSubmit = null, string? snackbarMessage = null,
        IFormDataContainer<TestForm, TestEntity>? formDataContainer = null, RenderFragment? fields = null,
        RenderFragment? headerButtons = null, RenderFragment? footerButtons = null)
    {
        onValidSubmit ??= EventCallback.Factory.Create<TestForm>(this, () => { });
        snackbarMessage ??= "SnackbarMessage";
        formDataContainer ??= Substitute.For<IFormDataContainer<TestForm, TestEntity>>();
        fields ??= builder => builder.AddMarkupContent(0, "Fields");
        headerButtons ??= builder => builder.AddMarkupContent(0, "HeaderButtons");
        footerButtons ??= builder => builder.AddMarkupContent(0, "FooterButtons");
        return _testContext.RenderComponent<BaseForm<TestForm, TestEntity>>(
            (nameof(BaseForm<TestForm, TestEntity>.OnValidSubmit), onValidSubmit),
            (nameof(BaseForm<TestForm, TestEntity>.SnackbarMessage), snackbarMessage),
            (nameof(BaseForm<TestForm, TestEntity>.FormDataContainer), formDataContainer),
            (nameof(BaseForm<TestForm, TestEntity>.Fields), fields),
            (nameof(BaseForm<TestForm, TestEntity>.HeaderButtons), headerButtons),
            (nameof(BaseForm<TestForm, TestEntity>.FooterButtons), footerButtons)
        );
    }

    public class TestForm
    {
        public string Foo { get; set; }
    }

    public class TestEntity
    {
        public string Foo { get; set; }
    }
}