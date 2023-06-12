using System;
using System.Collections.Generic;
using System.Linq;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.DropZone;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace PresentationTest.PresentationLogic.DropZone;

[TestFixture]
public class LearningElementDropZoneHelperUt
{
    #region GetWorldAndSpaceElements

    [Test]
    public void GetWorldAndSpaceElements_WhenLearningSpaceVmAndLearningWorldVmNull_ReturnsEmptyList()
    {
        // Arrange
        var systemUnderTest = CreateDropZoneHelperForTesting();

        // Act
        var result = systemUnderTest.GetWorldAndSpaceElements();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetWorldAndSpaceElements_WhenLearningSpaceVmNotNull_ReturnsCombinedElements()
    {
        // Arrange
        var element1 = Substitute.For<ILearningElementViewModel>();
        var element2 = Substitute.For<ILearningElementViewModel>();
        var spaceElements = new List<ILearningElementViewModel> {element1, element2};

        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        var mockSpace = Substitute.For<ILearningSpaceViewModel>();
        mockSpace.ContainedLearningElements.Returns(spaceElements);
        spacePresenter.LearningSpaceVm.Returns(mockSpace);
        var systemUnderTest = CreateDropZoneHelperForTesting(spacePresenter: spacePresenter);

        // Act
        var result = systemUnderTest.GetWorldAndSpaceElements();

        // Assert
        Assert.That(result, Is.EqualTo(spaceElements));
    }

    [Test]
    public void GetWorldAndSpaceElements_WhenLearningWorldVmNotNull_ReturnsCombinedElements()
    {
        // Arrange
        var element1 = Substitute.For<ILearningElementViewModel>();
        var element2 = Substitute.For<ILearningElementViewModel>();
        var learningElements = new List<ILearningElementViewModel> {element1, element2};

        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        mockWorld.UnplacedLearningElements.Returns(learningElements);
        worldPresenter.LearningWorldVm.Returns(mockWorld);
        var systemUnderTest = CreateDropZoneHelperForTesting(worldPresenter: worldPresenter);

        // Act
        var result = systemUnderTest.GetWorldAndSpaceElements();

        // Assert
        Assert.That(result, Is.EqualTo(learningElements));
    }

    [Test]
    public void GetWorldAndSpaceElements_WhenSpaceVmAndLearningWorldVmNotNull_ReturnsCombinedElements()
    {
        // Arrange
        var element1 = Substitute.For<ILearningElementViewModel>();
        var element2 = Substitute.For<ILearningElementViewModel>();
        var element3 = Substitute.For<ILearningElementViewModel>();
        var element4 = Substitute.For<ILearningElementViewModel>();
        var spaceElements = new List<ILearningElementViewModel> {element1, element2};
        var worldElements = new List<ILearningElementViewModel> {element3, element4};

        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        var mockSpace = Substitute.For<ILearningSpaceViewModel>();
        mockSpace.ContainedLearningElements.Returns(spaceElements);
        spacePresenter.LearningSpaceVm.Returns(mockSpace);

        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        mockWorld.UnplacedLearningElements.Returns(worldElements);
        worldPresenter.LearningWorldVm.Returns(mockWorld);

        var systemUnderTest =
            CreateDropZoneHelperForTesting(spacePresenter: spacePresenter, worldPresenter: worldPresenter);

        // Act
        var result = systemUnderTest.GetWorldAndSpaceElements();

        // Assert
        Assert.That(result, Is.EqualTo(spaceElements.Concat(worldElements)));
    }

    #endregion

    #region ItemUpdated

    #region DropzoneIdentifierIsUnplaced

    [Test]
    public void ItemUpdated_DropzoneIdentifierIsUnplacedAndLearningSpaceVmIsNull_ThrowsException()
    {
        // Arrange
        var dropItem = new MudItemDropInfo<ILearningElementViewModel>(Substitute.For<ILearningElementViewModel>(),
            "unplacedElements", 0);
        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        spacePresenter.LearningSpaceVm.Returns((ILearningSpaceViewModel?) null);
        var systemUnderTest = CreateDropZoneHelperForTesting(spacePresenter: spacePresenter);

        // Act
        // Assert
        Assert.Throws(Is.TypeOf<ApplicationException>().And.Message.EqualTo("LearningSpaceVm is null"),
            () => systemUnderTest.ItemUpdated(dropItem));
    }

    [Test]
    public void ItemUpdated_DropzoneIdentifierIsUnplacedAndLearningWorldVmIsNull_ThrowsException()
    {
        // Arrange
        var dropItem = new MudItemDropInfo<ILearningElementViewModel>(Substitute.For<ILearningElementViewModel>(),
            "unplacedElements", 0);
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns((ILearningWorldViewModel?) null);
        var systemUnderTest = CreateDropZoneHelperForTesting(worldPresenter: worldPresenter);

        // Act
        // Assert
        Assert.Throws(Is.TypeOf<ApplicationException>().And.Message.EqualTo("LearningWorldVm is null"),
            () => systemUnderTest.ItemUpdated(dropItem));
    }

    [Test]
    public void ItemUpdated_DropzoneIdentifierIsUnplacedAndDropItemIsInLearningSpace_CallsPresentationLogic()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        var dropItem = new MudItemDropInfo<ILearningElementViewModel>(mockElement, "unplacedElements", 0);
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        var mockSpace = Substitute.For<ILearningSpaceViewModel>();
        mockSpace.ContainedLearningElements.Returns(new List<ILearningElementViewModel> {mockElement});
        mockElement.Parent.Returns(mockSpace);
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns(mockWorld);
        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        spacePresenter.LearningSpaceVm.Returns(mockSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreateDropZoneHelperForTesting(presentationLogic: presentationLogic,
            worldPresenter: worldPresenter, spacePresenter: spacePresenter);

        // Act
        systemUnderTest.ItemUpdated(dropItem);

        // Assert
        presentationLogic.Received().DragLearningElementToUnplaced(mockWorld, mockSpace, mockElement);
    }

    [Test]
    public void ItemUpdated_DropzoneIdentifierIsUnplacedAndDropItemIsInLearningWorld_DoNotCallPresentationLogic()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        mockElement.Parent.Returns((ILearningSpaceViewModel?) null);
        var dropItem = new MudItemDropInfo<ILearningElementViewModel>(mockElement, "unplacedElements", 0);
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        mockWorld.UnplacedLearningElements.Returns(new List<ILearningElementViewModel> {mockElement});
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns(mockWorld);
        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreateDropZoneHelperForTesting(presentationLogic: presentationLogic,
            worldPresenter: worldPresenter, spacePresenter: spacePresenter);

        // Act
        systemUnderTest.ItemUpdated(dropItem);

        // Assert
        presentationLogic.DidNotReceive().DragLearningElementToUnplaced(Arg.Any<LearningWorldViewModel>(),
            Arg.Any<LearningSpaceViewModel>(), Arg.Any<ILearningElementViewModel>());
    }

