using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using Microsoft.Extensions.Localization;
using Shared;
using Shared.H5P;

namespace BusinessLogic.Validation.Validators;

public class LearningWorldStructureValidator : ILearningWorldStructureValidator
{
    private readonly IStringLocalizer<LearningWorldStructureValidator> _localizer;

    public LearningWorldStructureValidator(
        IStringLocalizer<LearningWorldStructureValidator> localizer)
    {
        _localizer = localizer;
    }

    public ValidationResult ValidateForExport(ILearningWorld world, List<ILearningContent> listLearningContent)
    {
        var result = new ValidationResult();

        foreach (var element in world.LearningSpaces.SelectMany(space => space.ContainedLearningElements))
        {
            if (element.LearningContent is not (IFileContent or ILinkContent))
                continue;

            if (!listLearningContent.Contains(element.LearningContent))
            {
                result.Errors.Add(
                    $"<li>{_localizer["ErrorString.Missing.LearningContent.Message", element.Name]}</li>");
            }
        }

        foreach (var (adaptivityContent, learningElement) in world.AllLearningElements
                     .Where(ele => ele.LearningContent is IAdaptivityContent)
                     .Select(ele => ((IAdaptivityContent)ele.LearningContent, ele)))
        {
            if (TaskReferencesNonExistentElement(adaptivityContent, world))
            {
                result.Errors.Add(
                    $"<li>{_localizer["ErrorString.TaskReferencesNonexistantElement.Message", learningElement.Name]}</li>");
            }

            if (TaskReferencesNonExistentContent(adaptivityContent, listLearningContent))
            {
                result.Errors.Add(
                    $"<li>{_localizer["ErrorString.TaskReferencesNonexistantContent.Message", learningElement.Name]}</li>");
            }
        }

        return result;
    }

    private static bool TaskReferencesNonExistentElement(IAdaptivityContent adaptivityContent, ILearningWorld world)
    {
        var ids = GetElementIdsContentReferences(adaptivityContent);
        return ids.Any(id => world.AllLearningElements.All(ele => ele.Id != id));
    }

    private bool TaskReferencesNonExistentContent(IAdaptivityContent adaptivityContent, List<ILearningContent> listLearningContent)
    {
        var contents = GetContentReferencesByAdaptivityContent(adaptivityContent);
        return contents.Any(content => !listLearningContent.Contains(content));
    }

    private static IEnumerable<Guid> GetElementIdsContentReferences(IAdaptivityContent adaptivityContent)
    {
        return adaptivityContent.Tasks
            .SelectMany(t => t.Questions)
            .SelectMany(q => q.Rules)
            .Select(r => r.Action)
            .OfType<ElementReferenceAction>()
            .Select(a => a.ElementId);
    }

    private static IEnumerable<ILearningContent> GetContentReferencesByAdaptivityContent(
        IAdaptivityContent adaptivityContent)
    {
        return adaptivityContent.Tasks
            .SelectMany(t => t.Questions)
            .SelectMany(q => q.Rules)
            .Select(r => r.Action)
            .OfType<ContentReferenceAction>()
            .Select(a => a.Content);
    }

    public ValidationResult ValidateForGeneration(ILearningWorld world, List<ILearningContent> listLearningContent)
    {
        var result = ValidateForExport(world, listLearningContent);

        ValidateLearningSpaceCount(world, result);
        ValidateLearningSpaces(world, result);
        
        return result;
    }

    private void ValidateLearningSpaceCount(ILearningWorld world, ValidationResult result)
    {
        if (!world.LearningSpaces.Any())
        {
            result.Errors.Add($"<li>{_localizer["ErrorString.Missing.LearningSpace.Message"]}</li>");
        }
        else if (world.LearningSpaces.Count > 50)
        {
            result.Errors.Add($"<li>{_localizer["ErrorString.TooMany.LearningSpaces.Message"]}</li>");
        }
    }

