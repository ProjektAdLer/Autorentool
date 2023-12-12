using System;
using System.IO.Abstractions;
using Bunit;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.View.MyLearningWorlds;
using PresentationTest;
using Shared;

namespace IntegrationTest.View.MyLearningWorlds;

[TestFixture]
public class LearningWorldCardIt : MudBlazorTestFixture<LearningWorldCard>
{
    [Test]
    public void Render_InjectsDependenciesAndParameters()
    {
        var world = Substitute.For<ILearningWorldViewModel>();
        var onOpenLearningWorld = EventCallback.Factory.Create<ILearningWorldViewModel>(this, () => { });
        var onCloseWorld = EventCallback.Factory.Create<ILearningWorldViewModel>(this, () => { });
        var fileInfo = Substitute.For<IFileInfo>();

        var systemUnderTest = GetRenderedComponent(world, onOpenLearningWorld, onCloseWorld, fileInfo);

        Assert.That(systemUnderTest.Instance.Localizer, Is.Not.Null);
        Assert.That(systemUnderTest.Instance.LearningWorld, Is.EqualTo(world));
        Assert.That(systemUnderTest.Instance.OnOpenLearningWorld, Is.EqualTo(onOpenLearningWorld));
        Assert.That(systemUnderTest.Instance.OnCloseLearningWorld, Is.EqualTo(onCloseWorld));
        Assert.That(systemUnderTest.Instance.FileInfo, Is.EqualTo(fileInfo));
    }

    [Test]
    public void Render_ShowFileNameTrue_ShowsFileNameFromFileInfo()
    {
        var world = Substitute.For<ILearningWorldViewModel>();
        world.SavePath = "/somepath/foo.bar";
        
        var systemUnderTest = GetRenderedComponent(world, showFileName: true);
        
        Assert.That(systemUnderTest.Find("div.file-name-display").TextContent, Is.EqualTo("foo.bar"));
    }

    [Test]
    public void Render_ShowFileNameTrue_NotYetSaved_ShowsSpecialMessage()
    {
        var world = Substitute.For<ILearningWorldViewModel>();
        
        var systemUnderTest = GetRenderedComponent(world, showFileName: true);
        
        Assert.That(systemUnderTest.Find("div.file-name-display").TextContent, Is.EqualTo("LearningWorldCard.PathDisplayMessage.NotYetSaved"));
    }

    [Test]
    public void CloseButtonPressed_CallsOnCloseCallback()
    {
        var callbackCallCount = 0;
        var world = Substitute.For<ILearningWorldViewModel>();
        world.SavePath = "/foo/bar";
        var onCloseWorld = EventCallback.Factory.Create<ILearningWorldViewModel>(this, () => { callbackCallCount++; });

        var systemUnderTest = GetRenderedComponent(onCloseWorld: onCloseWorld, learningWorld: world);

        systemUnderTest.FindComponentWithMarkup<MudFab>("close-button").Find("button").Click();

        Assert.That(callbackCallCount, Is.EqualTo(1));
    }

    [Test]
    public void OpenButtonPressed_CallsOnOpenCallback()
    {
        var callbackCallCount = 0;
        var world = Substitute.For<ILearningWorldViewModel>();
        var onOpenLearningWorld = EventCallback.Factory.Create<ILearningWorldViewModel>(this,
            () => { callbackCallCount++; });

        var systemUnderTest = GetRenderedComponent(onOpenLearningWorld: onOpenLearningWorld, learningWorld: world);

        systemUnderTest.Find("button.open-button").Click();

        Assert.That(callbackCallCount, Is.EqualTo(1));
    }

    private IRenderedComponent<LearningWorldCard> GetRenderedComponent(
        ILearningWorldViewModel? learningWorld = null,
        EventCallback<ILearningWorldViewModel>? onOpenLearningWorld = null,
        EventCallback<ILearningWorldViewModel>? onCloseWorld = null,
        IFileInfo? fileInfo = null,
        bool showFileName = true
    )
    {
        onOpenLearningWorld ??= EventCallback.Factory.Create<ILearningWorldViewModel>(this, () => { });
        onCloseWorld ??= EventCallback.Factory.Create<ILearningWorldViewModel>(this, () => { });
        return Context.RenderComponent<LearningWorldCard>(p =>
        {
            p.Add(c => c.LearningWorld, learningWorld);
            p.Add(c => c.OnOpenLearningWorld, onOpenLearningWorld.Value);
            p.Add(c => c.OnCloseLearningWorld, onCloseWorld.Value);
            p.Add(c => c.FileInfo, fileInfo);
            p.Add(c => c.ShowFileName, showFileName);
        });
    }
}