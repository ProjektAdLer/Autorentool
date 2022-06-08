using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.View.Toolbox;

/// <summary>
/// Provides filtering service to Toolbox for Search functionality
/// </summary>
public interface IToolboxResultFilter
{
    /// <summary>
    /// Filters the passed collection according to the provided search term.
    /// </summary>
    /// <remarks>
    /// Searching for a plain string will return only objects that contain said string in their name, case-insensitively.
    /// Search terms starting with the words 'world', 'space' or 'element' will return only objects of that type.
    /// Appending a colon and another search term will return only objects of that type and whos name contains the secondary search term,
    /// e.g. <c>space:test</c> will only return <see cref="LearningSpaceViewModel"/> objects that have the substring 'test' in their name.
    /// Beginning and ending the search term with quotation marks will result in the string being taken literally,
    /// ignoring the above rules and instead searching for the entire <paramref name="searchTerm"/> directly.
    /// </remarks>
    /// <param name="items">The items to be filtered.</param>
    /// <param name="searchTerm">The search term by which <paramref name="items"/> should be filtered, adhering to the rules set forth in the remarks.</param>
    /// <returns>All <see cref="IDisplayableLearningObject"/> objects in <paramref name="items"/> that are matched by the <paramref name="searchTerm"/>.</returns>
    IEnumerable<IDisplayableLearningObject> FilterCollection(IEnumerable<IDisplayableLearningObject> items,
        string searchTerm);
    
    /// <summary>
    /// Determines whether or not a given search term triggers the quote rule (take entire search term literally).
    /// </summary>
    /// <param name="searchTerm">The term to be tested.</param>
    /// <returns>Whether or not the quote rule shall be triggered.</returns>
    bool MatchesQuoteRule(string searchTerm);
    
    /// <summary>
    /// Determines whether or not a given search term triggers the 'world' rule (return only <see cref="LearningWorldViewModel"/> objects).
    /// </summary>
    /// <param name="searchTerm">The term to be tested.</param>
    /// <returns>Whether or not the 'world' rule shall be triggered.</returns>
    bool MatchesWorldRule(string searchTerm);
    
    /// <summary>
    /// Determines whether or not a given search term triggers the 'space' rule (return only <see cref="LearningSpaceViewModel"/> objects).
    /// </summary>
    /// <param name="searchTerm">The term to be tested.</param>
    /// <returns>Whether or not the 'space' rule shall be triggered.</returns>
    bool MatchesSpaceRule(string searchTerm);
    
    /// <summary>
    /// Determines whether or not a given search term triggers the 'element' rule (return only <see cref="LearningElementViewModel"/> objects).
    /// </summary>
    /// <param name="searchTerm">The term to be tested.</param>
    /// <returns>Whether or not the 'element' rule shall be triggered.</returns>
    bool MatchesElementRule(string searchTerm);
    
    /// <summary>
    /// The Regex string used for <see cref="MatchesQuoteRule"/>.
    /// </summary>
    /// <remarks>See https://regex101.com/r/Ge3MQs/1 for an explanation of the Regex.</remarks>
    static string QuoteRegexString => @"^(?:\s*)(?:\"")+(.*?)(?:\"")+(?:\s*)$";
    
    /// <summary>
    /// The Regex string used for <see cref="MatchesWorldRule"/>.
    /// </summary>
    /// <remarks>See https://regex101.com/r/poPQoK/1 for an explanation of the Regex.</remarks>
    static string WorldRegexString => @"^(?:\s*)(?:world:)(?:\s*)(.*)$|^(?:\s*)(?:world)(?:.*)$";
    
    /// <summary>
    /// The Regex string used for <see cref="MatchesSpaceRule"/>.
    /// </summary>
    /// <remarks>See https://regex101.com/r/8dQqAN/1 for an explanation of the Regex.</remarks>
    static string SpaceRegexString => @"^(?:\s*)(?:space:)(?:\s*)(.*)$|^(?:\s*)(?:space)(?:.*)$";
    
    /// <summary>
    /// The Regex string used for <see cref="MatchesElementRule"/>.
    /// </summary>
    /// <remarks>See https://regex101.com/r/0QEoXQ/1 for an explanation of the Regex.</remarks>
    static string ElementRegexString => @"^(?:\s*)(?:element:)(?:\s*)(.*)$|^(?:\s*)(?:element)(?:.*)$";
}