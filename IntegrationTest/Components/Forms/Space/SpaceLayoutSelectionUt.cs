using System;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.Space;
using Presentation.PresentationLogic.LearningSpace;
using PresentationTest;
using Shared;
using TestHelpers;

namespace IntegrationTest.Components.Forms.Space;

[TestFixture]
public class SpaceLayoutSelectionUt : MudBlazorTestFixture<SpaceLayoutSelection>
{
    private ILearningSpacePresenter Presenter { get; set; }

    [SetUp]
    public new void Setup()
    {
        Context.AddLocalizerForTest<FloorPlanEnum>();
        Presenter = Substitute.For<ILearningSpacePresenter>();
        Context.Services.AddSingleton(Presenter);
    }

    [Test]
    public void Render_ShowsAllFloorPlanValues_AndShowsSelectedFloorPlan()
    {
        var floorPlanValues = Enum.GetValues<FloorPlanEnum>();
        var lsvm = ViewModelProvider.GetLearningSpace(floorPlan: floorPlanValues[0]);
        Presenter.LearningSpaceVm.Returns(lsvm);

        var sut = GetRenderedComponent(lsvm);


        var mudIcons = sut.FindComponentsOrFail<MudIcon>().ToList();
        Assert.That(mudIcons, Has.Count.EqualTo(floorPlanValues.Length));
        Assert.Multiple(() =>
        {
            Assert.That(mudIcons.First().Instance.Style!, Does.Contain("filter: grayscale(0%)"));
            Assert.That(mudIcons.Skip(1).Select(i => i.Instance.Style!),
                Has.All.Contains("filter: grayscale(70%)"));
        });
    }

    [Test]
    // ANF-ID: [AWA0023]
    public async Task ClickFloorPlan_SetsFloorPlanInSpace()
    {
        var floorPlanValues = Enum.GetValues<FloorPlanEnum>();
        var lsvm = ViewModelProvider.GetLearningSpace(floorPlan: floorPlanValues[0]);
        Presenter.LearningSpaceVm.Returns(lsvm);
        
        var sut = GetRenderedComponent(lsvm);

        var listItems = sut.FindComponentsOrFail<MudListItem>();
        await listItems.ElementAt(1).Find("div").ClickAsync(new MouseEventArgs());
        
        Presenter.Received().SetLearningSpaceLayout(floorPlanValues[1]);
    }

    private IRenderedComponent<SpaceLayoutSelection> GetRenderedComponent(ILearningSpaceViewModel? lsvm = null)
    {
        // ReSharper disable once InvertIf
        if (lsvm == null)
        {
            lsvm = ViewModelProvider.GetLearningSpace();
            Presenter.LearningSpaceVm.Returns(lsvm);
        }

        return Context.RenderComponent<SpaceLayoutSelection>(pBuilder =>
            pBuilder.Add(p => p.LearningSpaceViewModel, lsvm));
    }
}