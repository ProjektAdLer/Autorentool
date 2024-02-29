using System.Globalization;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.Logging;
using Shared.LearningOutcomes;

namespace BusinessLogic.Commands.LearningOutcomes;

public class LearningOutcomeCommandFactory : ILearningOutcomeCommandFactory
{
    public LearningOutcomeCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    public IAddLearningOutcome GetAddLearningOutcomeCommand(LearningOutcomeCollection learningOutcomeCollection,
        TaxonomyLevel taxonomyLevel, string what, string verbOfVisibility, string whereby, string whatFor,
        CultureInfo cultureInfo, Action<LearningOutcomeCollection> mappingAction, int index) =>
        new AddLearningOutcome(learningOutcomeCollection, taxonomyLevel, what, verbOfVisibility, whereby, whatFor,
            cultureInfo, mappingAction, LoggerFactory.CreateLogger<AddLearningOutcome>(), index);

    public IAddLearningOutcome GetAddLearningOutcomeCommand(LearningOutcomeCollection learningOutcomeCollection,
        string manualLearningOutcomeText, Action<LearningOutcomeCollection> mappingAction, int index) =>
        new AddLearningOutcome(learningOutcomeCollection, manualLearningOutcomeText, mappingAction,
            LoggerFactory.CreateLogger<AddLearningOutcome>(), index);

    public IDeleteLearningOutcome GetDeleteLearningOutcomeCommand(LearningOutcomeCollection learningOutcomeCollection,
        ILearningOutcome learningOutcome, Action<LearningOutcomeCollection> mappingAction) =>
        new DeleteLearningOutcome(learningOutcomeCollection, learningOutcome, mappingAction,
            LoggerFactory.CreateLogger<DeleteLearningOutcome>());
}