    private void ValidateLearningSpaces(ILearningWorld world, ValidationResult result)
    {
        foreach (var space in world.LearningSpaces)
        {
            if (!space.ContainedLearningElements.Any())
            {
                result.Errors.Add($"<li>{_localizer["ErrorString.Missing.LearningElements.Message", space.Name]}</li>");
            }

            if (space.Points < space.RequiredPoints)
            {
                result.Errors.Add($"<li>{_localizer["ErrorString.Insufficient.Points.Message", space.Name]}</li>");
            }

            IfH5PHasStateNotUsableOrNotValidatedReturnError(world, result, space);
            
            
            
        }
    }

    private void IfH5PHasStateNotUsableOrNotValidatedReturnError(ILearningWorld world, ValidationResult result,
        ILearningSpace space)
    {
        foreach (var element in space.ContainedLearningElements)
        {
            ValidateAdaptivityRules(element, world, result);
            if (element.LearningContent is IFileContent fileContent && fileContent.IsH5P)
            {
                if (fileContent.H5PState is H5PContentState.NotUsable or H5PContentState.NotValidated)
                {
                    result.Errors.Add("<li>All H5P-File-Contents must be usable!</li>");
                }
            }
        }
    }

    private void ValidateAdaptivityRules(ILearningElement element, ILearningWorld world, ValidationResult result)
    {
        if (element.LearningContent is not IAdaptivityContent adaptivityContent)
            return;

        var invalidReferences = ReferencesElementInSpaceAfterOwnSpace(element, world);
        foreach (var (targetSpace, targetElement) in invalidReferences)
        {
            result.Errors.Add(
                $"<li>{_localizer["ErrorString.TaskReferencesElementInSpaceAfterOwnSpace.Message", element.Name, targetElement.Name, targetSpace.Name]}</li>");
        }

        if (adaptivityContent.Tasks.Any(TaskReferencesUnplacedElement(world)))
        {
            result.Errors.Add(
                $"<li>{_localizer["ErrorString.TaskReferencesUnplacedElement.Message", element.Name]}</li>");
        }
    }

    private static Func<IAdaptivityTask, bool> TaskReferencesUnplacedElement(ILearningWorld world) =>
        task => task.Questions
            .Any(q => q.Rules
                .Any(r => r.Action is ElementReferenceAction action &&
                          world.UnplacedLearningElements.Any(unplaced => unplaced.Id == action.ElementId)));

    private static IEnumerable<(ILearningSpace Space, ILearningElement Element)> ReferencesElementInSpaceAfterOwnSpace(
        ILearningElement element, ILearningWorld world)
    {
        var ownSpace = world.LearningSpaces.SingleOrDefault(s => s.ContainedLearningElements.Contains(element));
        if (ownSpace == null)
            return Enumerable.Empty<(ILearningSpace, ILearningElement)>();

        if (element.LearningContent is not IAdaptivityContent adaptivityContent)
            return Enumerable.Empty<(ILearningSpace, ILearningElement)>();

        var referencedIds = GetElementIdsContentReferences(adaptivityContent);
        var spaceElementTuples = world.LearningSpaces
            .SelectMany(space => space.ContainedLearningElements.Select(e => (space, e)));
        var referencedTuples = spaceElementTuples.Where(t => referencedIds.Contains(t.e.Id));
        var spacesAfterOwn = GetFollowingSpaces(ownSpace);

        return referencedTuples.Where(t => spacesAfterOwn.Contains(t.space));
    }

    private static IEnumerable<LearningSpace> GetFollowingSpaces(ILearningSpace initialSpace)
    {
        var queue = new Queue<IObjectInPathWay>(new[] { initialSpace });
        var visited = new HashSet<Guid>();
        var result = new List<LearningSpace>();

        while (queue.Any())
        {
            var current = queue.Dequeue();
            foreach (var next in current.OutBoundObjects)
            {
                if (next is LearningSpace nextSpace && visited.Add(nextSpace.Id))
                {
                    result.Add(nextSpace);
                    queue.Enqueue(nextSpace);
                }
                else
                {
                    queue.Enqueue(next);
                }
            }
        }

        return result;
    }
}