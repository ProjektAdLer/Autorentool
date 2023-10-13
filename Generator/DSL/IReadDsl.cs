using Generator.DSL.AdaptivityElement;

namespace Generator.DSL;

public interface IReadDsl
{
    /// <summary>
    /// Reads and processes a learning world, either from a specified DSL path or from a provided JSON document.
    /// </summary>
    /// <param name="dslPath">The path to the DSL file to read. Ignored if rootJsonForTest is not null.</param>
    /// <param name="rootJsonForTest">An optional DocumentRootJson object. If null, a file is read from dslPath.</param>
    /// <exception cref="InvalidOperationException">Thrown when unable to deserialize the DSL document from the specified dslPath.</exception>
    void ReadLearningWorld(string dslPath, DocumentRootJson? rootJsonForTest = null);

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
    List<ILearningSpaceJson> GetSectionList();

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
}