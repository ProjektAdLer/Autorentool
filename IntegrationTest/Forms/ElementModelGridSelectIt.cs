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

namespace IntegrationTest.Forms;

[TestFixture]
public class ElementModelGridSelectIt : MudBlazorTestFixture<ElementModelGridSelect>
{
    [SetUp]
    public void SetUp()
    {
        _elementModelHandler = Substitute.For<IElementModelHandler>();
        _elementModelHandler.GetIconForElementModel(Arg.Any<ElementModel>())
            .Returns(x => x.Arg<ElementModel>().ToString());

        Context.Services.AddSingleton(Substitute.For<IStringLocalizer<ElementModelGridSelect>>());
        Context.Services.AddSingleton(_elementModelHandler);

        _systemUnderTest =
            Context.RenderComponent<ElementModelGridSelect>(
                (nameof(ElementModelGridSelect.Elements), _elementModels));
    }

    [TearDown]
    public void TearDown()
    {
        _systemUnderTest.Dispose();
    }

    private IRenderedComponent<ElementModelGridSelect> _systemUnderTest = null!;

    private readonly IEnumerable<ElementModel> _elementModels = new List<ElementModel>
        { ElementModel.l_random, ElementModel.l_h5p_slotmachine_1, ElementModel.l_text_bookshelf_1 };

    private IElementModelHandler _elementModelHandler = null!;

    [Test]
    // ANF-ID: [AWA0002, AWA0015, AWA0003, AWA0010, ASN0011, ASN0013]
    public void ClickingItemShouldChangeValue()
    {
        const ElementModel elementModelToSelect = ElementModel.l_h5p_slotmachine_1;
        var imgElements = _systemUnderTest.FindAll("img");

        var imgToClick = imgElements.FirstOrDefault(img => img.GetAttribute("src") == elementModelToSelect.ToString());

        Assert.Multiple(() =>
        {
            Assert.That(imgToClick, Is.Not.Null);
            Assert.That(_systemUnderTest.Instance.Value, Is.Not.EqualTo(elementModelToSelect));
        });

        imgToClick!.Click();

        Assert.That(_systemUnderTest.Instance.Value, Is.EqualTo(elementModelToSelect));
    }
}