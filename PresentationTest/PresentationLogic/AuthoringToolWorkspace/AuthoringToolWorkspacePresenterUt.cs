using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;
using Shared;

namespace PresentationTest.PresentationLogic.AuthoringToolWorkspace;

[TestFixture]
public class AuthoringToolWorkspacePresenterUt
{
    private IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceViewModel;

    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        var workspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var worldPresenter = Substitute.For<IWorldPresenter>();
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter:worldPresenter);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CreateSpaceDialogOpen, Is.EqualTo(false));
            Assert.That(systemUnderTest.CreateWorldDialogOpen, Is.EqualTo(false));
            Assert.That(systemUnderTest.EditSpaceDialogOpen, Is.EqualTo(false));
            Assert.That(systemUnderTest.EditWorldDialogOpen, Is.EqualTo(false));
        });
    }
    
    #region CreateNewWorld

    [Test]
    public void AddNewWorld_SetsFieldToTrue()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        Assert.That(!systemUnderTest.CreateWorldDialogOpen);
        
        systemUnderTest.AddNewWorld();
        
        Assert.That(systemUnderTest.CreateWorldDialogOpen);
    }

    [Test]
    public void OnCreateWorldDialogClose_DialogDataAreNull_ThrowsException()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateWorldDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }

    [Test]
    public void OnCreateWorldDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var authoringToolWorkspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Authors"] = "a";
        dictionary["Language"] = "a";
        dictionary["Description"] = "d";
        dictionary["Goals"] = "g";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspaceVm:authoringToolWorkspaceVm, presentationLogic: presentationLogic);

        systemUnderTest.OnCreateWorldDialogClose(returnValueTuple);

        presentationLogic.Received()
            .CreateWorld(authoringToolWorkspaceVm, "n", "sn", "a", "a", "d", "g");
    }

    [Test]
    public void OnCreateWorldDialogClose_EventHandlerCalled()
    {
        var worldVm = new WorldViewModel("n", "sn", "a", "l", "d", "g");
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Authors"] = "a";
        dictionary["Language"] = "a";
        dictionary["Description"] = "d";
        dictionary["Goals"] = "g";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var callbackCreateCalled = false;
        WorldViewModel? callbackCreateArg = null;
        EventHandler<WorldViewModel?> callbackCreate =
            (_, args) =>
            {
                callbackCreateCalled = true;
                if (args != null)
                {
                    callbackCreateArg = args;
                }
            };

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.AddWorld(worldVm);
        systemUnderTest.SetSelectedWorld(worldVm);
        systemUnderTest.OnWorldCreate += callbackCreate;

        systemUnderTest.OnCreateWorldDialogClose(returnValueTuple);
        
        Assert.Multiple(() =>
        {
            Assert.That(callbackCreateCalled, Is.True);
            Assert.That(callbackCreateArg, Is.EqualTo(worldVm));
        });
    }

    #endregion

    #region ChangeSelectedWorld

    [Test]
    public void ChangeSelectedWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        workspaceVm._worlds.Add(new WorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo", "Foo"));
        workspaceVm._worlds.Add(new WorldViewModel("tetete", "f", "bar", "de", "A test", "testing"));
        var firstCallbackCalled = false;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);

        EventHandler<WorldViewModel?> firstCallback = (_, world) =>
        {
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

        systemUnderTest.OnWorldSelect += firstCallback;

        systemUnderTest.SetSelectedWorld("tetete");

        Assert.That(firstCallbackCalled, Is.EqualTo(true));
        firstCallbackCalled = false;
        systemUnderTest.OnWorldSelect -= firstCallback;

        var secondCallbackCalled = false;
        systemUnderTest.OnWorldSelect += (_, world) =>
        {
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
                Assert.That(systemUnderTest.WorldSelected, Is.True);
            });
        };

        systemUnderTest.SetSelectedWorld("Foo");
        Assert.That(secondCallbackCalled, Is.EqualTo(true));
    }

    [Test]
    public void ChangeSelectedWorld_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateWorldPresenter();
        var world1 = new WorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new WorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm._worlds.Add(world1);
        workspaceVm._worlds.Add(world2);
        

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter:worldPresenter);
        
        Assert.That(workspaceVm.SelectedWorld, Is.Null);
        
        systemUnderTest.SetSelectedWorld("tetete");
        Assert.That(workspaceVm.SelectedWorld, Is.EqualTo(world2));
        
        systemUnderTest.SetSelectedWorld("Foo");
        Assert.That(workspaceVm.SelectedWorld, Is.EqualTo(world1));
    }
    
    [Test]
    public void ChangeSelectedWorld_ThrowsIfNoWorldWithName() 
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateWorldPresenter();
        Assert.That(workspaceVm.Worlds, Is.Empty);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter: worldPresenter);

        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.SetSelectedWorld("foo"));
        Assert.That(ex!.Message, Is.EqualTo("no world with that name in viewmodel"));
    }
    
    #endregion

    #region DeleteSelectedWorld

    [Test]
    public void DeleteSelectedWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateWorldPresenter();
        var world1 = new WorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new WorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm._worlds.Add(world1);
        workspaceVm._worlds.Add(world2);
        EventHandler<WorldViewModel?> firstCallback = (_, _) =>
        {
            Assert.Fail("first callback with null as selected world should not have been called");
        };
        var secondCallbackCalled = false;
        EventHandler<WorldViewModel?> secondCallback = (_, world) =>
        {
            secondCallbackCalled = true;
            Assert.That(world, Is.EqualTo(world1));
        };
        var thirdCallbackCalled = false;
        EventHandler<WorldViewModel?> thirdCallback = (_, world) =>
        {
            thirdCallbackCalled = true;
            Assert.That(world, Is.EqualTo(world2));
        };

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter: worldPresenter);

        systemUnderTest.OnWorldDelete += firstCallback;
         
        systemUnderTest.DeleteSelectedWorld();

        systemUnderTest.OnWorldDelete -= firstCallback;
        systemUnderTest.OnWorldDelete += secondCallback;
        
        systemUnderTest.SetSelectedWorld(world1.Name);
        systemUnderTest.DeleteSelectedWorld();
        
        Assert.That(secondCallbackCalled, Is.True);
        systemUnderTest.OnWorldDelete -= secondCallback;
        systemUnderTest.OnWorldDelete += thirdCallback;
        
        systemUnderTest.SetSelectedWorld(world2.Name);
        systemUnderTest.DeleteSelectedWorld();
        
        Assert.That(thirdCallbackCalled, Is.True);
    }

    [Test]
    public void DeleteSelectedWorld_CallsPresentationLogic()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world1 = new WorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        workspaceVm._worlds.Add(world1);
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic);

        systemUnderTest.SetSelectedWorld(world1.Name);
        systemUnderTest.DeleteSelectedWorld();
        
        presentationLogic.Received().DeleteWorld(workspaceVm, world1);
    }

    [Test]
    public void DeleteSelectedWorld_SetsDeletedUnsavedWorld()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new WorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo")
        {
            UnsavedChanges = true
        };
        workspaceVm._worlds.Add(world);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        systemUnderTest.SetSelectedWorld(world);

        systemUnderTest.DeleteSelectedWorld();
        Assert.That(systemUnderTest.DeletedUnsavedWorld, Is.EqualTo(world));
    }

    [Test]
    public void DeleteSelectedWorld_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world1 = new WorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new WorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm._worlds.Add(world1);
        workspaceVm._worlds.Add(world2);

        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic
            .When(x => x.DeleteWorld(Arg.Any<IAuthoringToolWorkspaceViewModel>(),
                Arg.Any<WorldViewModel>()))
            .Do(y =>
            {
                workspaceVm.RemoveWorld(((AuthoringToolWorkspaceViewModel) y.Args()[0]).SelectedWorld!);
                ((AuthoringToolWorkspaceViewModel) y.Args()[0]).SelectedWorld =
                    ((AuthoringToolWorkspaceViewModel) y.Args()[0]).Worlds.LastOrDefault();
            });

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic);

        systemUnderTest.SetSelectedWorld(world2.Name);
        Assert.That(workspaceVm.SelectedWorld, Is.EqualTo(world2));

        systemUnderTest.DeleteSelectedWorld();

        Assert.That(workspaceVm.SelectedWorld, Is.EqualTo(world1));
    }

    [Test]
    public void DeleteSelectedWorld_DoesNotThrowWhenSelectedWorldNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateWorldPresenter();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter: worldPresenter);

        Assert.DoesNotThrow(systemUnderTest.DeleteSelectedWorld);
    }
    
    #endregion
    
    #region EditCurrentWorld

    [Test]
    public void OpenEditSelectedWorldDialog_CallsMethod()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new WorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        workspaceVm._worlds.Add(world);
        workspaceVm.EditDialogInitialValues = new Dictionary<string, string>();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        systemUnderTest.SetSelectedWorld(world);

        systemUnderTest.OpenEditSelectedWorldDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditWorldDialogOpen, Is.True);
            Assert.That(workspaceVm.EditDialogInitialValues["Name"], Is.EqualTo(world.Name));
            Assert.That(workspaceVm.EditDialogInitialValues["Shortname"], Is.EqualTo(world.Shortname));
            Assert.That(workspaceVm.EditDialogInitialValues["Authors"], Is.EqualTo(world.Authors));
            Assert.That(workspaceVm.EditDialogInitialValues["Language"], Is.EqualTo(world.Language));
            Assert.That(workspaceVm.EditDialogInitialValues["Description"], Is.EqualTo(world.Description));
            Assert.That(workspaceVm.EditDialogInitialValues["Goals"], Is.EqualTo(world.Goals));
        });
    }

    [Test]
    public void OpenEditSelectedWorldDialog_SelectedWorldIsNull_ThrowsException()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new WorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        workspaceVm._worlds.Add(world);
        workspaceVm.EditDialogInitialValues = new Dictionary<string, string>();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OpenEditSelectedWorldDialog());
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }

    [Test]
    public void OnEditWorldDialogClose_DialogDataAreNull_ThrowsException()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSelectedWorld(world);
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditWorldDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnEditWorldDialogClose_SelectedWorldIsNull_ThrowsException()
    {
        new WorldViewModel("foo", "foo", "foo", "foo", "foo",
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

        var systemUnderTest = CreatePresenterForTesting();
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditWorldDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }

    [Test]
    public void OnEditWorldDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
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

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSelectedWorld(world);

        systemUnderTest.OnEditWorldDialogClose(returnValueTuple);

        presentationLogic.Received().EditWorld(world, "n", "sn", "a", "a", "d", "g");
    }

    #endregion

    #region Save(Selected)WorldAsync

    [Test]
    public void OnSaveWorldDialogClose_UnsavedWorldsQueueIsNull_ThrowsException()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Yes;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.UnsavedWorldsQueue = null;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnSaveWorldDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("SaveUnsavedChanges modal returned value despite UnsavedWorldsQueue being null"));
    }
    
    [Test]
    public void OnSaveWorldDialogClose_ModalDialogCancelled_CallsCompletedSaveQueue()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Cancel;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SaveUnsavedChangesDialogOpen = true;
        systemUnderTest.UnsavedWorldsQueue = new Queue<WorldViewModel>();
        
        systemUnderTest.OnSaveWorldDialogClose(returnValueTuple);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SaveUnsavedChangesDialogOpen, Is.False);
            Assert.That(systemUnderTest.UnsavedWorldsQueue, Is.Null);
        });
    }
    
    [Test]
    public void OnSaveWorldDialogClose_ModalDialogYes_DequeuesAndSavesWorld()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel._worlds.Add(world);
        
        var modalDialogReturnValue = ModalDialogReturnValue.Yes;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(viewModel);

        systemUnderTest.UnsavedWorldsQueue = new Queue<WorldViewModel>(viewModel.Worlds);

        Assert.That(systemUnderTest.UnsavedWorldsQueue, Contains.Item(world));
        
        systemUnderTest.OnSaveWorldDialogClose(returnValueTuple);

        Assert.That(systemUnderTest.UnsavedWorldsQueue, Is.Null);
    }
    
    [Test]
    public void OnSaveWorldDialogClose_ModalDialogNo_DequeueWorldAndDeletesUnsavedChanges()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel._worlds.Add(world);
        
        var modalDialogReturnValue = ModalDialogReturnValue.No;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(viewModel);

        systemUnderTest.UnsavedWorldsQueue = new Queue<WorldViewModel>(viewModel.Worlds);

        Assert.That(systemUnderTest.UnsavedWorldsQueue, Contains.Item(world));
        Assert.That(world.UnsavedChanges);
        
        systemUnderTest.OnSaveWorldDialogClose(returnValueTuple);

        Assert.That(systemUnderTest.UnsavedWorldsQueue, Is.Null);
        Assert.That(!world.UnsavedChanges);
    }
    
    [Test]
    public void OnSaveWorldDialogClose_SwitchDefaultValue_ThrowsException()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var modalDialogReturnValue = (ModalDialogReturnValue)10;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(viewModel);

        systemUnderTest.UnsavedWorldsQueue = new Queue<WorldViewModel>(viewModel.Worlds);

        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => systemUnderTest.OnSaveWorldDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Unexpected return value of 10 (Parameter 'returnValueTuple')"));
    }
    
    [Test]
    public void OnSaveDeletedWorldDialogClose_DeletedUnsavedWorldIsNull_ThrowsException()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Yes;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.DeletedUnsavedWorld = null;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnSaveDeletedWorldDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("SaveDeletedWorld modal returned value despite DeleteUnsavedWorld being null"));
    }
    
    [Test]
    public void OnSaveDeletedWorldDialogClose_ModalDialogYes_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel._worlds.Add(world);
    
        var modalDialogReturnValue = ModalDialogReturnValue.Yes;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic : presentationLogic);
        systemUnderTest.DeletedUnsavedWorld = world;
    
        systemUnderTest.OnSaveDeletedWorldDialogClose(returnValueTuple);

        presentationLogic.Received().SaveWorldAsync(world);
    }
    
    [Test]
    public void OnSaveDeletedWorldDialogClose_ModalDialogNo()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel._worlds.Add(world);
        
        var modalDialogReturnValue = ModalDialogReturnValue.No;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(viewModel);
        systemUnderTest.DeletedUnsavedWorld = world;
        
        systemUnderTest.OnSaveDeletedWorldDialogClose(returnValueTuple);

        Assert.That(systemUnderTest.DeletedUnsavedWorld, Is.EqualTo(null));
    }
    
    [Test]
    public void  OnSaveDeletedWorldDialogClose_SwitchDefaultValue_ThrowsException()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel._worlds.Add(world);
        
        var modalDialogReturnValue = (ModalDialogReturnValue)10;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(viewModel);
        systemUnderTest.DeletedUnsavedWorld = world;

        Assert.Throws<ArgumentOutOfRangeException>(() => systemUnderTest.OnSaveDeletedWorldDialogClose(returnValueTuple));
    }

    [Test]
    public async Task SaveWorldAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new WorldViewModel("fo", "f", "", "f", "", "");

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);

        await systemUnderTest.SaveWorldAsync(world);
        await presentationLogic.Received().SaveWorldAsync(world);
    }

    [Test]
    public async Task SaveSelectedWorldAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new WorldViewModel("fo", "f", "", "f", "", "");
        var viewModel = new AuthoringToolWorkspaceViewModel();
        viewModel._worlds.Add(world);
        viewModel.SelectedWorld = world;

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, authoringToolWorkspaceVm: viewModel);

        await systemUnderTest.SaveSelectedWorldAsync();
        await presentationLogic.Received().SaveWorldAsync(world);
    }

    [Test]
    public void SaveSelectedWorldAsync_ThrowsIfSelectedWorldNull()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, authoringToolWorkspaceVm: viewModel);

        Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedWorldAsync());
        
    }
    
    #endregion
    
    #region LoadWorldAsync

    [Test]
    public async Task LoadWorldAsync_CallsPresentationLogicAndAddsToViewModel()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");
        presentationLogic
            .When(x => x.LoadWorldAsync(Arg.Any<IAuthoringToolWorkspaceViewModel>()))
            .Do(y =>
            {
                ((AuthoringToolWorkspaceViewModel)y.Args()[0])._worlds.Add(world);
                ((AuthoringToolWorkspaceViewModel)y.Args()[0]).SelectedWorld = world;
            });
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic);

        await systemUnderTest.LoadWorldAsync();
        await presentationLogic.Received().LoadWorldAsync(viewModel);
        Assert.That(viewModel.Worlds, Contains.Item(world));
    }

    #endregion
    
    #region Shutdown

    [Test]
    public void OnBeforeShutdown_CancelsShutdownCreatesQueueAndInvokesViewUpdate()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel._worlds.Add(world);
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
            Assert.That(systemUnderTest.UnsavedWorldsQueue, Contains.Item(world));
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
        systemUnderTest.UnsavedWorldsQueue = new Queue<WorldViewModel>();
        
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
     TestCase("txt"), TestCase("c"), TestCase("h"), TestCase("cpp"), 
     TestCase("cc"), TestCase("c++"), TestCase("py"), TestCase("cs"), 
     TestCase("js"), TestCase("php"), TestCase("html"), TestCase("css"), 
     TestCase("mp4"), TestCase("h5p"), TestCase("pdf"), TestCase("unsupportedEnding")]
    public void ProcessDragAndDropResult_CallsPresentationLogic(string ending)
    {
        var fileName = "testFile." + ending;
        var stream = Substitute.For<MemoryStream>();
        var resultTuple = new Tuple<string, MemoryStream>(fileName, stream);
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        
        var world = new WorldViewModel("n", "sn", "a", "l", "d", "g");
        var space = new SpaceViewModel("a", "b", "c", "d", "e", 5);
        world.Spaces.Add(space);
        world.SelectedObject = space;
        world.ShowingSpaceView = true;
        authoringToolWorkspace._worlds.Add(world);
        authoringToolWorkspace.SelectedWorld = world;
        
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var content = new ContentViewModel(fileName, ending, "");
        presentationLogic.LoadContentViewModel(Arg.Any<string>(), Arg.Any<MemoryStream>())
            .Returns(content);
        var worldPresenter = Substitute.For<IWorldPresenter>();
        var spacePresenter = Substitute.For<ISpacePresenter>();
        spacePresenter.SpaceVm.Returns(space);
        var logger = Substitute.For<ILogger<AuthoringToolWorkspacePresenter>>();
        var systemUnderTest = 
            CreatePresenterForTesting(authoringToolWorkspace, presentationLogic, worldPresenter, spacePresenter, logger: logger);

        systemUnderTest.ProcessDragAndDropResult(resultTuple);

        switch (ending)
        {
            case "awf":
                presentationLogic.Received().LoadWorldViewModel(authoringToolWorkspace, stream);
                break;
            case "asf":
                presentationLogic.Received().LoadSpaceViewModel(world, stream);
                break;
            case "aef":
                presentationLogic.Received().LoadElementViewModel(space, 0, stream);
                break;
            case "jpg":
            case "png":
            case "webp":
            case "bmp":
            case "txt":
            case "c":
            case "h":
            case "cpp":
            case "cc":
            case "c++":
            case "py":
            case "cs":
            case "js":
            case "php":
            case "html":
            case "css":
            case "mp4":
            case "h5p":
            case "pdf":
                presentationLogic.Received().LoadContentViewModel(fileName, stream);
                break;
            default:
                //logger.Received().Log(LogLevel.Information,$"Couldn't load file 'testFile.{ending}', because the file extension '{ending}' is not supported.");
                Assert.Pass();
                break;
        }
    }

    [Test]
    public void
        CallCreateElementWithPreloadedContentFromActiveView_NoWorldSelected_DoNothing()
    {
        var content = new ContentViewModel("n", "t", "");

        var systemUnderTest = CreatePresenterForTesting();
        
        systemUnderTest.CallCreateElementWithPreloadedContentFromActiveView(content);
        
        Assert.Pass();
    }
    
    [Test]
    public void CallCreateElementWithPreloadedContentFromActiveView_ShowingSpaceViewIsFalse_CorrectInformationMessageToShow()
    {
        var content = new ContentViewModel("n", "t", "");
        var world = new WorldViewModel("n", "sn", "a", "l", "d", "g");
        var space = new SpaceViewModel("n", "sn", "a", "d", "g");
        world.Spaces.Add(space);
        world.SelectedObject = space;
        world.ShowingSpaceView = false;
        var authoringToolWorkspaceVm = new AuthoringToolWorkspaceViewModel();
        authoringToolWorkspaceVm._worlds.Add(world);
        authoringToolWorkspaceVm.SelectedWorld = world;
        var spacePresenter = Substitute.For<ISpacePresenter>();
        spacePresenter.SpaceVm.Returns(space);

        var systemUnderTest =
            CreatePresenterForTesting(authoringToolWorkspaceVm, spacePresenter: spacePresenter);
        
        systemUnderTest.CallCreateElementWithPreloadedContentFromActiveView(content);

        Assert.That(systemUnderTest.InformationMessageToShow, Is.EqualTo("Elements can only get loaded into spaces."));
        Assert.Pass();
    }
    
    [Test]
    public void
        CallCreateElementWithPreloadedContentFromActiveView_SpaceSelected_CallsSpacePresenter()
    {
        var content = new ContentViewModel("n", "t", "");
        var world = new WorldViewModel("n", "sn", "a", "l", "d", "g");
        var space = new SpaceViewModel("n", "sn", "a", "d", "g");
        world.Spaces.Add(space);
        world.SelectedObject = space;
        world.ShowingSpaceView = true;
        var authoringToolWorkspaceVm = new AuthoringToolWorkspaceViewModel();
        authoringToolWorkspaceVm._worlds.Add(world);
        authoringToolWorkspaceVm.SelectedWorld = world;
        var spacePresenter = Substitute.For<ISpacePresenter>();
        spacePresenter.SpaceVm.Returns(space);

        var systemUnderTest =
            CreatePresenterForTesting(authoringToolWorkspaceVm, spacePresenter: spacePresenter);
        
        systemUnderTest.CallCreateElementWithPreloadedContentFromActiveView(content);
        
        spacePresenter.Received().CreateElementWithPreloadedContent(content);
    }
    
    [Test]
    public void
        CallCreateElementWithPreloadedContentFromActiveView_SpaceSelectedButSpaceVmIsNull_DoNothing()
    {
        var content = new ContentViewModel("n", "t", "");
        var world = new WorldViewModel("n", "sn", "a", "l", "d", "g");
        var space = new SpaceViewModel("n", "sn", "a", "d", "g");
        world.Spaces.Add(space);
        world.SelectedObject = space;
        world.ShowingSpaceView = true;
        var authoringToolWorkspaceVm = new AuthoringToolWorkspaceViewModel();
        authoringToolWorkspaceVm._worlds.Add(world);
        authoringToolWorkspaceVm.SelectedWorld = world;
        var spacePresenter = Substitute.For<ISpacePresenter>();
        //spacePresenter.SpaceVm.Returns(space);

        var systemUnderTest =
            CreatePresenterForTesting(authoringToolWorkspaceVm, spacePresenter: spacePresenter);
        
        systemUnderTest.CallCreateElementWithPreloadedContentFromActiveView(content);
        
        Assert.Pass();
    }

    [Test]
    public void LoadWorldFromFileStream_CallsPresentationLogic()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadWorldFromFileStream(stream);

        presentationLogic.Received().LoadWorldViewModel(authoringToolWorkspace, stream);
    }

    [Test]
    public void LoadSpaceFromFileStream_CallsPresentationLogic()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic);
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");
        authoringToolWorkspace.Worlds.Add(world);
        
        systemUnderTest.SetSelectedWorld(world);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadSpaceFromFileStream(stream);

        presentationLogic.Received().LoadSpaceViewModel(world, stream);
    }

    [Test]
    public void LoadSpaceFromFileStream_SelectedWorldIsNull_CorrectInformationMessageToShow()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        var stream = Substitute.For<Stream>();
        
        Assert.That(systemUnderTest.InformationMessageToShow, Is.Not.EqualTo("A world must be selected to import a space."));

        systemUnderTest.LoadSpaceFromFileStream(stream);
        
        Assert.That(systemUnderTest.InformationMessageToShow, Is.EqualTo("A world must be selected to import a space."));
    }

    [Test]
    public void LoadElementFromFileStream_SelectedWorldIsNull_CorrectInformationMessageToShow()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        var stream = Substitute.For<Stream>();
        
        Assert.That(systemUnderTest.InformationMessageToShow, Is.Not.EqualTo("A world must be selected to import a element."));

        systemUnderTest.LoadElementFromFileStream(stream, 0);
        
        Assert.That(systemUnderTest.InformationMessageToShow, Is.EqualTo("A world must be selected to import a element."));
    }
    
    [Test]
    public void LoadElementFromFileStream_ShowingSpaceViewIsFalse_CorrectInformationMessageToShow()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var world = new WorldViewModel("n", "sn", "a", "l", "d", "g")
        {
            ShowingSpaceView = false
        };
        authoringToolWorkspace.Worlds.Add(world);
        authoringToolWorkspace.SelectedWorld = world;
        var spacePresenter = Substitute.For<ISpacePresenter>();
        spacePresenter.SpaceVm.Returns((SpaceViewModel?) null);
        var stream = Substitute.For<Stream>();
        
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, spacePresenter: spacePresenter);
        
        systemUnderTest.LoadElementFromFileStream(stream, 0);

        Assert.That(systemUnderTest.InformationMessageToShow, Is.EqualTo("Elements can only get loaded into spaces."));
        Assert.Pass();
    }
    
    [Test]
    public void LoadElementFromFileStream_SpaceVmInSpacePresenterIsNull_ThrowsException()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var world = new WorldViewModel("n", "sn", "a", "l", "d", "g")
        {
            ShowingSpaceView = true
        };
        authoringToolWorkspace.Worlds.Add(world);
        authoringToolWorkspace.SelectedWorld = world;
        var spacePresenter = Substitute.For<ISpacePresenter>();
        spacePresenter.SpaceVm.Returns((SpaceViewModel?) null);
        var stream = Substitute.For<Stream>();
        
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, spacePresenter: spacePresenter);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.LoadElementFromFileStream(stream, 0));
        Assert.That(ex!.Message, Is.EqualTo("ShowingSpaceView for World 'n' is true, but SpaceVm in SpacePresenter is null"));
    }
    
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void LoadElementFromFileStream_CallsPresentationLogic(bool showingSpaceView)
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");
        var existingSpace = Substitute.For<ISpaceViewModel>();
        var spacePresenter = Substitute.For<ISpacePresenter>();
        spacePresenter.SpaceVm.Returns(existingSpace);
        
        world.ShowingSpaceView = showingSpaceView;
        
        authoringToolWorkspace.Worlds.Add(world);
        authoringToolWorkspace.SelectedWorld = world;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic,
            spacePresenter: spacePresenter);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadElementFromFileStream(stream, 0);
        
        if (showingSpaceView)
        {
            presentationLogic.Received().LoadElementViewModel(existingSpace, 0, stream);
        }
    }

    #endregion


    private AuthoringToolWorkspacePresenter CreatePresenterForTesting(
        IAuthoringToolWorkspaceViewModel? authoringToolWorkspaceVm = null, IPresentationLogic? presentationLogic = null,
        IWorldPresenter? worldPresenter = null, ISpacePresenter? spacePresenter = null,
        ILogger<AuthoringToolWorkspacePresenter>? logger = null, IShutdownManager? shutdownManager = null)
    {
        authoringToolWorkspaceVm ??= Substitute.For<IAuthoringToolWorkspaceViewModel>();
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        worldPresenter ??= Substitute.For<IWorldPresenter>();
        spacePresenter ??= Substitute.For<ISpacePresenter>();
        logger ??= Substitute.For<ILogger<AuthoringToolWorkspacePresenter>>();
        shutdownManager ??= Substitute.For<IShutdownManager>();
        return new AuthoringToolWorkspacePresenter(authoringToolWorkspaceVm, presentationLogic,
            spacePresenter, logger, shutdownManager);
    }

    private WorldPresenter CreateWorldPresenter(IPresentationLogic? presentationLogic = null,
        ISpacePresenter? spacePresenter = null, ILogger<WorldPresenter>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        spacePresenter ??= Substitute.For<ISpacePresenter>();
        logger ??= Substitute.For<ILogger<WorldPresenter>>();
        _authoringToolWorkspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        return new WorldPresenter(presentationLogic, spacePresenter, logger, _authoringToolWorkspaceViewModel);
    }
}