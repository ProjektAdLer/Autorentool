using BusinessLogic.Entities;
using FluentValidation;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using Microsoft.Extensions.Localization;

namespace BusinessLogic.Validation.Validators
{
    public class LearningWorldExportValidator : AbstractValidator<ILearningWorld>
    {
        private readonly IStringLocalizer<LearningWorldExportValidator> _localizer;

        public LearningWorldExportValidator(IStringLocalizer<LearningWorldExportValidator> localizer)
        {
            _localizer = localizer;
            RuleFor(world => world)
                .Must(HaveLearningSpace())
                .WithMessage(world => $"<li> {_localizer["ErrorString.Missing.LearningSpace.Message"]} </li>");

            RuleForEach<ILearningSpace>(world => world.LearningSpaces)
                .Must(HaveLearningElement())
                .WithMessage((world, space) =>
                    $"<li> {_localizer["ErrorString.Missing.LearningElements.Message", space.Name]} </li>")
                .Must(HaveSufficientPoints())
                .WithMessage((world, space) =>
                    $"<li> {_localizer["ErrorString.Insufficient.Points.Message", space.Name]} </li>");


            RuleFor(world => ListofWorldAndAdaptivityContentPairings(world))
                .ForEach(tuple =>
                {
                    tuple.Must(AdaptivityContentHasTask())
                        .WithMessage((enumWorldElement, tupleworldelement) =>
                            $"<li> {_localizer["ErrorString.NoTasks.Message", tupleworldelement.element.Name]} </li>");

                    tuple.Must(NoAdaptivityContentTaskReferencesNonexistantElement())
                        .WithMessage((enumWorldElement, tupleworldelement) =>
                            $"<li> {_localizer["ErrorString.TaskReferencesNonexistantElement.Message", tupleworldelement.element.Name]} </li>");

                    tuple.Must(NoAdaptivityContentTaskReferencesUnplacedElement())
                        .WithMessage((enumWorldElement, tupleworldelement) =>
                            $"<li> {_localizer["ErrorString.TaskReferencesUnplacedElement.Message", tupleworldelement.element.Name]} </li>");
                    tuple.Custom((tupleWorldElement, context) =>
                        NoElementMustReferenceElementInSpaceAfterOwnSpace(tupleWorldElement, context)
                    );
                });

        }

        private void NoElementMustReferenceElementInSpaceAfterOwnSpace(
            (ILearningWorld world, ILearningElement element) tupleWorldElement, ValidationContext<IEnumerable<(ILearningWorld world, ILearningElement element)>> context)
        {
            var invalidSpaceReferences =
                ReferencesElementInSpaceAfterOwnSpace(tupleWorldElement.element, tupleWorldElement.world);

            foreach (var spaceElementTuple in invalidSpaceReferences)
            {
                var errorMessage =
                    $"<li> {_localizer["ErrorString.TaskReferencesElementInSpaceAfterOwnSpace.Message", tupleWorldElement.element.Name, spaceElementTuple.Space.Name, spaceElementTuple.Element.Name]} </li>";
                context.AddFailure(errorMessage);
            }
        }

        private static Func<(ILearningWorld world, ILearningElement element), bool> NoAdaptivityContentTaskReferencesUnplacedElement()
        {
            return tupleWorldElement =>
                tupleWorldElement.element.LearningContent is IAdaptivityContent adaptivityContent &&
                !adaptivityContent.Tasks.Any(TaskReferencesUnplacedElement(tupleWorldElement.world));
        }

        private Func<(ILearningWorld world, ILearningElement element), bool> NoAdaptivityContentTaskReferencesNonexistantElement()
        {
            return tupleWorldElement =>
                tupleWorldElement.element.LearningContent is IAdaptivityContent adaptivityContent &&
                !TaskReferencesNonexistantElement(adaptivityContent, tupleWorldElement.world);
        }

