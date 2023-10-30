using Generator.ATF.AdaptivityElement;

namespace Generator.ATF;

public interface IReadAtf
{
    /// <summary>
    /// Reads and processes a learning world, either from a specified ATF path or from a provided JSON document.
    /// </summary>
    /// <param name="atfPath">The path to the ATF file to read. Ignored if rootJsonForTest is not null.</param>
    /// <param name="rootJsonForTest">An optional DocumentRootJson object. If null, a file is read from atfPath.</param>
    /// <exception cref="InvalidOperationException">Thrown when unable to deserialize the ATF document from the specified atfPath.</exception>
    void ReadLearningWorld(string atfPath, DocumentRootJson? rootJsonForTest = null);

    /// <summary>
    /// Retrieves the list of H5P elements.
    /// </summary>
    /// <returns>A list of LearningElementJson objects that represent the H5P elements.</returns>
    List<ILearningElementJson> GetH5PElementsList();

    /// <summary>
    /// Retrieves the current learning world object.
    /// </summary>
    ILearningWorldJson GetLearningWorld();

    /// <summary>
    /// Retrieves the learning world attributes.
    /// </summary>
    LearningElementJson GetWorldAttributes();

    /// <summary>
    /// Retrieves the list of learning spaces, including an initial empty space.
    /// </summary>
    /// <returns>A list of LearningSpaceJson objects that represent the learning spaces.</returns>
    List<ILearningSpaceJson> GetSpaceList();

    /// <summary>
    /// Retrieves the list of resource elements.
    /// </summary>
    /// <returns>A list of LearningElementJson objects that represent the resource elements.</returns>
    List<ILearningElementJson> GetResourceElementList();

    /// <summary>
    /// Retrieves the list of URL elements.
    /// </summary>
    /// <returns>A list of LearningElementJson objects that represent the URL elements.</returns>
    List<ILearningElementJson> GetUrlElementList();

    /// <summary>
    /// Retrieves the list of adaptivity elements.
    /// </summary>
    /// <returns>A list of IAdaptivityElementJson objects that represent the adaptivity elements. </returns>
    List<IAdaptivityElementJson> GetAdaptivityElementsList();

    /// <summary>
    /// Retrieves the list of elements in their correct order.
    /// </summary>
    /// <returns>A list of LearningElementJson objects that represent the ordered elements.</returns>
    List<IElementJson> GetElementsOrderedList();

    /// <summary>
    /// Retrieves the list of base learning elements.
    /// </summary>
    /// <returns>A list of BaseLearningElementJson objects that represent the base learning elements.</returns>
    List<IBaseLearningElementJson> GetBaseLearningElementsList();
}