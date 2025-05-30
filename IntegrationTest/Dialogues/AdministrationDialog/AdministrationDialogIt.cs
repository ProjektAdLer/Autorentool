using System;
using System.Threading.Tasks;
using Bunit;
using Bunit.Rendering;
using Bunit.TestDoubles;
using MudBlazor;
using NUnit.Framework;
using Presentation.Components.Dialogues.AdministrationDialog;
using PresentationTest;

namespace IntegrationTest.Dialogues.AdministrationDialog;

[TestFixture]
public class AdministrationDialogIt : MudDialogTestFixture<
    Presentation.Components.Dialogues.AdministrationDialog.AdministrationDialog>
{
    [SetUp]
    public void SetUp()
    {
        Context.ComponentFactories.AddStub<LmsLoginDialog>();
        Context.ComponentFactories.AddStub<HelpDialog>();
        Context.ComponentFactories.AddStub<LanguageDialog>();
        Context.ComponentFactories.AddStub<ArchiveDialog>();
        Context.RenderComponent<MudPopoverProvider>();
    }

    [Test]
    public async Task DialogCreated_DependenciesInjected()
    {
        await OpenDialogAndGetDialogReferenceAsync();

        var systemUnderTest = DialogProvider
            .FindComponentOrFail<Presentation.Components.Dialogues.AdministrationDialog.AdministrationDialog>();

        Assert.That(systemUnderTest, Is.Not.Null);
        Assert.That(systemUnderTest.Instance, Is.Not.Null);
    }

    [Test]
    public async Task DialogCreated_ShowsTabs()
    {
        await OpenDialogAndGetDialogReferenceAsync();

        var systemUnderTest = DialogProvider
            .FindComponentOrFail<Presentation.Components.Dialogues.AdministrationDialog.AdministrationDialog>();
        var tabs = systemUnderTest.FindComponents<MudTabPanel>();
        Assert.Multiple(() =>
        {
            Assert.That(tabs, Is.Not.Null);
            Assert.That(tabs, Has.Count.EqualTo(4));
            Assert.That(tabs[0].Instance, Is.Not.Null);
            Assert.That(tabs[0].Instance.Icon, Is.Not.Null);
            Assert.That(tabs[1].Instance, Is.Not.Null);
            Assert.That(tabs[1].Instance.Icon, Is.Not.Null);
            Assert.That(tabs[2].Instance, Is.Not.Null);
            Assert.That(tabs[2].Instance.Icon, Is.EqualTo(Icons.Material.Filled.Language));
            Assert.That(tabs[3].Instance, Is.Not.Null);
            Assert.That(tabs[3].Instance.Icon, Is.EqualTo(Icons.Material.Filled.Unarchive));
        });
    }

    [Test]
    public async Task DialogCreated_ShowsLmsLoginDialog()
    {
        await OpenDialogAndGetDialogReferenceAsync();
        var systemUnderTest = DialogProvider
            .FindComponentOrFail<Presentation.Components.Dialogues.AdministrationDialog.AdministrationDialog>();
        Assert.Multiple(() =>
        {
            Assert.That(() => systemUnderTest.FindComponent<Stub<LmsLoginDialog>>(), Throws.Nothing);
            Assert.That(() => systemUnderTest.FindComponent<Stub<HelpDialog>>(),
                Throws.InstanceOf<ComponentNotFoundException>());
            Assert.That(() => systemUnderTest.FindComponent<Stub<LanguageDialog>>(),
                Throws.InstanceOf<ComponentNotFoundException>());
            Assert.That(() => systemUnderTest.FindComponent<Stub<ArchiveDialog>>(),
                Throws.InstanceOf<ComponentNotFoundException>());
        });
    }

    [Test]
    [TestCase(0, typeof(LmsLoginDialog))]
    [TestCase(1, typeof(HelpDialog))]
    [TestCase(2, typeof(LanguageDialog))]
    [TestCase(3, typeof(ArchiveDialog))]
    public async Task SelectingTab_ShowsCorrectDialog(int position, Type expectedDialog)
    {
        await OpenDialogAndGetDialogReferenceAsync();
        var systemUnderTest = DialogProvider
            .FindComponentOrFail<Presentation.Components.Dialogues.AdministrationDialog.AdministrationDialog>();
        var tabs = systemUnderTest.FindComponents<MudTabPanel>();

        Assert.That(tabs, Is.Not.Null);
        Assert.That(tabs, Has.Count.EqualTo(4));
        Assert.That(tabs[position].Instance, Is.Not.Null);
        Assert.That(tabs[position].Instance.Icon, Is.Not.Null);
        await DialogProvider.InvokeAsync(async () => { await tabs[position].Instance.OnClick.InvokeAsync(null); });

        Assert.Multiple(() =>
        {
            Assert.That(() => systemUnderTest.FindComponent<Stub<LmsLoginDialog>>(),
                expectedDialog == typeof(LmsLoginDialog)
                    ? Throws.Nothing
                    : Throws.InstanceOf<ComponentNotFoundException>());
            Assert.That(() => systemUnderTest.FindComponent<Stub<HelpDialog>>(),
                expectedDialog == typeof(HelpDialog)
                    ? Throws.Nothing
                    : Throws.InstanceOf<ComponentNotFoundException>());
            Assert.That(() => systemUnderTest.FindComponent<Stub<LanguageDialog>>(),
                expectedDialog == typeof(LanguageDialog)
                    ? Throws.Nothing
                    : Throws.InstanceOf<ComponentNotFoundException>());
            Assert.That(() => systemUnderTest.FindComponent<Stub<ArchiveDialog>>(),
                expectedDialog == typeof(ArchiveDialog)
                    ? Throws.Nothing
                    : Throws.InstanceOf<ComponentNotFoundException>());
        });
    }

    [Test]
    public async Task CloseDialog_ClosesDialog()
    {
        await OpenDialogAndGetDialogReferenceAsync();
        var systemUnderTest = DialogProvider
            .FindComponentOrFail<Presentation.Components.Dialogues.AdministrationDialog.AdministrationDialog>();

        Assert.That(systemUnderTest, Is.Not.Null);
        Assert.That(systemUnderTest.Instance, Is.Not.Null);

        var closeButton = systemUnderTest.FindComponent<MudIconButton>();
        Assert.That(closeButton, Is.Not.Null);
        Assert.That(closeButton.Instance, Is.Not.Null);

        await DialogProvider.InvokeAsync(async () => { await closeButton.Instance.OnClick.InvokeAsync(null); });
        DialogProvider.Render();

        Assert.That(
            DialogProvider.FindComponent<Presentation.Components.Dialogues.AdministrationDialog.AdministrationDialog>,
            Throws.InstanceOf<ComponentNotFoundException>()
        );
    }
}