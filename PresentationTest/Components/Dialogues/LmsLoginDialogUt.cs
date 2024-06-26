using NUnit.Framework;

namespace PresentationTest.Components.Dialogues;

[TestFixture]
public class LmsLoginDialogUt
{
    // TODO: rewrite this test suite completely
    // [SetUp]
    // public void Setup()
    // {
    //     _context = new TestContext();
    //     _context.AddMudBlazorTestServices();
    //     _presentationLogic = Substitute.For<IPresentationLogic>();
    //     _dialogService = Substitute.For<IDialogService>();
    //     _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
    //     _localizer = Substitute.For<IStringLocalizer<LmsLoginDialog>>();
    //     _errorService = Substitute.For<IErrorService>();
    //     _context.Services.AddSingleton(_presentationLogic);
    //     _context.Services.AddSingleton(_dialogService);
    //     _context.Services.AddSingleton(_applicationConfiguration);
    //     _context.Services.AddSingleton(_localizer);
    //     _context.Services.AddSingleton(_errorService);
    //     _context.Services.AddMudServices();
    //     _context.ComponentFactories.AddStub<MudDialog>();
    //     _context.ComponentFactories.AddStub<MudForm>();
    //     _context.ComponentFactories.AddStub<MudText>();
    //     _context.ComponentFactories.AddStub<MudButton>();
    //     _context.ComponentFactories.AddStub<MudList>();
    //     _context.ComponentFactories.AddStub<MudListItem>();
    //     _context.ComponentFactories.AddStub<MudTextField<string>>();
    //     _context.ComponentFactories.AddStub<MudDialog>();
    //     // DialogContent belongs to MudDialog
    //     _context.ComponentFactories.AddStub<MudText>();
    //     _context.ComponentFactories.AddStub<MudButton>();
    // }
    //
    // [TearDown]
    // public void TearDown()
    // {
    //     _context.Dispose();
    // }
    //
    // private TestContext _context;
    // private IPresentationLogic _presentationLogic;
    // private IDialogService _dialogService;
    // private IApplicationConfiguration _applicationConfiguration;
    // private IStringLocalizer<LmsLoginDialog> _localizer;
    // private IErrorService _errorService;
    //
    // [Test]
    // public void OnParametersSet_CallsPresentationLogic()
    // {
    //     using (_context)
    //     {
    //         var systemUnderTest = CreateTestableLmsLoginDialogComponent();
    //
    //         _presentationLogic.Received().IsLmsConnected();
    //     }
    // }
    //
    // [Test]
    // public void Render_IfLoggedIn_ShowLogout()
    // {
    //     using (_context)
    //     {
    //         _presentationLogic.IsLmsConnected().Returns(true);
    //         _presentationLogic.LoginName.Returns("Test");
    //         _localizer["DialogContent.LoggedIn.Message"]
    //             .Returns(new LocalizedString("DialogContent.LoggedIn.Message", "Logged in as"));
    //         _localizer["DialogContent.Button.Logout"]
    //             .Returns(new LocalizedString("DialogContent.Button.Logout", "Logout"));
    //
    //         var systemUnderTest = CreateTestableLmsLoginDialogComponent();
    //
    //         var dialogContent = systemUnderTest.FindComponent<Stub<MudDialog>>().Instance.Parameters["DialogContent"];
    //         var dialogContentRendered = _context.Render((RenderFragment)dialogContent);
    //         var dialogText = dialogContentRendered.Find("div div");
    //         var childContent = dialogContentRendered.FindComponent<Stub<MudButton>>().Instance
    //             .Parameters["ChildContent"];
    //         var mudButtonStub = _context.Render((RenderFragment)childContent);
    //
    //         Assert.That(dialogText.ToMarkup(), Contains.Substring("Logged in as"));
    //         Assert.That(dialogText.ToMarkup(), Contains.Substring("Test"));
    //
    //         Assert.That(mudButtonStub.Markup, Is.EqualTo("Logout"));
    //     }
    // }
    //
    // [Test]
    // public void Render_IfNotLoggedIn_ShowLogin()
    // {
    //     using (_context)
    //     {
    //         _presentationLogic.IsLmsConnected().Returns(false);
    //         _localizer["DialogContent.Button.Login"]
    //             .Returns(new LocalizedString("DialogContent.Button.Login", "Login"));
    //
    //         var systemUnderTest = CreateTestableLmsLoginDialogComponent();
    //
    //         var dialogContent = _context.Render((RenderFragment)systemUnderTest.FindComponent<Stub<MudDialog>>()
    //             .Instance.Parameters["DialogContent"]);
    //         var mudFormStub = _context.Render((RenderFragment)dialogContent.FindComponent<Stub<MudForm>>().Instance
    //             .Parameters["ChildContent"]);
    //         var mudButtonStub = _context.Render((RenderFragment)mudFormStub.FindComponent<Stub<MudButton>>().Instance
    //             .Parameters["ChildContent"]);
    //
    //         Assert.That(mudButtonStub.Markup, Is.EqualTo("Login"));
    //     }
    // }
    //
    // private IRenderedComponent<LmsLoginDialog> CreateTestableLmsLoginDialogComponent()
    // {
    //     return _context.RenderComponent<LmsLoginDialog>(p => p
    //         .AddCascadingValue(_context.RenderComponent<MudDialogInstance>().Instance));
    // }
}