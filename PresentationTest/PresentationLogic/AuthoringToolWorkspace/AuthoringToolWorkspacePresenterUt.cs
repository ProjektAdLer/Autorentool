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
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Shared;

namespace PresentationTest.PresentationLogic.AuthoringToolWorkspace;

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
            Assert.That(systemUnderTest.CreateLearningSpaceDialogOpen, Is.EqualTo(false));
            Assert.That(systemUnderTest.CreateLearningWorldDialogOpen, Is.EqualTo(false));
            Assert.That(systemUnderTest.EditLearningSpaceDialogOpen, Is.EqualTo(false));
            Assert.That(systemUnderTest.EditLearningWorldDialogOpen, Is.EqualTo(false));
        });
    }
    
    #region CreateNewLearningWorld

    [Test]
    public void AddNewLearningWorld_SetsFieldToTrue()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        Assert.That(!systemUnderTest.CreateLearningWorldDialogOpen);
        
        systemUnderTest.AddNewLearningWorld();
        
        Assert.That(systemUnderTest.CreateLearningWorldDialogOpen);
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
            .CreateLearningWorld(authoringToolWorkspaceVm, "n", "sn", "a", "a", "d", "g");
    }

    [Test]
    public void OnCreateWorldDialogClose_EventHandlerCalled()
    {
        var learningWorldVm = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
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
        LearningWorldViewModel? callbackCreateArg = null;
        EventHandler<LearningWorldViewModel?> callbackCreate =
            (_, args) =>
            {
                callbackCreateCalled = true;
                if (args != null)
                {
                    callbackCreateArg = args;
                }
            };
        var callbackSelectCalled = false;
        LearningWorldViewModel? callbackSelectArg = null;
        EventHandler<LearningWorldViewModel?> callbackSelect =
            (_, args) =>
            {
                callbackSelectCalled = true;
                if (args != null)
                {
                    callbackSelectArg = args;
                }
            };

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.AddLearningWorld(learningWorldVm);
        systemUnderTest.SetSelectedLearningWorld(learningWorldVm);
        systemUnderTest.OnLearningWorldCreate += callbackCreate;
        systemUnderTest.OnLearningWorldSelect += callbackSelect;

        systemUnderTest.OnCreateWorldDialogClose(returnValueTuple);
        
        Assert.Multiple(() =>
        {
            Assert.That(callbackCreateCalled, Is.True);
            Assert.That(callbackSelectCalled, Is.True);
            Assert.That(callbackCreateArg, Is.EqualTo(learningWorldVm));
            Assert.That(callbackSelectArg, Is.EqualTo(learningWorldVm));
        });
    }

    #endregion

    #region ChangeSelectedLearningWorld

    [Test]
    public void ChangeSelectedLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        workspaceVm._learningWorlds.Add(new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo", "Foo"));
        workspaceVm._learningWorlds.Add(new LearningWorldViewModel("tetete", "f", "bar", "de", "A test", "testing"));
        var firstCallbackCalled = false;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);

        EventHandler<LearningWorldViewModel?> firstCallback = (_, world) =>
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

        systemUnderTest.OnLearningWorldSelect += firstCallback;

        systemUnderTest.SetSelectedLearningWorld("tetete");

        Assert.That(firstCallbackCalled, Is.EqualTo(true));
        firstCallbackCalled = false;
        systemUnderTest.OnLearningWorldSelect -= firstCallback;

        var secondCallbackCalled = false;
        systemUnderTest.OnLearningWorldSelect += (_, world) =>
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
        workspaceVm._learningWorlds.Add(world1);
        workspaceVm._learningWorlds.Add(world2);
        

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

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter);

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
        workspaceVm._learningWorlds.Add(world1);
        workspaceVm._learningWorlds.Add(world2);
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

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter);

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
    public void DeleteSelectedLearningWorld_CallsPresentationLogic()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        workspaceVm._learningWorlds.Add(world1);
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic);

        systemUnderTest.SetSelectedLearningWorld(world1.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        
        presentationLogic.Received().DeleteLearningWorld(workspaceVm, world1);
    }

    [Test]
    public void DeleteSelectedLearningWorld_SetsDeletedUnsavedWorld()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo")
        {
            UnsavedChanges = true
        };
        workspaceVm._learningWorlds.Add(world);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        systemUnderTest.SetSelectedLearningWorld(world);

        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.That(systemUnderTest.DeletedUnsavedWorld, Is.EqualTo(world));
    }

    [Test]
    public void DeleteSelectedLearningWorld_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm._learningWorlds.Add(world1);
        workspaceVm._learningWorlds.Add(world2);

        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic
            .When(x => x.DeleteLearningWorld(Arg.Any<IAuthoringToolWorkspaceViewModel>(),
                Arg.Any<LearningWorldViewModel>()))
            .Do(y =>
            {
                workspaceVm.RemoveLearningWorld(((AuthoringToolWorkspaceViewModel) y.Args()[0]).SelectedLearningWorld!);
                ((AuthoringToolWorkspaceViewModel) y.Args()[0]).SelectedLearningWorld =
                    ((AuthoringToolWorkspaceViewModel) y.Args()[0]).LearningWorlds.LastOrDefault();
            });

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic);

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
        workspaceVm._learningWorlds.Add(world);
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
        workspaceVm._learningWorlds.Add(world);
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
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSelectedLearningWorld(world);
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditWorldDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnEditWorldDialogClose_SelectedLearningWorldIsNull_ThrowsException()
    {
        new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
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
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void OnEditWorldDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
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

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSelectedLearningWorld(world);

        systemUnderTest.OnEditWorldDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningWorld(world, "n", "sn", "a", "a", "d", "g");
    }

    #endregion

    #region Save(Selected)LearningWorldAsync

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
        systemUnderTest.UnsavedWorldsQueue = new Queue<LearningWorldViewModel>();
        
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
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel._learningWorlds.Add(learningWorld);
        
        var modalDialogReturnValue = ModalDialogReturnValue.Yes;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(viewModel);

        systemUnderTest.UnsavedWorldsQueue = new Queue<LearningWorldViewModel>(viewModel.LearningWorlds);

        Assert.That(systemUnderTest.UnsavedWorldsQueue, Contains.Item(learningWorld));
        
        systemUnderTest.OnSaveWorldDialogClose(returnValueTuple);

        Assert.That(systemUnderTest.UnsavedWorldsQueue, Is.Null);
    }
    
    [Test]
    public void OnSaveWorldDialogClose_ModalDialogNo_DequeueWorldAndDeletesUnsavedChanges()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel._learningWorlds.Add(learningWorld);
        
        var modalDialogReturnValue = ModalDialogReturnValue.No;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(viewModel);

        systemUnderTest.UnsavedWorldsQueue = new Queue<LearningWorldViewModel>(viewModel.LearningWorlds);

        Assert.That(systemUnderTest.UnsavedWorldsQueue, Contains.Item(learningWorld));
        Assert.That(learningWorld.UnsavedChanges);
        
        systemUnderTest.OnSaveWorldDialogClose(returnValueTuple);

        Assert.That(systemUnderTest.UnsavedWorldsQueue, Is.Null);
        Assert.That(!learningWorld.UnsavedChanges);
    }
    
    [Test]
    public void OnSaveWorldDialogClose_SwitchDefaultValue_ThrowsException()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var modalDialogReturnValue = (ModalDialogReturnValue)10;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(viewModel);

        systemUnderTest.UnsavedWorldsQueue = new Queue<LearningWorldViewModel>(viewModel.LearningWorlds);

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
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel._learningWorlds.Add(learningWorld);
    
        var modalDialogReturnValue = ModalDialogReturnValue.Yes;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic : presentationLogic);
        systemUnderTest.DeletedUnsavedWorld = learningWorld;
    
        systemUnderTest.OnSaveDeletedWorldDialogClose(returnValueTuple);

        presentationLogic.Received().SaveLearningWorldAsync(learningWorld);
    }
    
    [Test]
    public void OnSaveDeletedWorldDialogClose_ModalDialogNo()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel._learningWorlds.Add(learningWorld);
        
        var modalDialogReturnValue = ModalDialogReturnValue.No;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(viewModel);
        systemUnderTest.DeletedUnsavedWorld = learningWorld;
        
        systemUnderTest.OnSaveDeletedWorldDialogClose(returnValueTuple);

        Assert.That(systemUnderTest.DeletedUnsavedWorld, Is.EqualTo(null));
    }
    
    [Test]
    public void  OnSaveDeletedWorldDialogClose_SwitchDefaultValue_ThrowsException()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel._learningWorlds.Add(learningWorld);
        
        var modalDialogReturnValue = (ModalDialogReturnValue)10;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(viewModel);
        systemUnderTest.DeletedUnsavedWorld = learningWorld;

        Assert.Throws<ArgumentOutOfRangeException>(() => systemUnderTest.OnSaveDeletedWorldDialogClose(returnValueTuple));
    }

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
        viewModel._learningWorlds.Add(learningWorld);
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
        presentationLogic
            .When(x => x.LoadLearningWorldAsync(Arg.Any<IAuthoringToolWorkspaceViewModel>()))
            .Do(y =>
            {
                ((AuthoringToolWorkspaceViewModel)y.Args()[0])._learningWorlds.Add(learningWorld);
                ((AuthoringToolWorkspaceViewModel)y.Args()[0]).SelectedLearningWorld = learningWorld;
            });
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic);

        await systemUnderTest.LoadLearningWorldAsync();
        await presentationLogic.Received().LoadLearningWorldAsync(viewModel);
        Assert.That(viewModel.LearningWorlds, Contains.Item(learningWorld));
    }

    #endregion
    
    #region Shutdown

    [Test]
    public void OnBeforeShutdown_CancelsShutdownCreatesQueueAndInvokesViewUpdate()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel._learningWorlds.Add(learningWorld);
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
     TestCase("txt"), TestCase("c"), TestCase("h"), TestCase("cpp"), 
     TestCase("cc"), TestCase("c++"), TestCase("py"), TestCase("cs"), 
     TestCase("js"), TestCase("php"), TestCase("html"), TestCase("css"), 
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
                    authoringToolWorkspace._learningWorlds.Add(learningWorld);
                    authoringToolWorkspace.SelectedLearningWorld = learningWorld;
        }
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningContent = new LearningContentViewModel(fileName, ending, Array.Empty<byte>());
        presentationLogic.LoadLearningContentViewModel(Arg.Any<string>(), Arg.Any<Stream>())
            .Returns(learningContent);
        var learningWorldPresenter = Substitute.For<ILearningWorldPresenter>();
        var logger = Substitute.For<ILogger<AuthoringToolWorkspacePresenter>>();
        var systemUnderTest = 
            CreatePresenterForTesting(authoringToolWorkspace, presentationLogic, learningWorldPresenter, logger: logger);

        systemUnderTest.ProcessDragAndDropResult(resultTuple);

        switch (ending)
        {
            case "awf":
                presentationLogic.Received().LoadLearningWorldViewModel(authoringToolWorkspace, stream);
                break;
            case "asf":
                presentationLogic.Received().LoadLearningSpaceViewModel(stream);
                break;
            case "aef":
                presentationLogic.Received().LoadLearningElementViewModel(stream);
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
                presentationLogic.Received().LoadLearningContentViewModel(fileName, stream);
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
        authoringToolWorkspaceVm._learningWorlds.Add(learningWorld);
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
        authoringToolWorkspaceVm._learningWorlds.Add(learningWorld);
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
        authoringToolWorkspaceVm._learningWorlds.Add(learningWorld);
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
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningWorldFromFileStream(stream);

        presentationLogic.Received().LoadLearningWorldViewModel(authoringToolWorkspace, stream);
    }

    [Test]
    public void LoadLearningWorldFromFileStream_CallsEventHandler()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var newLearningWorld = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
        presentationLogic
            .When(x => x.LoadLearningWorldViewModel(Arg.Any<IAuthoringToolWorkspaceViewModel>(), Arg.Any<Stream>()))
            .Do(y =>
            {
                y.Arg<IAuthoringToolWorkspaceViewModel>().LearningWorlds.Add(newLearningWorld);
                y.Arg<IAuthoringToolWorkspaceViewModel>().SelectedLearningWorld = newLearningWorld;
            });
        
        var stream = Substitute.For<Stream>();
        var callbackCalled = false;
        LearningWorldViewModel? callbackSelectedWorld = null;
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic);
        EventHandler<LearningWorldViewModel?> callback = delegate(object? _, LearningWorldViewModel? model)
        {
            callbackCalled = true;
            callbackSelectedWorld = model;
        };
        systemUnderTest.OnLearningWorldSelect += callback;

        systemUnderTest.LoadLearningWorldFromFileStream(stream);
        
        Assert.Multiple(() =>
        {
            Assert.That(callbackCalled, Is.True);
            Assert.That(callbackSelectedWorld, Is.EqualTo(newLearningWorld));
        });
    }

    [Test]
    public void LoadLearningSpaceFromFileStream_CallsPresentationLogic()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.LoadLearningSpaceViewModel(Arg.Any<Stream>())
            .Returns(new LearningSpaceViewModel("n", "sn", "a", "d", "g"));
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningSpaceFromFileStream(stream);

        presentationLogic.Received().LoadLearningSpaceViewModel(stream);
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
        authoringToolWorkspace._learningWorlds.Add(existingLearningWorld);
        if (isLearningWorldSelected)
        {
            authoringToolWorkspace.SelectedLearningWorld = existingLearningWorld;
        }

        var presentationLogic = Substitute.For<IPresentationLogic>();
        var newLearningSpace = new LearningSpaceViewModel("n", "sn", "a", "d", "g");
        presentationLogic.LoadLearningSpaceViewModel(Arg.Any<Stream>())
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
        var newLearningElement = new LearningElementViewModel("n", "sn",
            new LearningContentViewModel("n", "t", Array.Empty<byte>()), "a", "d", "g",LearningElementDifficultyEnum.Easy);
        presentationLogic.LoadLearningElementViewModel(Arg.Any<Stream>())
            .Returns(newLearningElement);
        var systemUnderTest = CreatePresenterForTesting(authoringToolWorkspace, presentationLogic);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningElementFromFileStream(stream);

        presentationLogic.Received().LoadLearningElementViewModel(stream);
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
        authoringToolWorkspace._learningWorlds.Add(existingLearningWorld);
        authoringToolWorkspace.SelectedLearningWorld = existingLearningWorld;

        existingLearningWorld.ShowingLearningSpaceView = showingLearningSpaceView;

        var presentationLogic = Substitute.For<IPresentationLogic>();
        var newLearningElement = new LearningElementViewModel("n", "sn",
            new LearningContentViewModel("n", "t", Array.Empty<byte>()), "a", "d", "g",LearningElementDifficultyEnum.Easy);
        presentationLogic.LoadLearningElementViewModel(Arg.Any<Stream>()).Returns(newLearningElement);
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
        ILogger<AuthoringToolWorkspacePresenter>? logger = null, IShutdownManager? shutdownManager = null)
    {
        authoringToolWorkspaceVm ??= Substitute.For<IAuthoringToolWorkspaceViewModel>();
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningWorldPresenter ??= Substitute.For<ILearningWorldPresenter>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        logger ??= Substitute.For<ILogger<AuthoringToolWorkspacePresenter>>();
        shutdownManager ??= Substitute.For<IShutdownManager>();
        return new AuthoringToolWorkspacePresenter(authoringToolWorkspaceVm, presentationLogic, learningWorldPresenter,
            learningSpacePresenter, logger, shutdownManager);
    }

    private LearningWorldPresenter CreateLearningWorldPresenter(IPresentationLogic? presentationLogic = null,
        ILearningSpacePresenter? learningSpacePresenter = null, ILogger<LearningWorldPresenter>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        logger ??= Substitute.For<ILogger<LearningWorldPresenter>>();
        return new LearningWorldPresenter(presentationLogic, learningSpacePresenter, logger);
    }
}