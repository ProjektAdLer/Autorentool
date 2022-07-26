using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.AuthoringToolWorkspace;

[TestFixture]
public class AuthoringToolWorkspacePresenterUt
{
    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        var workspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CreateLearningSpaceDialogueOpen, Is.EqualTo(false));
            Assert.That(systemUnderTest.CreateLearningWorldDialogOpen, Is.EqualTo(false));
            Assert.That(systemUnderTest.EditLearningSpaceDialogOpen, Is.EqualTo(false));
            Assert.That(systemUnderTest.EditLearningWorldDialogOpen, Is.EqualTo(false));
        });
    }
    
    #region CreateNewLearningWorld

    [Test]
    public void OnCreateWorldDialogClose_DialogDataAreNull_ThrowsException()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateWorldDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }

    [Test]
    public void OnCreateWorldDialogClose_CallsLearningWorldPresenter()
    {
        var learningWorldPresenter = Substitute.For<ILearningWorldPresenter>();
        learningWorldPresenter.CreateNewLearningWorld(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(new LearningWorldViewModel("a", "b", "c", "d", "e", "f"));

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Authors"] = "a";
        dictionary["Language"] = "a";
        dictionary["Description"] = "d";
        dictionary["Goals"] = "g";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningWorldPresenter: learningWorldPresenter);

        systemUnderTest.OnCreateWorldDialogClose(returnValueTuple);

        learningWorldPresenter.Received().CreateNewLearningWorld("n", "sn", "a", "a", "d", "g");
    }
    
    
    [Test]
    public void CreateNewLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
        var callbackCalled = false;
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);

        systemUnderTest.OnLearningWorldCreate += (_, world) => {
            callbackCalled = true;
            Assert.That(world, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(world!.Name, Is.EqualTo("foo"));
                Assert.That(world.Shortname, Is.EqualTo("f"));
                Assert.That(world.Authors, Is.EqualTo("bar"));
                Assert.That(world.Language, Is.EqualTo("de"));
                Assert.That(world.Description, Is.EqualTo("A test"));
                Assert.That(world.Goals, Is.EqualTo("testing"));
            });
        };
        systemUnderTest.CreateNewLearningWorld("foo", "f", "bar", "de", "A test", "testing");
        Assert.That(callbackCalled, Is.EqualTo(true));
    }

    [Test]
    public void CreateNewLearningWorld_CallsLearningWorldPresenter()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.CreateNewLearningWorld(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo",
                "Foo", "Foo"));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        systemUnderTest.CreateNewLearningWorld("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        
        worldPresenter.Received()
            .CreateNewLearningWorld(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }
    
    [Test]
    public void CreateNewLearningWorld_AddsWorldToWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter);

        Assert.That(workspaceVm.LearningWorlds, Is.Empty);
        systemUnderTest.CreateNewLearningWorld("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        Assert.That(workspaceVm.LearningWorlds.Count(), Is.EqualTo(1));
        var learningWorldViewModel = workspaceVm.LearningWorlds.First();
        Assert.Multiple(() =>
        {
            Assert.That(learningWorldViewModel.Name, Is.EqualTo("Foo"));
            Assert.That(learningWorldViewModel.Shortname, Is.EqualTo("Foo"));
            Assert.That(learningWorldViewModel.Authors, Is.EqualTo("Foo"));
            Assert.That(learningWorldViewModel.Language, Is.EqualTo("Foo"));
            Assert.That(learningWorldViewModel.Description, Is.EqualTo("Foo"));
            Assert.That(learningWorldViewModel.Goals, Is.EqualTo("Foo"));
        });
    }
    
    #endregion
    
    #region ChangeSelectedLearningWorld

    [Test]
    public void ChangeSelectedLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
        var firstCallbackCalled = false;
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);

        EventHandler<LearningWorldViewModel?> firstCallback = (_, world) => {
            firstCallbackCalled = true;
            Assert.That(world, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(world!.Name, Is.EqualTo("tetete"));
                Assert.That(world.Shortname, Is.EqualTo("f"));
                Assert.That(world.Authors, Is.EqualTo("bar"));
                Assert.That(world.Language, Is.EqualTo("de"));
                Assert.That(world.Description, Is.EqualTo("A test"));
                Assert.That(world.Goals, Is.EqualTo("testing"));
            });
        };
        
        systemUnderTest.OnLearningWorldSelect += firstCallback;
        
        systemUnderTest.CreateNewLearningWorld("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        systemUnderTest.CreateNewLearningWorld("tetete", "f", "bar", "de", "A test",
            "testing");
        systemUnderTest.SetSelectedLearningWorld("tetete");
        
        Assert.That(firstCallbackCalled, Is.EqualTo(true));
        firstCallbackCalled = false;
        systemUnderTest.OnLearningWorldSelect -= firstCallback;
        
        var secondCallbackCalled = false;
        systemUnderTest.OnLearningWorldSelect += (_, world) => {
            secondCallbackCalled = true;
            Assert.That(world, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(world!.Name, Is.EqualTo("Foo"));
                Assert.That(world.Shortname, Is.EqualTo("Foo"));
                Assert.That(world.Authors, Is.EqualTo("Foo"));
                Assert.That(world.Language, Is.EqualTo("Foo"));
                Assert.That(world.Description, Is.EqualTo("Foo"));
                Assert.That(world.Goals, Is.EqualTo("Foo"));
                Assert.That(systemUnderTest.LearningWorldSelected, Is.True);
            });
        };
        
        systemUnderTest.SetSelectedLearningWorld("Foo");
        Assert.That(secondCallbackCalled, Is.EqualTo(true));
    }

    [Test]
    public void ChangeSelectedLearningWorld_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.AddLearningWorld(world1);
        workspaceVm.AddLearningWorld(world2);
        

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        Assert.That(workspaceVm.SelectedLearningWorld, Is.Null);
        
        systemUnderTest.SetSelectedLearningWorld("tetete");
        Assert.That(workspaceVm.SelectedLearningWorld, Is.EqualTo(world2));
        
        systemUnderTest.SetSelectedLearningWorld("Foo");
        Assert.That(workspaceVm.SelectedLearningWorld, Is.EqualTo(world1));
    }
    
    [Test]
    public void ChangeSelectedLearningWorld_ThrowsIfNoLearningWorldWithName() 
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
        Assert.That(workspaceVm.LearningWorlds, Is.Empty);
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.SetSelectedLearningWorld("foo"));
        Assert.That(ex!.Message, Is.EqualTo("no world with that name in viewmodel"));
    }
    
    #endregion

    #region DeleteSelectedLearningWorld

    [Test]
    public void DeleteSelectedLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.AddLearningWorld(world1);
        workspaceVm.AddLearningWorld(world2);
        EventHandler<LearningWorldViewModel?> firstCallback = (_, _) =>
        {
            Assert.Fail("first callback with null as selected world should not have been called");
        };
        var secondCallbackCalled = false;
        EventHandler<LearningWorldViewModel?> secondCallback = (_, world) =>
        {
            secondCallbackCalled = true;
            Assert.That(world, Is.EqualTo(world1));
        };
        var thirdCallbackCalled = false;
        EventHandler<LearningWorldViewModel?> thirdCallback = (_, world) =>
        {
            thirdCallbackCalled = true;
            Assert.That(world, Is.EqualTo(world2));
        };
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        systemUnderTest.OnLearningWorldDelete += firstCallback;
         
        systemUnderTest.DeleteSelectedLearningWorld();

        systemUnderTest.OnLearningWorldDelete -= firstCallback;
        systemUnderTest.OnLearningWorldDelete += secondCallback;
        
        systemUnderTest.SetSelectedLearningWorld(world1.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        
        Assert.That(secondCallbackCalled, Is.True);
        systemUnderTest.OnLearningWorldDelete -= secondCallback;
        systemUnderTest.OnLearningWorldDelete += thirdCallback;
        
        systemUnderTest.SetSelectedLearningWorld(world2.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        
        Assert.That(thirdCallbackCalled, Is.True);
    }

    [Test]
    public void DeleteSelectedLearningWorld_DeletesWorldFromViewModelAndSetsUnsaved()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo")
        {
            UnsavedChanges = true
        };
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.AddLearningWorld(world1);
        workspaceVm.AddLearningWorld(world2);
        
        Assert.That(workspaceVm.LearningWorlds.Count(), Is.EqualTo(2));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world1));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world2));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.That(workspaceVm.LearningWorlds.Count(), Is.EqualTo(2));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world1));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world2));
        
        systemUnderTest.SetSelectedLearningWorld(world1.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.That(workspaceVm.LearningWorlds.Count(), Is.EqualTo(1));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world2));
        Assert.That(systemUnderTest.DeletedUnsavedWorld, Is.EqualTo(world1));
        
        systemUnderTest.SetSelectedLearningWorld(world2.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.That(workspaceVm.LearningWorlds, Is.Empty);
    }
    
    [Test]
    public void DeleteSelectedLearningWorld_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.AddLearningWorld(world1);
        workspaceVm.AddLearningWorld(world2);
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        systemUnderTest.SetSelectedLearningWorld(world2.Name);
        Assert.That(workspaceVm.SelectedLearningWorld, Is.EqualTo(world2));
        
        systemUnderTest.DeleteSelectedLearningWorld();
        
        Assert.That(workspaceVm.SelectedLearningWorld, Is.EqualTo(world1));
    }

    [Test]
    public void DeleteSelectedLearningWorld_DoesNotThrowWhenSelectedWorldNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter);

        Assert.DoesNotThrow(systemUnderTest.DeleteSelectedLearningWorld);
    }
    
    #endregion
    
    #region EditCurrentLearningWorld

    [Test]
    public void OpenEditSelectedLearningWorldDialog_CallsMethod()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        workspaceVm.AddLearningWorld(world);
        workspaceVm.EditDialogInitialValues = new Dictionary<string, string>();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        systemUnderTest.SetSelectedLearningWorld(world);

        systemUnderTest.OpenEditSelectedLearningWorldDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditLearningWorldDialogOpen, Is.True);
            Assert.That(workspaceVm.EditDialogInitialValues["Name"], Is.EqualTo(world.Name));
            Assert.That(workspaceVm.EditDialogInitialValues["Shortname"], Is.EqualTo(world.Shortname));
            Assert.That(workspaceVm.EditDialogInitialValues["Authors"], Is.EqualTo(world.Authors));
            Assert.That(workspaceVm.EditDialogInitialValues["Language"], Is.EqualTo(world.Language));
            Assert.That(workspaceVm.EditDialogInitialValues["Description"], Is.EqualTo(world.Description));
            Assert.That(workspaceVm.EditDialogInitialValues["Goals"], Is.EqualTo(world.Goals));
        });
    }

    [Test]
    public void OpenEditSelectedLearningWorldDialog_SelectedLearningWorldIsNull_ThrowsException()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        workspaceVm.AddLearningWorld(world);
        workspaceVm.EditDialogInitialValues = new Dictionary<string, string>();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OpenEditSelectedLearningWorldDialog());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void OnEditWorldDialogClose_DialogDataAreNull_ThrowsException()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, null);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSelectedLearningWorld(world);
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditWorldDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }

    [Test]
    public void OnEditWorldDialogClose_CallsLearningWorldPresenter()
    {
        var learningWorldPresenter = Substitute.For<ILearningWorldPresenter>();
        learningWorldPresenter.EditLearningWorld(Arg.Any<LearningWorldViewModel>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),Arg.Any<string>())
            .Returns(new LearningWorldViewModel("a", "b", "c", "d", "e", "f"));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Authors"] = "a";
        dictionary["Language"] = "a";
        dictionary["Description"] = "d";
        dictionary["Goals"] = "g";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningWorldPresenter: learningWorldPresenter);
        systemUnderTest.SetSelectedLearningWorld(world);

        systemUnderTest.OnEditWorldDialogClose(returnValueTuple);

        learningWorldPresenter.Received().EditLearningWorld(world, "n", "sn", "a", "a", "d", "g");
    }
    
    [Test]
    public void EditCurrentLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        workspaceVm.AddLearningWorld(world1);
        var callbackCalled = false;
        EventHandler<LearningWorldViewModel?> callback = (_, world) =>
        {
            callbackCalled = true;
            Assert.That(world, Is.Not.Null);
            Assert.That(world, Is.EqualTo(world1));
        };
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        systemUnderTest.OnLearningWorldEdit += callback;
        
        systemUnderTest.SetSelectedLearningWorld(world1.Name);
        
        systemUnderTest.EditSelectedLearningWorld("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        
        Assert.That(callbackCalled, Is.True);
    }

    [Test]
    public void EditCurrentLearningWorld_CallsLearningWorldPresenter()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        worldPresenter.EditLearningWorld(world1, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(world1);
        workspaceVm.AddLearningWorld(world1);
        workspaceVm.SelectedLearningWorld = world1;
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);

        systemUnderTest.EditSelectedLearningWorld("a", "a", "a", "a", "a", "a");

        worldPresenter.Received().EditLearningWorld(world1, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }
    
    [Test]
    public void EditCurrentLearningWorld_SelectedWorldChanged()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.AddLearningWorld(world1);
        workspaceVm.AddLearningWorld(world2);
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        systemUnderTest.SetSelectedLearningWorld(world1.Name);
        
        systemUnderTest.EditSelectedLearningWorld("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        Assert.Multiple(() =>
        {
            Assert.That(world1.Name, Is.EqualTo("Name"));
            Assert.That(world1.Shortname, Is.EqualTo("Shortname"));
            Assert.That(world1.Authors, Is.EqualTo("Authors"));
            Assert.That(world1.Language, Is.EqualTo("Language"));
            Assert.That(world1.Description, Is.EqualTo("Description"));
            Assert.That(world1.Goals, Is.EqualTo("Goals"));
        });
    }

    [Test]
    public void EditCurrentLearningWorld_ThrowsWhenSelectedWorldNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter);
        Assert.That(workspaceVm.SelectedLearningWorld, Is.Null);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.EditSelectedLearningWorld("foo",
            "bar", "this", "does", "not", "matter"));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }
    
    #endregion
    
    #region Save(Selected)LearningWorldAsync

    [Test]
    public async Task SaveLearningWorldAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = new LearningWorldViewModel("fo", "f", "", "f", "", "");

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);

        await systemUnderTest.SaveLearningWorldAsync(learningWorld);
        await presentationLogic.Received().SaveLearningWorldAsync(learningWorld);
    }

    [Test]
    public async Task SaveSelectedLearningWorldAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = new LearningWorldViewModel("fo", "f", "", "f", "", "");
        var viewModel = new AuthoringToolWorkspaceViewModel();
        viewModel.AddLearningWorld(learningWorld);
        viewModel.SelectedLearningWorld = learningWorld;

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, authoringToolWorkspaceVm: viewModel);

        await systemUnderTest.SaveSelectedLearningWorldAsync();
        await presentationLogic.Received().SaveLearningWorldAsync(learningWorld);
    }

    [Test]
    public void SaveSelectedLearningWorldAsync_ThrowsIfSelectedWorldNull()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, authoringToolWorkspaceVm: viewModel);

        Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedLearningWorldAsync());
        
    }
    
    #endregion
    
    #region LoadLearningWorldAsync

    [Test]
    public async Task LoadLearningWorldAsync_CallsPresentationLogicAndAddsToViewModel()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        presentationLogic.LoadLearningWorldAsync().Returns(learningWorld);
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic);

        await systemUnderTest.LoadLearningWorldAsync();
        await presentationLogic.Received().LoadLearningWorldAsync();
        Assert.That(viewModel.LearningWorlds, Contains.Item(learningWorld));
    }

    [Test]
    public async Task
        LoadLearningWorldAsync_SetsWorldToReplaceWithWhenWorldWithNameExists()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var learningWorld2 = new LearningWorldViewModel("f", "fu", "fu", "fu", "fu", "fu");
        presentationLogic.LoadLearningWorldAsync().Returns(learningWorld2);
        var viewModel = new AuthoringToolWorkspaceViewModel();
        viewModel.AddLearningWorld(learningWorld);
        
        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic);

        await systemUnderTest.LoadLearningWorldAsync();
        Assert.That(systemUnderTest.WorldToReplaceWith, Is.EqualTo(learningWorld2));
    }
    
    #endregion
    
    #region ReplaceLearningWorld
    
    [Test]
    public void ReplaceLearningWorld_ReplacesWorldAndSetsReplacedUnsavedWorld() 
    {
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f")
        {
            UnsavedChanges = true
        };
        var learningWorld2 = new LearningWorldViewModel("f", "fu", "fu", "fu", "fu", "fu");
        var viewModel = new AuthoringToolWorkspaceViewModel();
        viewModel.AddLearningWorld(learningWorld);
        viewModel.SelectedLearningWorld = learningWorld;

        var systemUnderTest = CreatePresenterForTesting(viewModel);
        
        systemUnderTest.ReplaceLearningWorld(learningWorld2);
        Assert.Multiple(() =>
        {
            Assert.That(viewModel.LearningWorlds, Contains.Item(learningWorld2));
            Assert.That(viewModel.LearningWorlds, Does.Not.Contain(learningWorld));
            Assert.That(viewModel.SelectedLearningWorld, Is.EqualTo(learningWorld2));
            Assert.That(systemUnderTest.ReplacedUnsavedWorld, Is.EqualTo(learningWorld));
        });
    }

    [Test]
    public void ReplaceLearningWorld_MultipleUnSavedReplacedWorlds_ThrowException()
    {
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f")
        {
            UnsavedChanges = true
        };
        var learningWorld2 = new LearningWorldViewModel("f", "fu", "fu", "fu", "fu", "fu");
        var learningWorld3 = new LearningWorldViewModel("fi", "fi", "fi", "fi", "fi", "fi");
        var viewModel = new AuthoringToolWorkspaceViewModel();
        viewModel.AddLearningWorld(learningWorld);
        viewModel.SelectedLearningWorld = learningWorld;

        var systemUnderTest = CreatePresenterForTesting(viewModel);

        systemUnderTest.ReplacedUnsavedWorld = learningWorld3;
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.ReplaceLearningWorld(learningWorld2));
        Assert.That(ex!.Message, Is.EqualTo("multiple unsaved replaced worlds, this should not happen"));
    }
    
    #endregion
    
    #region Shutdown

    [Test]
    public void OnBeforeShutdown_CancelsShutdownCreatesQueueAndInvokesViewUpdate()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel.AddLearningWorld(learningWorld);
        var args = new BeforeShutdownEventArgs();
        var callbackCalled = false;
        var callback = () => { callbackCalled = true; };

        var systemUnderTest = CreatePresenterForTesting(viewModel);
        systemUnderTest.OnForceViewUpdate += callback;
        
        systemUnderTest.OnBeforeShutdown(null, args);
        Assert.That(systemUnderTest.UnsavedWorldsQueue, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(args.CancelShutdownState, Is.True);
            Assert.That(systemUnderTest.UnsavedWorldsQueue, Contains.Item(learningWorld));
            Assert.That(systemUnderTest.SaveUnsavedChangesDialogOpen, Is.True);
            Assert.That(callbackCalled, Is.True);
        });
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void CompletedSaveQueue_DeletesQueueAndRecallsShutdownManager(bool cancelled)
    {
        var shutdownManager = Substitute.For<IShutdownManager>();

        var systemUnderTest = CreatePresenterForTesting(shutdownManager: shutdownManager);
        systemUnderTest.SaveUnsavedChangesDialogOpen = true;
        systemUnderTest.UnsavedWorldsQueue = new Queue<LearningWorldViewModel>();
        
        systemUnderTest.CompletedSaveQueue(cancelled);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SaveUnsavedChangesDialogOpen, Is.False);
            Assert.That(systemUnderTest.UnsavedWorldsQueue, Is.Null);
        });
        if (!cancelled)
            shutdownManager.Received().BeginShutdown();
    }
    
    #endregion

    #region DragAndDrop

    [Test]
    [TestCase("awf"), TestCase("asf"), TestCase("aef"), 
     TestCase("jpg"), TestCase("png"), TestCase("webp"), TestCase("bmp"), 
     TestCase("mp4"), TestCase("h5p"), TestCase("pdf"), TestCase("unsupportedEnding")]
    public void ProcessDragAndDropResult_CallsPresentationLogic(string ending)
    {
        var fileName = "testFile." + ending;
        var stream = Substitute.For<Stream>();
        var resultTuple = new Tuple<string, Stream>(fileName, stream);
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        string[] saveFileEndings = {"awf", "asf", "aef"};
        if (!saveFileEndings.Contains(ending))
        {
            var learningWorld = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
                    authoringToolWorkspace.AddLearningWorld(learningWorld);
                    authoringToolWorkspace.SelectedLearningWorld = learningWorld;
        }
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningContent = new LearningContentViewModel(fileName, ending, Array.Empty<byte>());
        presentationLogic.LoadLearningContentViewModelFromStream(Arg.Any<string>(), Arg.Any<Stream>())
            .Returns(learningContent);
        var learningWorldPresenter = Substitute.For<ILearningWorldPresenter>();
        var logger = Substitute.For<ILogger<AuthoringToolWorkspacePresenter>>();
        var systemUnderTest = 
            CreatePresenterForTesting(authoringToolWorkspace, presentationLogic, learningWorldPresenter, logger: logger);

        systemUnderTest.ProcessDragAndDropResult(resultTuple);

        switch (ending)
        {
            case "awf":
                presentationLogic.Received().LoadLearningWorldViewModelFromStream(stream);
                break;
            case "asf":
                presentationLogic.Received().LoadLearningSpaceViewModelFromStream(stream);
                break;
            case "aef":
                presentationLogic.Received().LoadLearningElementViewModelFromStream(stream);
                break;
            case "jpg":
            case "png":
            case "webp":
            case "bmp":
            case "mp4":
            case "h5p":
            case "pdf":
                presentationLogic.Received().LoadLearningContentViewModelFromStream(fileName, stream);
                learningWorldPresenter.Received().CreateLearningElementWithPreloadedContent(learningContent);
                break;
            default:
                //logger.Received().Log(LogLevel.Information,$"Couldn't load file 'testFile.{ending}', because the file extension '{ending}' is not supported.");
                Assert.Pass();
                break;
        }
    }

    [Test]
    public void
        CallCreateLearningElementWithPreloadedContentFromActiveView_NoLearningWorldSelected_DoNothing()
    {
        var learningContent = new LearningContentViewModel("n", "t", Array.Empty<byte>());

        var systemUnderTest = CreatePresenterForTesting();
        
        systemUnderTest.CallCreateLearningElementWithPreloadedContentFromActiveView(learningContent);
        
        Assert.Pass();
    }
    
    [Test]
    public void
        CallCreateLearningElementWithPreloadedContentFromActiveView_LearningWorldSelected_CallsWorldPresenter()
    {
        var learningContent = new LearningContentViewModel("n", "t", Array.Empty<byte>());
        var learningWorld = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
        var authoringToolWorkspaceVm = new AuthoringToolWorkspaceViewModel();
        authoringToolWorkspaceVm.AddLearningWorld(learningWorld);
        authoringToolWorkspaceVm.SelectedLearningWorld = learningWorld;
        var learningWorldPresenter = Substitute.For<ILearningWorldPresenter>();

        var systemUnderTest =
            CreatePresenterForTesting(authoringToolWorkspaceVm, learningWorldPresenter: learningWorldPresenter);
        
        systemUnderTest.CallCreateLearningElementWithPreloadedContentFromActiveView(learningContent);
        
        learningWorldPresenter.Received().CreateLearningElementWithPreloadedContent(learningContent);
    }
    
    [Test]
    public void
        CallCreateLearningElementWithPreloadedContentFromActiveView_LearningSpaceSelected_CallsSpacePresenter()
    {
        var learningContent = new LearningContentViewModel("n", "t", Array.Empty<byte>());
        var learningWorld = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
        var learningSpace = new LearningSpaceViewModel("n", "sn", "a", "d", "g");
        learningWorld.LearningSpaces.Add(learningSpace);
        learningWorld.SelectedLearningObject = learningSpace;
        learningWorld.ShowingLearningSpaceView = true;
        var authoringToolWorkspaceVm = new AuthoringToolWorkspaceViewModel();
        authoringToolWorkspaceVm.AddLearningWorld(learningWorld);
        authoringToolWorkspaceVm.SelectedLearningWorld = learningWorld;
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        learningSpacePresenter.LearningSpaceVm.Returns(learningSpace);

        var systemUnderTest =
            CreatePresenterForTesting(authoringToolWorkspaceVm, learningSpacePresenter: learningSpacePresenter);
        
        systemUnderTest.CallCreateLearningElementWithPreloadedContentFromActiveView(learningContent);
        
        learningSpacePresenter.Received().CreateLearningElementWithPreloadedContent(learningContent);
    }
    
    [Test]
    public void
        CallCreateLearningElementWithPreloadedContentFromActiveView_LearningSpaceSelectedButSpaceVmIsNull_DoNothing()
    {
        var learningContent = new LearningContentViewModel("n", "t", Array.Empty<byte>());
        var learningWorld = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
        var learningSpace = new LearningSpaceViewModel("n", "sn", "a", "d", "g");
        learningWorld.LearningSpaces.Add(learningSpace);
        learningWorld.SelectedLearningObject = learningSpace;
        learningWorld.ShowingLearningSpaceView = true;
        var authoringToolWorkspaceVm = new AuthoringToolWorkspaceViewModel();
        authoringToolWorkspaceVm.AddLearningWorld(learningWorld);
        authoringToolWorkspaceVm.SelectedLearningWorld = learningWorld;
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        //learningSpacePresenter.LearningSpaceVm.Returns(learningSpace);

        var systemUnderTest =
            CreatePresenterForTesting(authoringToolWorkspaceVm, learningSpacePresenter: learningSpacePresenter);
        
        systemUnderTest.CallCreateLearningElementWithPreloadedContentFromActiveView(learningContent);
        
        Assert.Pass();
    }

    [Test]
    public void LoadLearningWorldFromFileStream_CallsPresentationLogic()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.LoadLearningWorldViewModelFromStream(Arg.Any<Stream>())
            .Returns(new LearningWorldViewModel("n", "sn", "a", "l", "d", "g"));
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningWorldFromFileStream(stream);

        presentationLogic.Received().LoadLearningWorldViewModelFromStream(stream);
    }

    [Test]
    public void
        LoadLearningWorldFromFileStream_SetsWorldToReplaceWithIfWorldWithSameNameAlreadyExists()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        authoringToolWorkspace.AddLearningWorld(new LearningWorldViewModel("n", "x", "x", "x", "x", "x"));
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var newLearningWorld = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
        presentationLogic.LoadLearningWorldViewModelFromStream(Arg.Any<Stream>())
            .Returns(newLearningWorld);
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic);
        var stream = Substitute.For<Stream>();

        Assert.That(systemUnderTest.WorldToReplaceWith, Is.Null);

        systemUnderTest.LoadLearningWorldFromFileStream(stream);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.WorldToReplaceWith, Is.Not.Null);
            Assert.That(systemUnderTest.WorldToReplaceWith!, Is.EqualTo(newLearningWorld));
        });
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void LoadLearningWorldFromFileStream_AddsAndSetSelectedLearningWorld(
        bool otherLearningWorldWasSelected)
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var existingLearningWorld = new LearningWorldViewModel("existing", "x", "x", "x", "x", "x");
        authoringToolWorkspace.AddLearningWorld(existingLearningWorld);
        if (otherLearningWorldWasSelected)
        {
            authoringToolWorkspace.SelectedLearningWorld = existingLearningWorld;
        }

        var presentationLogic = Substitute.For<IPresentationLogic>();
        var newLearningWorld = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
        presentationLogic.LoadLearningWorldViewModelFromStream(Arg.Any<Stream>())
            .Returns(newLearningWorld);
        var stream = Substitute.For<Stream>();
        var callbackCalled = false;
        LearningWorldViewModel? callbackSelectedWorld = null;
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic);
        EventHandler<LearningWorldViewModel?> callback = delegate(object? sender, LearningWorldViewModel? model)
        {
            callbackCalled = true;
            callbackSelectedWorld = model;
        };
        systemUnderTest.OnLearningWorldSelect += callback;

        if (otherLearningWorldWasSelected)
        {
            Assert.That(authoringToolWorkspace.SelectedLearningWorld, Is.EqualTo(existingLearningWorld));
        }
        else
        {
            Assert.That(authoringToolWorkspace.SelectedLearningWorld, Is.Null);
        }

        systemUnderTest.LoadLearningWorldFromFileStream(stream);

        Assert.That(callbackCalled, Is.True);
        if (otherLearningWorldWasSelected)
        {
            Assert.That(authoringToolWorkspace.SelectedLearningWorld, Is.EqualTo(existingLearningWorld));
            Assert.That(callbackSelectedWorld, Is.EqualTo(existingLearningWorld));
        }
        else
        {
            Assert.That(authoringToolWorkspace.SelectedLearningWorld, Is.EqualTo(newLearningWorld));
            Assert.That(callbackSelectedWorld, Is.EqualTo(newLearningWorld));
        }
    }

    [Test]
    public void LoadLearningSpaceFromFileStream_CallsPresentationLogic()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.LoadLearningSpaceViewModelFromStream(Arg.Any<Stream>())
            .Returns(new LearningSpaceViewModel("n", "sn", "a", "d", "g"));
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningSpaceFromFileStream(stream);

        presentationLogic.Received().LoadLearningSpaceViewModelFromStream(stream);
    }

    [Test]
    public void LoadLearningSpaceFromFileStream_SelectedLearningWorldIsNull_CorrectInformationMessageToShow()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        var stream = Substitute.For<Stream>();
        
        Assert.That(systemUnderTest.InformationMessageToShow, Is.Not.EqualTo("A learning world must be selected to import a learning space."));

        systemUnderTest.LoadLearningSpaceFromFileStream(stream);
        
        Assert.That(systemUnderTest.InformationMessageToShow, Is.EqualTo("A learning world must be selected to import a learning space."));
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void LoadLearningSpaceFromFileStream_AddsAndSetSelectedLearningSpace(
        bool isLearningWorldSelected)
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var existingLearningWorld = new LearningWorldViewModel("existing", "x", "x", "x", "x", "x");
        authoringToolWorkspace.AddLearningWorld(existingLearningWorld);
        if (isLearningWorldSelected)
        {
            authoringToolWorkspace.SelectedLearningWorld = existingLearningWorld;
        }

        var presentationLogic = Substitute.For<IPresentationLogic>();
        var newLearningSpace = new LearningSpaceViewModel("n", "sn", "a", "d", "g");
        presentationLogic.LoadLearningSpaceViewModelFromStream(Arg.Any<Stream>())
            .Returns(newLearningSpace);
        var stream = Substitute.For<Stream>();
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic);

        Assert.That(existingLearningWorld.LearningSpaces, Is.Empty);

        systemUnderTest.LoadLearningSpaceFromFileStream(stream);

        if (isLearningWorldSelected)
        {
            Assert.That(existingLearningWorld.LearningSpaces, Contains.Item(newLearningSpace));
            Assert.That(existingLearningWorld.SelectedLearningObject, Is.EqualTo(newLearningSpace));
        }
        else
        {
            Assert.That(existingLearningWorld.LearningSpaces, Is.Empty);
        }
    }

    [Test]
    public void LoadLearningElementFromFileStream_CallsPresentationLogic()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var newLearningElement = new LearningElementViewModel("n", "sn", null,
            new LearningContentViewModel("n", "t", Array.Empty<byte>()), "a", "d", "g",LearningElementDifficultyEnum.Easy);
        presentationLogic.LoadLearningElementViewModelFromStream(Arg.Any<Stream>())
            .Returns(newLearningElement);
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningElementFromFileStream(stream);

        presentationLogic.Received().LoadLearningElementViewModelFromStream(stream);
    }

    [Test]
    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, false)]
    public void LoadLearningElementFromFileStream_AddsAndSetSelectedLearningElement(
        bool showingLearningSpaceView, bool isLearningSpaceVmSet)
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var existingLearningWorld = new LearningWorldViewModel("existingW", "x", "x", "x", "x", "x");
        var existingLearningSpace = new LearningSpaceViewModel("existingS", "sn", "a", "d", "g");
        existingLearningWorld.LearningSpaces.Add(existingLearningSpace);
        existingLearningWorld.SelectedLearningObject = existingLearningSpace;
        authoringToolWorkspace.AddLearningWorld(existingLearningWorld);
        authoringToolWorkspace.SelectedLearningWorld = existingLearningWorld;

        existingLearningWorld.ShowingLearningSpaceView = showingLearningSpaceView;

        var presentationLogic = Substitute.For<IPresentationLogic>();
        var newLearningElement = new LearningElementViewModel("n", "sn", null,
            new LearningContentViewModel("n", "t", Array.Empty<byte>()), "a", "d", "g",LearningElementDifficultyEnum.Easy);
        presentationLogic.LoadLearningElementViewModelFromStream(Arg.Any<Stream>()).Returns(newLearningElement);
        var stream = Substitute.For<Stream>();
        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        if (isLearningSpaceVmSet)
        {
            spacePresenter.LearningSpaceVm.Returns(existingLearningSpace);
        }

        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic,
            learningSpacePresenter: spacePresenter);

        systemUnderTest.LoadLearningElementFromFileStream(stream);

        if (showingLearningSpaceView)
        {
            if (isLearningSpaceVmSet)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(existingLearningSpace.LearningElements, Contains.Item(newLearningElement));
                    Assert.That(existingLearningSpace.SelectedLearningObject, Is.EqualTo(newLearningElement));
                });
                Assert.That(((LearningElementViewModel) existingLearningSpace.SelectedLearningObject!).Parent,
                    Is.EqualTo(existingLearningSpace));
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.That(existingLearningSpace.LearningElements, Is.Empty);
                    Assert.That(existingLearningSpace.SelectedLearningObject, Is.Null);
                });
            }
        }
        else
        {
            Assert.Multiple(() =>
            {
                Assert.That(existingLearningWorld.LearningElements, Contains.Item(newLearningElement));
                Assert.That(existingLearningWorld.SelectedLearningObject, Is.EqualTo(newLearningElement));
                Assert.That(((LearningElementViewModel) existingLearningWorld.SelectedLearningObject).Parent,
                    Is.EqualTo(existingLearningWorld));
            });
        }
    }

    #endregion


    private AuthoringToolWorkspacePresenter CreatePresenterForTesting(
        IAuthoringToolWorkspaceViewModel? authoringToolWorkspaceVm = null, IPresentationLogic? presentationLogic = null,
        ILearningWorldPresenter? learningWorldPresenter = null, ILearningSpacePresenter? learningSpacePresenter = null,
        ILearningElementPresenter? learningElementPresenter = null,
        ILogger<AuthoringToolWorkspacePresenter>? logger = null,
        IShutdownManager? shutdownManager = null)
    {
        authoringToolWorkspaceVm ??= Substitute.For<IAuthoringToolWorkspaceViewModel>();
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningWorldPresenter ??= Substitute.For<ILearningWorldPresenter>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        learningElementPresenter ??= Substitute.For<ILearningElementPresenter>();
        logger ??= Substitute.For<ILogger<AuthoringToolWorkspacePresenter>>();
        shutdownManager ??= Substitute.For<IShutdownManager>();
        return new AuthoringToolWorkspacePresenter(authoringToolWorkspaceVm, presentationLogic, learningWorldPresenter,
            learningSpacePresenter, learningElementPresenter, logger, shutdownManager);
    }

    private LearningWorldPresenter CreateLearningWorldPresenter(IPresentationLogic? presentationLogic = null,
        ILearningSpacePresenter? learningSpacePresenter = null,
        ILearningElementPresenter? learningElementPresenter = null,
        ILogger<LearningWorldPresenter>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        learningElementPresenter ??= Substitute.For<ILearningElementPresenter>();
        logger ??= Substitute.For<ILogger<LearningWorldPresenter>>();
        return new LearningWorldPresenter(presentationLogic, learningSpacePresenter, learningElementPresenter, logger);
    }
}