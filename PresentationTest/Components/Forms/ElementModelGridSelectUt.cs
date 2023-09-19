using System.Collections.Generic;
using System.Linq;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using Presentation.Components.Forms.Element;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Forms;

[TestFixture]
public class ElementModelGridSelectUt
{
    [SetUp]
    public void SetUp()
    {
        _elementModelHandler = Substitute.For<IElementModelHandler>();
        _elementModelHandler.GetIconForElementModel(Arg.Any<ElementModel>())
            .Returns(x => x.Arg<ElementModel>().ToString());

        var testContext = new TestContext();
        testContext.Services.AddSingleton(Substitute.For<IStringLocalizer<ElementModelGridSelect>>());
        testContext.Services.AddSingleton(_elementModelHandler);

        _component =
            testContext.RenderComponent<ElementModelGridSelect>(
                (nameof(ElementModelGridSelect.Elements), _elementModels));
    }

    [TearDown]
    public void TearDown()
    {
        _component.Dispose();
    }

    private IRenderedComponent<ElementModelGridSelect> _component = null!;

    private readonly IEnumerable<ElementModel> _elementModels = new List<ElementModel>
        {ElementModel.l_random, ElementModel.l_h5p_slotmachine_1, ElementModel.l_text_bookshelf_1};

    private IElementModelHandler _elementModelHandler = null!;

    [Test]
    public void ClickingItemShouldChangeValue()
    {
        const ElementModel elementModelToSelect = ElementModel.l_h5p_slotmachine_1;
        var imgElements = _component.FindAll("img");

        var imgToClick = imgElements.FirstOrDefault(img => img.GetAttribute("src") == elementModelToSelect.ToString());
        Assert.That(imgToClick, Is.Not.Null);

        Assert.That(_component.Instance.Value, Is.Not.EqualTo(elementModelToSelect));

        imgToClick!.Click();

        Assert.That(_component.Instance.Value, Is.EqualTo(elementModelToSelect));
    }
}