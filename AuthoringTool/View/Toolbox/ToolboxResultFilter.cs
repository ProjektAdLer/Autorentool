using System.Text.RegularExpressions;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.View.Toolbox;

public class ToolboxResultFilter : IToolboxResultFilter 
{

    private readonly Regex quoteRegex;
    private readonly Regex worldRegex;
    private readonly Regex spaceRegex;
    private readonly Regex elementRegex;

    public ToolboxResultFilter()
    {
        quoteRegex = new Regex(IToolboxResultFilter.QuoteRegexString);
        worldRegex = new Regex(IToolboxResultFilter.WorldRegexString);
        spaceRegex = new Regex(IToolboxResultFilter.SpaceRegexString);
        elementRegex = new Regex(IToolboxResultFilter.ElementRegexString);
    }

    public string UserExplanationText => 
@"Enter a search term to filter objects containing it in their name. Case is ignored.
A search term beginning with ""world"", ""space"", or ""element"" will only match those types of objects.
Example: ""world:basics"" will match all worlds containing ""basics"" in their name.
Search terms can be quoted to search them literally, ignoring the above rules.
";

    /// <inheritdoc cref="IToolboxResultFilter.FilterCollection"/>
    public IEnumerable<IDisplayableLearningObject> FilterCollection(IEnumerable<IDisplayableLearningObject> items, string searchTerm)
    {
        Match? match;
        
        if (MatchesQuoteRule(searchTerm))
        {
            match = quoteRegex.Match(searchTerm);
        }
        else if (MatchesWorldRule(searchTerm))
        {
            items = items.Where(item => item is LearningWorldViewModel);
            match = worldRegex.Match(searchTerm);
        } 
        else if (MatchesSpaceRule(searchTerm))
        {
            items = items.Where(item => item is LearningSpaceViewModel);
            match = spaceRegex.Match(searchTerm);
        } 
        else if (MatchesElementRule(searchTerm))
        {
            items = items.Where(item => item is LearningElementViewModel);
            match = elementRegex.Match(searchTerm);
        }
        else
        {
            return FilterCollectionByString(items, searchTerm);
        }
        
        var captureGroupValue = match.Groups[1].Value;
        return string.IsNullOrWhiteSpace(captureGroupValue) ? items : FilterCollectionByString(items, captureGroupValue);
    }

    /// <summary>
    /// Returns elements whose name contains the searchTerm, case-insensitively.
    /// </summary>
    /// <param name="items">The items to be filtered.</param>
    /// <param name="searchTerm">The string with which to filter.</param>
    /// <returns>All elements that contain the search term in name.</returns>
    private static IEnumerable<IDisplayableLearningObject> FilterCollectionByString(
        IEnumerable<IDisplayableLearningObject> items, string searchTerm)
    {
        var trimmedSearchTerm = searchTerm.ToLower().Trim();
        return items.Where(item => item.Name.ToLower().Contains(trimmedSearchTerm));
    }
    
    /// <inheritdoc cref="IToolboxResultFilter.MatchesQuoteRule"/>
    public bool MatchesQuoteRule(string searchTerm) => quoteRegex.IsMatch(searchTerm);
    
    /// <inheritdoc cref="IToolboxResultFilter.MatchesWorldRule"/>
    public bool MatchesWorldRule(string searchTerm) => worldRegex.IsMatch(searchTerm);

    /// <inheritdoc cref="IToolboxResultFilter.MatchesSpaceRule"/>
    public bool MatchesSpaceRule(string searchTerm) => spaceRegex.IsMatch(searchTerm);

    /// <inheritdoc cref="IToolboxResultFilter.MatchesElementRule"/>
    public bool MatchesElementRule(string searchTerm) => elementRegex.IsMatch(searchTerm);
}