        private static Func<(ILearningWorld world, ILearningElement element), bool> AdaptivityContentHasTask()
        {
            return tupleWorldElement =>
                tupleWorldElement.element.LearningContent is IAdaptivityContent adaptivityContent &&
                adaptivityContent.Tasks.Count > 0;
        }

        private static List<(ILearningWorld world, ILearningElement element)> ListofWorldAndAdaptivityContentPairings(ILearningWorld world)
        {
            return world.AllLearningElements
                .Where(ele => ele.LearningContent is IAdaptivityContent)
                .Select(ele => (world, ele)) 
                .ToList();
        }


        private static Func<ILearningSpace, bool> HaveSufficientPoints()
        {
            return space => space.Points >= space.RequiredPoints;
        }

        private static Func<ILearningSpace, bool> HaveLearningElement()
        {
            return space => space.ContainedLearningElements.Any();
        }

        private Func<ILearningWorld, bool> HaveLearningSpace()
        {
            return world => !HaveNoLearningSpaces(world);
        }

        private bool HaveNoLearningSpaces(ILearningWorld world)
        {
            return world.LearningSpaces.Count == 0;
        }

        private bool TaskReferencesNonexistantElement(IAdaptivityContent adaptivityContent, ILearningWorld world)
        {
            var adaptivityContentReferencedIds = GetElementIdsContentReferences(adaptivityContent);
            return adaptivityContentReferencedIds.Any(id => world.AllLearningElements.All(ele => ele.Id != id));
        }

        public static Func<IAdaptivityTask, bool> TaskReferencesUnplacedElement(ILearningWorld world) =>
            task => task.Questions
                .Any(question => question.Rules
                    .Any(rule => rule.Action is ElementReferenceAction eravm &&
                                 world.UnplacedLearningElements.Any(unplacedEle =>
                                     unplacedEle.Id == eravm.ElementId)));


        /// <summary>
        /// Checks if the given element references any elements that are placed in spaces which come AFTER the space it
        /// itself is in, in the partial ordering of spaces.
        /// </summary>
        private IEnumerable<(ILearningSpace Space, ILearningElement Element)> ReferencesElementInSpaceAfterOwnSpace(
            ILearningElement element, ILearningWorld world)
        {
            var ownSpace =
                world.LearningSpaces.SingleOrDefault(space => space.ContainedLearningElements.Contains(element));
            //element isn't placed yet, can't say anything about it
            if (ownSpace is null) return Enumerable.Empty<(ILearningSpace, ILearningElement)>();

            //find all the id's of elements that the adaptivity content of this element we are looking at references
            var adaptivityContent = (AdaptivityContent)element.LearningContent;
            var adaptivityContentReferencedIds = GetElementIdsContentReferences(adaptivityContent);

            //create a tuple of (space,ele) for every element in every space
            IEnumerable<(ILearningSpace Space, ILearningElement Element)> spaceElementTuples = world.LearningSpaces
                .SelectMany(space => space.ContainedLearningElements
                    .Select(ele => (space, ele)));

            //find all the tuples that contain an element that is referenced by the adaptivity content of the element we are looking at
            var spaceElementTuplesReferenced =
                spaceElementTuples.Where(tup => adaptivityContentReferencedIds.Contains(tup.Element.Id));

            //find all spaces that are after the space the element we are looking at is in
            var spacesAfterOwn = LearningSpaceExtensions.GetFollowingSpacesFrom(ownSpace);

            //find all tuples that contain a space that is after our own space
            return spaceElementTuplesReferenced.Where(tup => spacesAfterOwn.Contains(tup.Space));
        }

        public static IEnumerable<Guid> GetElementIdsContentReferences(IAdaptivityContent adaptivityContent)
        {
            return adaptivityContent.Tasks
                .SelectMany(task => task.Questions
                    .SelectMany(question => question.Rules
                        .Select(rule => rule.Action)
                        .Where(action => action is ElementReferenceAction)
                        .Cast<ElementReferenceAction>()
                        .Select(action => action.ElementId)));
        }

    }
}