using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
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
    private bool _boundValue;

    [Test]
    public void Constructor_InjectsDependencies([Values] bool initialValue)
    {
        _boundValue = initialValue;

        var systemUnderTest = GetRenderedComponent();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.IsRightSelected, Is.EqualTo(initialValue));
            Assert.That(systemUnderTest.Instance.IsLeftSelected, Is.EqualTo(!initialValue));
            Assert.That(systemUnderTest.Instance.LeftLabel, Is.EqualTo("left"));
            Assert.That(systemUnderTest.Instance.RightLabel, Is.EqualTo("right"));
        });
    }

    [Test]
    public void Select_ChangesValue()
    {
        _boundValue = false;

        var systemUnderTest = GetRenderedComponent();

        var mudSwitch = systemUnderTest.FindComponent<MudSwitch<bool>>();
        mudSwitch.InvokeAsync(() => mudSwitch.Instance.ValueChanged.InvokeAsync(true));
        mudSwitch.Render();

        Assert.That(_boundValue, Is.True);
        Assert.That(systemUnderTest.Instance.IsRightSelected, Is.True);
        Assert.That(systemUnderTest.Instance.IsLeftSelected, Is.False);

        mudSwitch.InvokeAsync(() => mudSwitch.Instance.ValueChanged.InvokeAsync(false));
        mudSwitch.Render();

        Assert.That(_boundValue, Is.False);
        Assert.That(systemUnderTest.Instance.IsRightSelected, Is.False);
        Assert.That(systemUnderTest.Instance.IsLeftSelected, Is.True);
    }

    private IRenderedComponent<LabelledSwitch> GetRenderedComponent(
        string leftLabel = "left", string rightLabel = "right")
    {
        return _testContext.RenderComponent<LabelledSwitch>(builder =>
        {
            builder.Add(p => p.LeftLabel, leftLabel);
            builder.Add(p => p.RightLabel, rightLabel);
            builder.Add(p => p.IsRightSelected, _boundValue);
            builder.Add<bool>(p => p.IsRightSelectedChanged, v => { _boundValue = v; });
        });
    }
}