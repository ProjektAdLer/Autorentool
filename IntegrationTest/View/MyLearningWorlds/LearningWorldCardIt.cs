using System;
using Bunit;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NUnit.Framework;
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
        var savedLearningWorldPath = new SavedLearningWorldPath
        {
            Id = new Guid("00000000-0000-0000-0000-000000000000"),
            Name = "Test",
            Path = "Test"
        };
        var onOpenLearningWorld = EventCallback.Factory.Create<SavedLearningWorldPath>(this, () => { });
        var onCloseWorld = EventCallback.Factory.Create<SavedLearningWorldPath>(this, () => { });

        var systemUnderTest = GetRenderedComponent(savedLearningWorldPath, onOpenLearningWorld, onCloseWorld);

        Assert.That(systemUnderTest.Instance.Localizer, Is.Not.Null);
        Assert.That(systemUnderTest.Instance.LearningWorldPath, Is.EqualTo(savedLearningWorldPath));
        Assert.That(systemUnderTest.Instance.OnOpenLearningWorld, Is.EqualTo(onOpenLearningWorld));
        Assert.That(systemUnderTest.Instance.OnCloseWorld, Is.EqualTo(onCloseWorld));
    }

    [Test]
    public void CloseButtonPressed_CallsOnCloseCallback()
    {
        var callbackCallCount = 0;
        var onCloseWorld = EventCallback.Factory.Create<SavedLearningWorldPath>(this, () => { callbackCallCount++; });

        var systemUnderTest = GetRenderedComponent(onCloseWorld: onCloseWorld);

        systemUnderTest.FindComponentWithMarkup<MudFab>("close-button").Find("button").Click();

        Assert.That(callbackCallCount, Is.EqualTo(1));
    }

    [Test]
    public void OpenButtonPressed_CallsOnOpenCallback()
    {
        var callbackCallCount = 0;
        var onOpenLearningWorld =
            EventCallback.Factory.Create<SavedLearningWorldPath>(this, () => { callbackCallCount++; });

        var systemUnderTest = GetRenderedComponent(onOpenLearningWorld: onOpenLearningWorld);

        systemUnderTest.Find("button.open-button").Click();

        Assert.That(callbackCallCount, Is.EqualTo(1));
    }

    private IRenderedComponent<LearningWorldCard> GetRenderedComponent(
        SavedLearningWorldPath? savedLearningWorldPath = null,
        EventCallback<SavedLearningWorldPath>? onOpenLearningWorld = null,
        EventCallback<SavedLearningWorldPath>? onCloseWorld = null)
    {
        savedLearningWorldPath ??= new SavedLearningWorldPath();
        onOpenLearningWorld ??= EventCallback.Factory.Create<SavedLearningWorldPath>(this, () => { });
        onCloseWorld ??= EventCallback.Factory.Create<SavedLearningWorldPath>(this, () => { });
        return Context.RenderComponent<LearningWorldCard>(p =>
        {
            p.Add(c => c.LearningWorldPath, savedLearningWorldPath);
            p.Add(c => c.OnOpenLearningWorld, onOpenLearningWorld.Value);
            p.Add(c => c.OnCloseWorld, onCloseWorld.Value);
        });
    }
}