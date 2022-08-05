using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AngleSharp.Common;
using AngleSharp.Dom;
using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.ModalDialog;
using AuthoringTool.View;
using Bunit;
using Microsoft.AspNetCore.Components;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using TestContext = NUnit.Framework.TestContext;

namespace AuthoringToolTest.PresentationLogic.ModalDialog;

[TestFixture]

public class ModalDialogInputFieldsFactoryUt
{
    [TestCase("jpg")]
    [TestCase("png")]
    [TestCase("webp")]
    [TestCase("bmp")]
    [TestCase("mp4")]
    [TestCase("h5p")]
    [TestCase("pdf")]
    public void GetCreateLearningElementInputFields_ValidDragAndDrop_ReturnsCorrectInputFields_ForCorrectFileExtension(string 
        correctFileExtensionForTest)
    {
        var name = "foo";
        var dragAndDropLearningContent =
            new LearningContentViewModel(
                name,
                correctFileExtensionForTest,
                new byte[] { }
            );

        var systemUnderTest = GetModalInputFieldsFactoryForTesting();
        var spaceName = "skazzle";


        var modalDialogInputFields =
            systemUnderTest.GetCreateLearningElementInputFields(dragAndDropLearningContent,
                spaceName).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(modalDialogInputFields.ElementAt(0).Name, Is.EqualTo("Name"));
            Assert.That(modalDialogInputFields.ElementAt(0).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(0).Required, Is.EqualTo(true));

            Assert.That(modalDialogInputFields.ElementAt(1).Name, Is.EqualTo("Shortname"));
            Assert.That(modalDialogInputFields.ElementAt(1).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(1).Required, Is.EqualTo(true));

            //parent Field: solved
            Assert.That(modalDialogInputFields.ElementAt(2).Name, Is.EqualTo("Parent"));
            Assert.That(modalDialogInputFields.ElementAt(2).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(2).Required, Is.EqualTo(true));
            Assert.That(modalDialogInputFields.ElementAt(2), Is.TypeOf<ModalDialogDropdownInputField>());
            if (modalDialogInputFields.ElementAt(2) is ModalDialogDropdownInputField parentDropDownInput)
            {
                Assert.That(parentDropDownInput.ValuesToChoiceMapping.ToList().Count, Is.EqualTo(1));
                Assert.That(parentDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
                Assert.That(parentDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).RequiredValues, Is.EqualTo(null));
                
                Assert.That(parentDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(parentDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ElementParentEnum.Space.ToString()));
            }
            
            
            //assignment Field: hier habe ich mich schwergetan weil ich dachte das man den Objekt typ welcher
            //  im autorentool gebaut wird nachstellen müsste, stimmt das so?
            Assert.That(modalDialogInputFields.ElementAt(3).Name, Is.EqualTo("Assignment"));
            Assert.That(modalDialogInputFields.ElementAt(3).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(3).Required, Is.EqualTo(true));
            if (modalDialogInputFields.ElementAt(3) is ModalDialogDropdownInputField assignmentDropDownInput)
            {
                Assert.That(assignmentDropDownInput.ValuesToChoiceMapping.ToList().Count, Is.EqualTo(1));
                Assert.That(
                    assignmentDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0),
                    Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>()
                    );
                
                Assert.That(
                    assignmentDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).RequiredValues,
                    Is.TypeOf<Dictionary<string,string>>()
                    );
                Assert.That(
                assignmentDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).RequiredValues, 
                Contains.Key("Parent")
                );
                
                Assert.That(
                assignmentDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).RequiredValues["Parent"],
                Is.EqualTo(ElementParentEnum.Space.ToString())
                );

                Assert.That(assignmentDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(assignmentDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices
                .ElementAt(0), Is.EqualTo(spaceName));
            }


            //type field: work in progress
            Assert.That(modalDialogInputFields.ElementAt(4).Name, Is.EqualTo("Type"));
            Assert.That(modalDialogInputFields.ElementAt(4).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(4).Required, Is.EqualTo(true));

            //content field: work in progress
            Assert.That(modalDialogInputFields.ElementAt(5).Name, Is.EqualTo("Content"));
            Assert.That(modalDialogInputFields.ElementAt(5).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(5).Required, Is.EqualTo(true));

            Assert.That(modalDialogInputFields.ElementAt(6).Name, Is.EqualTo("Authors"));
            Assert.That(modalDialogInputFields.ElementAt(6).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(6).Required, Is.EqualTo(false));

            Assert.That(modalDialogInputFields.ElementAt(7).Name, Is.EqualTo("Description"));
            Assert.That(modalDialogInputFields.ElementAt(7).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(7).Required, Is.EqualTo(true));

            Assert.That(modalDialogInputFields.ElementAt(8).Name, Is.EqualTo("Goals"));
            Assert.That(modalDialogInputFields.ElementAt(8).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(8).Required, Is.EqualTo(false));

            //wieso funktioniert das?
            Assert.That(modalDialogInputFields.ElementAt(9).Name, Is.EqualTo("Difficulty"));
            Assert.That(modalDialogInputFields.ElementAt(9).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(9).Required, Is.EqualTo(true));

            Assert.That(modalDialogInputFields.ElementAt(10).Name, Is.EqualTo("Workload (min)"));
            Assert.That(modalDialogInputFields.ElementAt(10).Type, Is.EqualTo(ModalDialogInputType.Number));
            Assert.That(modalDialogInputFields.ElementAt(10).Required, Is.EqualTo(false));
        });
        
    }
    

    [TestCase("abc")]
    [TestCase("xyz")]
    [TestCase("js")]
    [TestCase("baz")]
    public void GetCreateLearningElementInputFields_ThrowsException_ForWrongFileExtension(string wrongFileExtensionForTest)
    {
        var name = "foo";
        var dragAndDropLearningContent =
            new LearningContentViewModel(
                name,
                wrongFileExtensionForTest,
                new byte[] { }
            );
        var spaceName = "skazzle";
        var systemUnderTest = GetModalInputFieldsFactoryForTesting();
        Assert.Throws<Exception>(()=>systemUnderTest.GetCreateLearningElementInputFields(dragAndDropLearningContent,
            spaceName), $"Can not map the file extension '{wrongFileExtensionForTest}' to a ContentType ");
    }

    private ModalDialogInputFieldsFactory GetModalInputFieldsFactoryForTesting()
    {
        return new ModalDialogInputFieldsFactory();
    }

    /*
    [Test]
    public void GetCreateLearningElementInputFields_AlternateTest()
    {
        for (int contentFileTypeTest = 0; contentFileTypeTest < 8; contentFileTypeTest++)
        {
            try
            {
                var dragAndDropLearningContentName = "foo";
                //var dragAndDropLearningContentType =
                //    helper_GetCreateLearningElementInputFields_ReturnsExpectedFileExtension(contentFileTypeTest);
                var dragAndDropLearningContentContent = new byte[] { };
                LearningContentViewModel dragAndDropLearningContent =
                    new LearningContentViewModel(dragAndDropLearningContentName, dragAndDropLearningContentType,
                        dragAndDropLearningContentContent);


                var Name = "foo";
                var ShortName = "fshort";
                var Authors = "bazzle";
                var Description = "skazzle";
                var Goals = "zoo";
                ICollection<ILearningElementViewModel>? learningElements = null;
                var PositionX = 0D;
                var PositionY = 0D;

                LearningSpaceViewModel learningSpace = new LearningSpaceViewModel(Name,
                    ShortName,
                    Authors,
                    Description,
                    Goals,
                    learningElements,
                    PositionX,
                    PositionY);

                IEnumerable<LearningSpaceViewModel> learningSpaces = new[] { learningSpace };

                var worldName = "baskazzle";

                var systemUnderTest = new ModalDialogInputFieldsFactory();
                var modalDialogInputFields = systemUnderTest.GetCreateLearningElementInputFields
                    (dragAndDropLearningContent, learningSpaces, worldName).ToList();


                Assert.Multiple(() =>
                    {
                        Assert.That(modalDialogInputFields.ElementAt(0).Name, Is.EqualTo("Name"));
                        Assert.That(modalDialogInputFields.ElementAt(0).Type, Is.EqualTo(ModalDialogInputType.Text));
                        Assert.That(modalDialogInputFields.ElementAt(0).Required, Is.EqualTo(true));

                        Assert.That(modalDialogInputFields.ElementAt(1).Name, Is.EqualTo("Shortname"));
                        Assert.That(modalDialogInputFields.ElementAt(1).Type, Is.EqualTo(ModalDialogInputType.Text));
                        Assert.That(modalDialogInputFields.ElementAt(1).Required, Is.EqualTo(true));

                        //parent Field: hier habe ich mich schwergetan weil ich dachte das man den Objekt typ welcher
                        //   im autorentool gebaut wird nachstellen müsste, stimmt das so?
                        Assert.That(modalDialogInputFields.ElementAt(2).Name, Is.EqualTo("Parent"));
                        Assert.That(modalDialogInputFields.ElementAt(2).Type, Is.EqualTo(ModalDialogInputType.Text));
                        Assert.That(modalDialogInputFields.ElementAt(2).Required, Is.EqualTo(true));

                        //assignment Field: hier habe ich mich schwergetan weil ich dachte das man den Objekt typ welcher
                        //   im autorentool gebaut wird nachstellen müsste, stimmt das so?
                        Assert.That(modalDialogInputFields.ElementAt(3).Name, Is.EqualTo("Assignment"));
                        Assert.That(modalDialogInputFields.ElementAt(3).Type, Is.EqualTo(ModalDialogInputType.Text));
                        Assert.That(modalDialogInputFields.ElementAt(3).Required, Is.EqualTo(true));


                        // type field: hier habe ich mich schwergetan weil ich dachte das man den Objekt typ welcher
                        //   im autorentool gebaut wird nachstellen müsste, stimmt das so?
                        Assert.That(modalDialogInputFields.ElementAt(4).Name, Is.EqualTo("Type"));
                        Assert.That(modalDialogInputFields.ElementAt(4).Type, Is.EqualTo(ModalDialogInputType.Text));
                        Assert.That(modalDialogInputFields.ElementAt(4).Required, Is.EqualTo(true));

                        // content field: hier habe ich mich schwergetan weil ich dachte das man den Objekt typ welcher
                        //   im autorentool gebaut wird nachstellen müsste, stimmt das so?
                        Assert.That(modalDialogInputFields.ElementAt(5).Name, Is.EqualTo("Content"));
                        Assert.That(modalDialogInputFields.ElementAt(5).Type, Is.EqualTo(ModalDialogInputType.Text));
                        Assert.That(modalDialogInputFields.ElementAt(5).Required, Is.EqualTo(true));

                        Assert.That(modalDialogInputFields.ElementAt(6).Name, Is.EqualTo("Authors"));
                        Assert.That(modalDialogInputFields.ElementAt(6).Type, Is.EqualTo(ModalDialogInputType.Text));
                        Assert.That(modalDialogInputFields.ElementAt(6).Required, Is.EqualTo(false));

                        Assert.That(modalDialogInputFields.ElementAt(7).Name, Is.EqualTo("Description"));
                        Assert.That(modalDialogInputFields.ElementAt(7).Type, Is.EqualTo(ModalDialogInputType.Text));
                        Assert.That(modalDialogInputFields.ElementAt(7).Required, Is.EqualTo(true));

                        Assert.That(modalDialogInputFields.ElementAt(8).Name, Is.EqualTo("Goals"));
                        Assert.That(modalDialogInputFields.ElementAt(8).Type, Is.EqualTo(ModalDialogInputType.Text));
                        Assert.That(modalDialogInputFields.ElementAt(8).Required, Is.EqualTo(false));

                        //wieso funktioniert das?
                        Assert.That(modalDialogInputFields.ElementAt(9).Name, Is.EqualTo("Difficulty"));
                        Assert.That(modalDialogInputFields.ElementAt(9).Type, Is.EqualTo(ModalDialogInputType.Text));
                        Assert.That(modalDialogInputFields.ElementAt(9).Required, Is.EqualTo(true));

                        Assert.That(modalDialogInputFields.ElementAt(10).Name, Is.EqualTo("Workload (min)"));
                        Assert.That(modalDialogInputFields.ElementAt(10).Type, Is.EqualTo(ModalDialogInputType.Number));
                        Assert.That(modalDialogInputFields.ElementAt(10).Required, Is.EqualTo(false));
                    }
                );
            }
            catch (Exception err)
            {
                if (!(contentFileTypeTest is <= 6 and >= 0))
                {
                    Assert.That(err.Message, Is.EqualTo("Can not map the file extension 'abc' to a ContentType "));
                }
            }
        }
    }
*/
    [Test]
    public void GetCreateLearningSpaceInputFields()
    {
        
    }
    
    [Test]
    public void GetEditLearningSpaceInputFields_ReturnsCorrectInputFields()
    {
        var systemUnderTest = new ModalDialogInputFieldsFactory();

        var name = "Name";
        var shortname = "Shortname";
        var authors = "Authors";
        var description = "Description";
        var goals = "Goals";

        var modalDialogInputType = ModalDialogInputType.Text;

        var modalDialogInputFields = systemUnderTest.GetEditLearningSpaceInputFields().ToList();
        Assert.Multiple(() =>
        {
            Assert.That(modalDialogInputFields.ElementAt(0).Name, Is.EqualTo(name));
            Assert.That(modalDialogInputFields.ElementAt(0).Type, Is.EqualTo(modalDialogInputType));
            Assert.That(modalDialogInputFields.ElementAt(0).Required, Is.EqualTo(true));

            Assert.That(modalDialogInputFields.ElementAt(1).Name, Is.EqualTo(shortname));
            Assert.That(modalDialogInputFields.ElementAt(1).Type, Is.EqualTo(modalDialogInputType));
            Assert.That(modalDialogInputFields.ElementAt(1).Required, Is.EqualTo(true));

            Assert.That(modalDialogInputFields.ElementAt(2).Name, Is.EqualTo(authors));
            Assert.That(modalDialogInputFields.ElementAt(2).Type, Is.EqualTo(modalDialogInputType));
            Assert.That(modalDialogInputFields.ElementAt(2).Required, Is.EqualTo(false));

            Assert.That(modalDialogInputFields.ElementAt(3).Name, Is.EqualTo(description));
            Assert.That(modalDialogInputFields.ElementAt(3).Type, Is.EqualTo(modalDialogInputType));
            Assert.That(modalDialogInputFields.ElementAt(3).Required, Is.EqualTo(true));

            Assert.That(modalDialogInputFields.ElementAt(4).Name, Is.EqualTo(goals));
            Assert.That(modalDialogInputFields.ElementAt(4).Type, Is.EqualTo(modalDialogInputType));
            Assert.That(modalDialogInputFields.ElementAt(4).Required, Is.EqualTo(false));
        });
    }

    [Test]
    public void GetEditLearningElementInputFields()
    {
        
    }

}