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
    List<LearningElementJson> GetH5PElementsList();

    /// <summary>
    /// Retrieves the current learning world object.
    /// </summary>
    LearningWorldJson GetLearningWorld();

    /// <summary>
    /// Retrieves the list of learning spaces, including an initial empty space.
    /// </summary>
    /// <returns>A list of LearningSpaceJson objects that represent the learning spaces.</returns>
    List<LearningSpaceJson> GetSectionList();

    /// <summary>
    /// Retrieves the list of resource elements.
    /// </summary>
    /// <returns>A list of LearningElementJson objects that represent the resource elements.</returns>
    List<LearningElementJson> GetResourceElementList();

    /// <summary>
    /// Retrieves the list of label elements.
    /// </summary>
    /// <returns>A list of LearningElementJson objects that represent the label elements.</returns>
    List<LearningElementJson> GetLabelElementList();

    /// <summary>
    /// Retrieves the list of URL elements.
    /// </summary>
    /// <returns>A list of LearningElementJson objects that represent the URL elements.</returns>
    List<LearningElementJson> GetUrlElementList();

    /// <summary>
    /// Retrieves the list of elements in their correct order.
    /// </summary>
    /// <returns>A list of LearningElementJson objects that represent the ordered elements.</returns>
    List<LearningElementJson> GetElementsOrderedList();
}