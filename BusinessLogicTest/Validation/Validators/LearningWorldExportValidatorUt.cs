using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Validation.Validators;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using Shared;
using TestHelpers;
namespace BusinessLogicTest.Validation.Validators.CustomValidators
{
    [TestFixture]
    public class LearningWorldExportValidatorUt
    {
        private LearningWorldExportValidator _validator;
        private IStringLocalizer<LearningWorldExportValidator> _stringLocalizer;
        [SetUp]
        public void Setup()
        {
            _stringLocalizer = Substitute.For<IStringLocalizer<LearningWorldExportValidator>>();
            _validator= new LearningWorldExportValidator(_stringLocalizer);
            _stringLocalizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
            _stringLocalizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(ci =>
                new LocalizedString(ci.Arg<string>(), FormatStringLocalizerValue(ci)));
        }
        private static string FormatStringLocalizerValue(CallInfo ci)
        {
            return ci.Arg<string>() + " " + string.Join(" ", ci.Arg<object[]>().Select(obj => obj.ToString()));
        }

        [Test]
        public void Validate_WorldHasNoLearningSpaces_ErrorMessageOutput()
        {
            var world = EntityProvider.GetLearningWorld();

            var result = _validator.Validate(world);

            Assert.IsFalse(result.IsValid);
            var errorMessageFound = result.Errors.Any(error =>
                error.ErrorMessage.Equals($"<li> {_stringLocalizer["ErrorString.Missing.LearningSpace.Message"]} </li>"));
            Assert.IsTrue(errorMessageFound);
        }

        [Test]
        public void Validate_WorldHasLearningSpacesWithoutLearningElements_ErrorMessageOutput()
        {
            var world = EntityProvider.GetLearningWorldWithSpace();
            var space = world.LearningSpaces.First();
            
             var result = _validator.Validate(world);
            
             Assert.IsFalse(result.IsValid);
             var errorMessageFound = result.Errors.Any(error => error.ErrorMessage.Equals(
                 $"<li> {_stringLocalizer["ErrorString.Missing.LearningElements.Message", space.Name]} </li>"));
             Assert.IsTrue(errorMessageFound);

        }

        [Test]
        public void Validate_WorldHasAdaptivityContentWithNoTasks_ErrorMessageOutput()
        {
            var world = EntityProvider.GetLearningWorldWithSpaceWithElement();
            var space = world.LearningSpaces.First();
            var element = space.LearningSpaceLayout.LearningElements.First().Value;
            var adaptivityContent =EntityProvider.GetAdaptivityContent();
            element.LearningContent = adaptivityContent;
            adaptivityContent.Tasks.Clear();

            var result = _validator.Validate(world);
            
            Assert.IsFalse(result.IsValid);
            var errorMessageFound = result.Errors.Any(error => error.ErrorMessage.Equals(
                $"<li> {_stringLocalizer["ErrorString.NoTasks.Message", element.Name]} </li>"));
            Assert.IsTrue(errorMessageFound);
        }   

        [Test]
        public void Validate_LearningSpaceHasInsufficientPoints_ErrorMessageOutput()
        {
            var world = new LearningWorld("a", "f", "d", "e", "f", "d", "h");
            var space = new LearningSpace("a", "f", "d", 2, Theme.Campus);
            var element1 = new LearningElement("a", null!, "s", "e", LearningElementDifficultyEnum.Easy,
                ElementModel.l_h5p_blackboard_1, points: 1);
            space.LearningSpaceLayout.LearningElements.Add(0, element1);
            world.LearningSpaces.Add(space);

            var result = _validator.Validate(world);
            
            Assert.IsFalse(result.IsValid);
            var errorMessageFound = result.Errors.Any(error => error.ErrorMessage.Equals(
                $"<li> {_stringLocalizer["ErrorString.Insufficient.Points.Message", space.Name]} </li>"));
            Assert.IsTrue(errorMessageFound);
        }

