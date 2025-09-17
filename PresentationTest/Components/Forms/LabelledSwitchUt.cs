using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.LabelledSwitch;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Forms;

[TestFixture]
public class LabelledSwitchUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _testContext.Services.AddSingleton(Substitute.For<IKeyInterceptorService>());
        _testContext.Services.AddMudServices();
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    }

    private TestContext _testContext;
    private LabelledSwitchState _boundValue;

    [Test]
    public void Constructor_InjectsDependencies([Values] LabelledSwitchState initialValue)
    {
        _boundValue = initialValue;

        var systemUnderTest = GetRenderedComponent();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.State, Is.EqualTo(initialValue));
            Assert.That(systemUnderTest.Instance.LeftLabel, Is.EqualTo("left"));
            Assert.That(systemUnderTest.Instance.RightLabel, Is.EqualTo("right"));
        });
    }

    [Test]
    public void Select_ChangesValue()
    {
        _boundValue = LabelledSwitchState.Left;

        var systemUnderTest = GetRenderedComponent();

        var mudSwitch = systemUnderTest.FindComponent<MudSwitch<bool>>();
        mudSwitch.InvokeAsync(() => mudSwitch.Instance.ValueChanged.InvokeAsync(true));
        mudSwitch.Render();

        Assert.That(_boundValue, Is.EqualTo(LabelledSwitchState.Right));
        Assert.That(systemUnderTest.Instance.State, Is.EqualTo(LabelledSwitchState.Right));

        mudSwitch.InvokeAsync(() => mudSwitch.Instance.ValueChanged.InvokeAsync(false));
        mudSwitch.Render();

        Assert.That(_boundValue, Is.EqualTo(LabelledSwitchState.Left));
        Assert.That(systemUnderTest.Instance.State, Is.EqualTo(LabelledSwitchState.Left));
    }

    private IRenderedComponent<LabelledSwitch> GetRenderedComponent(
        string leftLabel = "left", string rightLabel = "right")
    {
        return _testContext.RenderComponent<LabelledSwitch>(builder =>
        {
            builder.Add(p => p.LeftLabel, leftLabel);
            builder.Add(p => p.RightLabel, rightLabel);
            builder.Add(p => p.State, _boundValue);
            builder.Add(p => p.StateChanged, v => { _boundValue = v; });
        });
    }
}