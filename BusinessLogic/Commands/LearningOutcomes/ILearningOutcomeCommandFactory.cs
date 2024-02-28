using System.Globalization;
using BusinessLogic.Entities.LearningOutcome;
using Shared.LearningOutcomes;

namespace BusinessLogic.Commands.LearningOutcomes;

/// <summary>
/// Factory for creating commands for LearningOutcomeCollection
/// </summary>
public interface ILearningOutcomeCommandFactory
{
    /// <summary>
    /// Creates a command to add a structured learning outcome.
    /// </summary>
    IAddLearningOutcome GetAddLearningOutcomeCommand(LearningOutcomeCollection learningOutcomeCollection,
        TaxonomyLevel taxonomyLevel,
        string what, string verbOfVisibility, string whereby, string whatFor, CultureInfo cultureInfo,
        Action<LearningOutcomeCollection> mappingAction, int index = -1);

    /// <summary>
    /// Creates a command to add a manual learning outcome.
    /// </summary>
    IAddLearningOutcome GetAddLearningOutcomeCommand(LearningOutcomeCollection learningOutcomeCollection,
        string manualLearningOutcomeText, Action<LearningOutcomeCollection> mappingAction, int index = -1);

    /// <summary>
    /// Creates a command to delete a learning outcome.
    /// </summary>
    IDeleteLearningOutcome GetDeleteLearningOutcomeCommand(LearningOutcomeCollection learningOutcomeCollection,
        ILearningOutcome learningOutcome,
        Action<LearningOutcomeCollection> mappingAction);
}