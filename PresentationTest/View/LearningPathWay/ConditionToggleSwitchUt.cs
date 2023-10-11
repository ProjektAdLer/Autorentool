using Bunit;
using Microsoft.AspNetCore.Components;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.View.LearningPathWay;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningPathWay;

[TestFixture]
public class ConditionToggleSwitchUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
    }

    [TearDown]
    public void Teardown()
    {
        _testContext.Dispose();
    }

    private TestContext _testContext = null!;

    private string _baseMarkup = @"
<foreignObject transform=""translate(9,7)"" x={0} y={1} width=""56"" height=""28"">
    <label class=""relative inline-flex items-center cursor-pointer"">
        <input type=""checkbox"" checked=""{2}"" class=""sr-only peer"">
        <div class=""w-14 h-7 bg-[#172D4C] rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-0.5 after:left-[4px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-6 after:w-6 after:transition-all dark:border-gray-600 peer-checked:bg-[#172D4C]""></div>
    </label>
</foreignobject>


{3}
";

    [Test]
    public void Render_ConditionAnd_RendersCorrectly()
    {
        var objectInPathwayViewModel = new PathWayConditionViewModel(ConditionEnum.And, false, 1337, 420);

        var systemUnderTest = GetRenderedComponent(objectInPathwayViewModel);

        var correctMarkup = string.Format(_baseMarkup, 268, 420, "true",
            @"<text x=268 y=420 font-size=""12"" transform=""translate(12,26)"" pointer-events=""none"" fill=""white"" style=""user-select:none;"">AND</text>");

        systemUnderTest.MarkupMatches(correctMarkup);
    }

    [Test]
    public void Render_ConditionOr_RendersCorrectly()
    {
        var objectInPathwayViewModel = new PathWayConditionViewModel(ConditionEnum.Or, false, 1337, 420);

        var systemUnderTest = GetRenderedComponent(objectInPathwayViewModel);

        var correctMarkup = string.Format(_baseMarkup, 268, 420, "false",
            @"<text x=268 y=420 font-size=""12"" transform=""translate(43,26)"" pointer-events=""none"" fill=""white"" style=""user-select:none;"">OR</text>");

        systemUnderTest.MarkupMatches(correctMarkup);
    }

    [Test]
    public void Input_Click_CallsOnSwitchPathwayCondition_WithObjectInPathway()
    {
        var objectInPathwayViewModel = new PathWayConditionViewModel(ConditionEnum.Or, false, 1337, 420);
        var callbackCalled = false;
        var callback = EventCallback.Factory.Create<PathWayConditionViewModel>(this,
            pwvm =>
            {
                Assert.That(pwvm, Is.EqualTo(objectInPathwayViewModel));
                callbackCalled = true;
            });

        var systemUnderTest = GetRenderedComponent(objectInPathwayViewModel, callback);

        systemUnderTest.Find("input").Click();
        Assert.That(callbackCalled);
    }

    private IRenderedComponent<ConditionToggleSwitch> GetRenderedComponent(
        IObjectInPathWayViewModel? objectInPathWayViewModel = null,
        EventCallback<PathWayConditionViewModel>? callback = null)
    {
        objectInPathWayViewModel ??= Substitute.For<IObjectInPathWayViewModel>();
        callback ??= EventCallback.Factory.Create<PathWayConditionViewModel>(this, _ => { });

        return _testContext.RenderComponent<ConditionToggleSwitch>(b =>
        {
            b.Add(c => c.ObjectInPathWay, objectInPathWayViewModel);
            b.Add(c => c.OnSwitchPathWayCondition, callback.Value);
        });
    }
}