    [Test]
    public void ItemUpdated_DropzoneIdentifierIsUnplacedAndDropItemIsNotInLearningWorldOrSpace_ThrowsException()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        mockElement.Parent.Returns((ILearningSpaceViewModel?) null);
        var dropItem = new MudItemDropInfo<ILearningElementViewModel>(mockElement, "unplacedElements", 0);
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        mockWorld.UnplacedLearningElements.Returns(new List<ILearningElementViewModel>());
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns(mockWorld);
        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreateDropZoneHelperForTesting(presentationLogic: presentationLogic,
            worldPresenter: worldPresenter, spacePresenter: spacePresenter);

        // Act
        // Assert
        Assert.Throws(
            Is.TypeOf<ApplicationException>().And.Message
                .EqualTo("DragDropItem is neither in unplaced elements nor in a learning space"),
            () => systemUnderTest.ItemUpdated(dropItem));
    }

    #endregion

    #region DropzoneIdentifierIsNotUnplaced

    [Test]
    public void ItemUpdated_DropzoneIdentifierIsNotUnplacedButLearningWorldVmIsNull_ThrowsException()
    {
        // Arrange
        var spaceId = Guid.NewGuid();
        const int newSlotId = 2;
        var dropItem = new MudItemDropInfo<ILearningElementViewModel>(Substitute.For<ILearningElementViewModel>(),
            spaceId.ToString() + newSlotId, 0);
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns((ILearningWorldViewModel?) null);
        var systemUnderTest = CreateDropZoneHelperForTesting(worldPresenter: worldPresenter);

        // Act
        // Assert
        Assert.Throws(Is.TypeOf<ApplicationException>().And.Message.EqualTo("LearningWorldVm is null"),
            () => systemUnderTest.ItemUpdated(dropItem));
    }

    [Test]
    public void ItemUpdated_DropItemIsInSpaceButSpaceIsNotInWorld_ThrowsException()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        var mockSpace = Substitute.For<ILearningSpaceViewModel>();
        var spaceId = Guid.NewGuid();
        const int oldSlotId = 1;
        const int newSlotId = 2;
        mockSpace.Id.Returns(spaceId);
        mockSpace.ContainedLearningElements.Returns(new List<ILearningElementViewModel> {mockElement});
        mockSpace.LearningSpaceLayout.LearningElements
            .Returns(new Dictionary<int, ILearningElementViewModel> {{oldSlotId, mockElement}});
        mockElement.Parent.Returns(mockSpace);
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns(mockWorld);
        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        spacePresenter.LearningSpaceVm.Returns(mockSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var dropItem = new MudItemDropInfo<ILearningElementViewModel>(mockElement, spaceId.ToString() + newSlotId, 0);
        var systemUnderTest = CreateDropZoneHelperForTesting(presentationLogic: presentationLogic,
            worldPresenter: worldPresenter, spacePresenter: spacePresenter);

        // Act
        // Assert
        Assert.Throws(
            Is.TypeOf<ApplicationException>().And.Message
                .EqualTo("The space to drop to is not in the world"),
            () => systemUnderTest.ItemUpdated(dropItem));
    }

    [Test]
    public void ItemUpdated_DropItemIsInSpaceAndSpaceIsInWorldButSpaceIsNotSelected_ThrowsException()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        var mockSpace = Substitute.For<ILearningSpaceViewModel>();
        var spaceId = Guid.NewGuid();
        const int oldSlotId = 1;
        const int newSlotId = 2;
        mockSpace.Id.Returns(spaceId);
        mockSpace.ContainedLearningElements.Returns(new List<ILearningElementViewModel> {mockElement});
        mockSpace.LearningSpaceLayout.LearningElements
            .Returns(new Dictionary<int, ILearningElementViewModel> {{oldSlotId, mockElement}});
        mockElement.Parent.Returns(mockSpace);
        mockWorld.LearningSpaces.Returns(new List<ILearningSpaceViewModel> {mockSpace});
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns(mockWorld);
        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        spacePresenter.LearningSpaceVm.Returns(Substitute.For<ILearningSpaceViewModel>());
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var dropItem = new MudItemDropInfo<ILearningElementViewModel>(mockElement, spaceId.ToString() + newSlotId, 0);
        var systemUnderTest = CreateDropZoneHelperForTesting(presentationLogic: presentationLogic,
            worldPresenter: worldPresenter, spacePresenter: spacePresenter);

        // Act
        // Assert
        Assert.Throws(
            Is.TypeOf<ApplicationException>().And.Message
                .EqualTo("The space to drop to is not the currently selected space"),
            () => systemUnderTest.ItemUpdated(dropItem));
    }

    [Test]
    public void ItemUpdated_DropItemIsInSpaceAndSpaceIsInWorldAndSpaceIsSelected_CallsPresentationLogic()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        var mockSpace = Substitute.For<ILearningSpaceViewModel>();
        var spaceId = Guid.NewGuid();
        const int oldSlotId = 1;
        const int newSlotId = 2;
        mockSpace.Id.Returns(spaceId);
        mockSpace.ContainedLearningElements.Returns(new List<ILearningElementViewModel> {mockElement});
        mockSpace.LearningSpaceLayout.LearningElements
            .Returns(new Dictionary<int, ILearningElementViewModel> {{oldSlotId, mockElement}});
        mockElement.Parent.Returns(mockSpace);
        mockWorld.LearningSpaces.Returns(new List<ILearningSpaceViewModel> {mockSpace});
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns(mockWorld);
        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        spacePresenter.LearningSpaceVm.Returns(mockSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var dropItem = new MudItemDropInfo<ILearningElementViewModel>(mockElement, spaceId.ToString() + newSlotId, 0);
        var systemUnderTest = CreateDropZoneHelperForTesting(presentationLogic: presentationLogic,
            worldPresenter: worldPresenter, spacePresenter: spacePresenter);

        // Act
        systemUnderTest.ItemUpdated(dropItem);

        // Assert
        presentationLogic.Received().SwitchLearningElementSlot(mockSpace, mockElement, newSlotId);
    }

    [Test]
    public void ItemUpdate_DropItemIsInWorldButUnplacedLearningElementsDoesNotContainIt_ThrowsException()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        var mockSpace = Substitute.For<ILearningSpaceViewModel>();
        var spaceId = Guid.NewGuid();
        const int newSlotId = 2;
        mockSpace.Id.Returns(spaceId);
        mockSpace.ContainedLearningElements.Returns(new List<ILearningElementViewModel>());
        mockSpace.LearningSpaceLayout.LearningElements
            .Returns(new Dictionary<int, ILearningElementViewModel>());
        mockElement.Parent.Returns((ILearningSpaceViewModel?) null);
        mockWorld.LearningSpaces.Returns(new List<ILearningSpaceViewModel> {mockSpace});
        mockWorld.UnplacedLearningElements.Returns(new List<ILearningElementViewModel>());
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns(mockWorld);
        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        spacePresenter.LearningSpaceVm.Returns(mockSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var dropItem = new MudItemDropInfo<ILearningElementViewModel>(mockElement, spaceId.ToString() + newSlotId, 0);
        var systemUnderTest = CreateDropZoneHelperForTesting(presentationLogic: presentationLogic,
            worldPresenter: worldPresenter, spacePresenter: spacePresenter);

        // Act
        // Assert
        Assert.Throws(
            Is.TypeOf<ApplicationException>().And.Message
                .EqualTo("DragDropItem should be in unplaced elements"),
            () => systemUnderTest.ItemUpdated(dropItem));
    }

    [Test]
    public void ItemUpdate_DropItemIsInWorldButThereIsAlreadyAnElementOnTheDraggedSlot_CallsSpacePresenter()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        var mockExistingElement = Substitute.For<ILearningElementViewModel>();
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        var mockSpace = Substitute.For<ILearningSpaceViewModel>();
        var spaceId = Guid.NewGuid();
        const int newSlotId = 2;
        mockSpace.Id.Returns(spaceId);
        mockSpace.ContainedLearningElements.Returns(new List<ILearningElementViewModel> {mockExistingElement});
        mockSpace.LearningSpaceLayout.LearningElements
            .Returns(new Dictionary<int, ILearningElementViewModel> {{newSlotId, mockExistingElement}});
        mockElement.Parent.Returns((ILearningSpaceViewModel?) null);
        mockWorld.LearningSpaces.Returns(new List<ILearningSpaceViewModel> {mockSpace});
        mockWorld.UnplacedLearningElements.Returns(new List<ILearningElementViewModel> {mockElement});
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns(mockWorld);
        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        spacePresenter.LearningSpaceVm.Returns(mockSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var dropItem = new MudItemDropInfo<ILearningElementViewModel>(mockElement, spaceId.ToString() + newSlotId, 0);
        var systemUnderTest = CreateDropZoneHelperForTesting(presentationLogic: presentationLogic,
            worldPresenter: worldPresenter, spacePresenter: spacePresenter);

        // Act
        systemUnderTest.ItemUpdated(dropItem);

        // Assert
        spacePresenter.Received().OpenReplaceLearningElementDialog(mockWorld, mockElement, newSlotId);
    }

    [Test]
    public void ItemUpdate_DropItemIsInWorldAndTheDraggedSlotIsFree_CallsPresentationLogic()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        var mockSpace = Substitute.For<ILearningSpaceViewModel>();
        var spaceId = Guid.NewGuid();
        const int newSlotId = 2;
        mockSpace.Id.Returns(spaceId);
        mockSpace.ContainedLearningElements.Returns(new List<ILearningElementViewModel>());
        mockSpace.LearningSpaceLayout.LearningElements
            .Returns(new Dictionary<int, ILearningElementViewModel>());
        mockElement.Parent.Returns((ILearningSpaceViewModel?) null);
        mockWorld.LearningSpaces.Returns(new List<ILearningSpaceViewModel> {mockSpace});
        mockWorld.UnplacedLearningElements.Returns(new List<ILearningElementViewModel> {mockElement});
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns(mockWorld);
        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        spacePresenter.LearningSpaceVm.Returns(mockSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var dropItem = new MudItemDropInfo<ILearningElementViewModel>(mockElement, spaceId.ToString() + newSlotId, 0);
        var systemUnderTest = CreateDropZoneHelperForTesting(presentationLogic: presentationLogic,
            worldPresenter: worldPresenter, spacePresenter: spacePresenter);

        // Act
        systemUnderTest.ItemUpdated(dropItem);

        // Assert
        presentationLogic.Received().DragLearningElementFromUnplaced(mockWorld, mockSpace, mockElement, newSlotId);
    }

    #endregion

    #endregion

    #region IsItemInDropZone

    [Test]
    public void IsItemInDropZone_DropItemParentIsNotNullAndDropItemIsInCorrectSlot_ReturnsTrue()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        var mockSpace = Substitute.For<ILearningSpaceViewModel>();
        var spaceId = Guid.NewGuid();
        const int newSlotId = 2;
        mockSpace.Id.Returns(spaceId);
        mockSpace.ContainedLearningElements.Returns(new List<ILearningElementViewModel>());
        mockSpace.LearningSpaceLayout.LearningElements
            .Returns(new Dictionary<int, ILearningElementViewModel> {{newSlotId, mockElement}});
        mockElement.Parent.Returns(mockSpace);
        var systemUnderTest = CreateDropZoneHelperForTesting();

        // Act
        var result = systemUnderTest.IsItemInDropZone(mockElement, spaceId.ToString() + newSlotId);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsItemInDropZone_DropItemParentIsNotNullButDropItemIsInAnotherSlot_ReturnsFalse()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        var mockSpace = Substitute.For<ILearningSpaceViewModel>();
        var spaceId = Guid.NewGuid();
        const int elementSlotId = 1;
        const int newSlotId = 2;
        mockSpace.Id.Returns(spaceId);
        mockSpace.ContainedLearningElements.Returns(new List<ILearningElementViewModel>());
        mockSpace.LearningSpaceLayout.LearningElements
            .Returns(new Dictionary<int, ILearningElementViewModel> {{elementSlotId, mockElement}});
        mockElement.Parent.Returns(mockSpace);
        var systemUnderTest = CreateDropZoneHelperForTesting();

        // Act
        var result = systemUnderTest.IsItemInDropZone(mockElement, spaceId.ToString() + newSlotId);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsItemInDropZone_DropItemParentIsNotNullButDropzoneIdentifierIsNotThisSpace_ReturnsFalse()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        var mockSpace = Substitute.For<ILearningSpaceViewModel>();
        var spaceId = Guid.NewGuid();
        const int elementSlotId = 1;
        const int newSlotId = 2;
        mockSpace.Id.Returns(spaceId);
        mockSpace.ContainedLearningElements.Returns(new List<ILearningElementViewModel>());
        mockSpace.LearningSpaceLayout.LearningElements
            .Returns(new Dictionary<int, ILearningElementViewModel> {{elementSlotId, mockElement}});
        mockElement.Parent.Returns(mockSpace);
        var systemUnderTest = CreateDropZoneHelperForTesting();

        // Act
        var result = systemUnderTest.IsItemInDropZone(mockElement, Guid.NewGuid().ToString() + newSlotId);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsItemInDropZone_DropItemParentIsNullButDropzoneIdentifierIsNotUnplacedElements_ReturnsFalse()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        const int newSlotId = 2;
        mockElement.Parent.Returns((ILearningSpaceViewModel?) null);
        var systemUnderTest = CreateDropZoneHelperForTesting();

        // Act
        var result = systemUnderTest.IsItemInDropZone(mockElement, Guid.NewGuid().ToString() + newSlotId);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void
        IsItemInDropZone_DropItemParentIsNullAndDropzoneIdentifierIsUnplacedElementsButLearningWorldVmIsNull_ReturnsFalse()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        mockElement.Parent.Returns((ILearningSpaceViewModel?) null);
        var systemUnderTest = CreateDropZoneHelperForTesting();

        // Act
        var result = systemUnderTest.IsItemInDropZone(mockElement, "unplacedElements");

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void
        IsItemInDropZone_DropItemParentIsNullAndDropzoneIdentifierIsUnplacedElementsButDropItemIsNotInUnplacedElements_ReturnsFalse()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        mockElement.Parent.Returns((ILearningSpaceViewModel?) null);
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        mockWorld.UnplacedLearningElements.Returns(new List<ILearningElementViewModel>());
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns(mockWorld);
        var systemUnderTest = CreateDropZoneHelperForTesting(worldPresenter: worldPresenter);

        // Act
        var result = systemUnderTest.IsItemInDropZone(mockElement, "unplacedElements");

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void
        IsItemInDropZone_DropItemParentIsNullAndDropzoneIdentifierIsUnplacedElementsAndDropItemIsInUnplacedElements_ReturnsTrue()
    {
        // Arrange
        var mockElement = Substitute.For<ILearningElementViewModel>();
        mockElement.Parent.Returns((ILearningSpaceViewModel?) null);
        var mockWorld = Substitute.For<ILearningWorldViewModel>();
        mockWorld.UnplacedLearningElements.Returns(new List<ILearningElementViewModel> {mockElement});
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.LearningWorldVm.Returns(mockWorld);
        var systemUnderTest = CreateDropZoneHelperForTesting(worldPresenter: worldPresenter);

        // Act
        var result = systemUnderTest.IsItemInDropZone(mockElement, "unplacedElements");

        // Assert
        Assert.That(result, Is.True);
    }

    #endregion


    private LearningElementDropZoneHelper CreateDropZoneHelperForTesting(IPresentationLogic? presentationLogic = null,
        ILearningWorldPresenter? worldPresenter = null, ILearningSpacePresenter? spacePresenter = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        worldPresenter ??= Substitute.For<ILearningWorldPresenter>();
        spacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        return new LearningElementDropZoneHelper(presentationLogic, worldPresenter, spacePresenter);
    }
}