        [Test]
        public void Validate_AdaptivityContentReferencesNonExistentElement_ErrorMessageOutput()
        {
            var world = EntityProvider.GetLearningWorldWithSpaceWithElement();
            var space = world.LearningSpaces.First();
            var element = space.LearningSpaceLayout.LearningElements.First().Value;
            var adaptivityContent = EntityProvider.GetAdaptivityContent();
            adaptivityContent.Tasks.First().Questions.First().Rules.First().Action =
                new ElementReferenceAction(Guid.NewGuid(), "foobar");
            element.LearningContent = adaptivityContent;

            var result = _validator.Validate(world);
            
            Assert.IsFalse(result.IsValid);
            var errorMessageFound = result.Errors.Any(error => error.ErrorMessage.Equals(
                $"<li> {_stringLocalizer["ErrorString.TaskReferencesNonexistantElement.Message", element.Name]} </li>"));
            Assert.IsTrue(errorMessageFound);
        }

        [Test]
        public void Validate_AdaptivityContentReferencesUnplacedElement_ErrorMessageOutput()
        {
            var world = EntityProvider.GetLearningWorldWithSpaceWithElement();
            var space = world.LearningSpaces.First();
            var element = space.LearningSpaceLayout.LearningElements.First().Value;
            var adaptivityContent = EntityProvider.GetAdaptivityContent();
            var unplacedElement = EntityProvider.GetLearningElement();
            world.UnplacedLearningElements.Add(unplacedElement);
            adaptivityContent.Tasks.First().Questions.First().Rules.First().Action =
                new ElementReferenceAction(unplacedElement.Id, "foobar");
            element.LearningContent = adaptivityContent;

            var result = _validator.Validate(world);
            
            Assert.IsFalse(result.IsValid);
            var errorMessageFound=result.Errors.Any(error => error.ErrorMessage.Equals(
                $"<li> {_stringLocalizer["ErrorString.TaskReferencesUnplacedElement.Message", element.Name]} </li>"));
            Assert.IsTrue(errorMessageFound);
        }

        [Test]
        public void Validate_AdaptivityContentReferencesElementInSpaceAfterOwnSpace_ErrorMessageOutput()
        {
            var world = EntityProvider.GetLearningWorldWithSpaceWithElement();
            var space = world.LearningSpaces.First();
            var element = space.LearningSpaceLayout.LearningElements.First().Value;
            var adaptivityContent = EntityProvider.GetAdaptivityContent();
            var laterElement = EntityProvider.GetLearningElement();
            laterElement.Points = 777;
            var laterSpace = EntityProvider.GetLearningSpace();
            laterSpace.LearningSpaceLayout.LearningElements.Add(0, laterElement);
            space.OutBoundObjects.Add(laterSpace);
            laterSpace.InBoundObjects.Add(space);
            world.LearningSpaces.Add(laterSpace);
            adaptivityContent.Tasks.First().Questions.First().Rules.First().Action =
                new ElementReferenceAction(laterElement.Id, "foobar");
            element.LearningContent = adaptivityContent;

            var result = _validator.Validate(world);
            
            Assert.IsFalse(result.IsValid);
            var errorMessageFound=result.Errors.Any(error => error.ErrorMessage.Equals(
                $"<li> {_stringLocalizer["ErrorString.TaskReferencesElementInSpaceAfterOwnSpace.Message", element.Name, laterSpace.Name, laterElement.Name]} </li>"));
            Assert.IsTrue(errorMessageFound);
        }

        [Test]
        public void Validate_AllConditionsMet_NoErrorGenerated()
        {
            var world = new LearningWorld("a", "f", "d", "e", "f", "d", "h");
            var space = new LearningSpace("a", "f", "d", 1, Theme.Campus);
            var element = new LearningElement("a", null!, "s", "e", LearningElementDifficultyEnum.Easy,
                ElementModel.l_h5p_blackboard_1, points: 1);
            space.LearningSpaceLayout.LearningElements.Add(0, element);
            world.LearningSpaces.Add(space);

            var result = _validator.Validate(world);
            
            Assert.IsTrue(result.IsValid);
        }
    }
}