using System.Collections;
using NUnit.Framework;
using Shared;

namespace SharedTest;

[TestFixture]
public class ContentTypeHelperUt
{
    [Test]
    [TestCaseSource(typeof(ContentTypeHelperTestData), nameof(ContentTypeHelperTestData.TestCases))]
    public ContentTypeEnum GetContentType_WhenCalledWithH5P_ReturnsH5P(string type)
    {
        return ContentTypeHelper.GetContentType(type);
    }
    
    [Test]
    public void GetContentType_WhenCalledWithUnsupportedType_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ContentTypeHelper.GetContentType("unsupported"));
    }

    private class ContentTypeHelperTestData
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData("h5p").Returns(ContentTypeEnum.H5P);
                yield return new TestCaseData("txt").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("c").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("h").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("cpp").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("cc").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("c++").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("py").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("cs").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("js").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("php").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("html").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("css").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("jpg").Returns(ContentTypeEnum.Image);
                yield return new TestCaseData("png").Returns(ContentTypeEnum.Image);
                yield return new TestCaseData("webp").Returns(ContentTypeEnum.Image);
                yield return new TestCaseData("bmp").Returns(ContentTypeEnum.Image);
                yield return new TestCaseData("pdf").Returns(ContentTypeEnum.Text);
                yield return new TestCaseData("video").Returns(ContentTypeEnum.Video);
            }
        }
    }
}
