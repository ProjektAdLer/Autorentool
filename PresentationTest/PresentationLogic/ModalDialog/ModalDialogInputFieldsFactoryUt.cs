using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.ModalDialog;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.Space.SpaceLayout;
using Shared;

namespace PresentationTest.PresentationLogic.ModalDialog;

[TestFixture]
public class ModalDialogInputFieldsFactoryUt
{
    [TestCase("jpg", ContentTypeEnum.Image)]
    [TestCase("png", ContentTypeEnum.Image)]
    [TestCase("webp", ContentTypeEnum.Image)]
    [TestCase("bmp", ContentTypeEnum.Image)]
    [TestCase("txt", ContentTypeEnum.Text)]
    [TestCase("c", ContentTypeEnum.Text)]
    [TestCase("h", ContentTypeEnum.Text)]
    [TestCase("cpp", ContentTypeEnum.Text)]
    [TestCase("cc", ContentTypeEnum.Text)]
    [TestCase("c++", ContentTypeEnum.Text)]
    [TestCase("py", ContentTypeEnum.Text)]
    [TestCase("cs", ContentTypeEnum.Text)]
    [TestCase("js", ContentTypeEnum.Text)]
    [TestCase("php", ContentTypeEnum.Text)]
    [TestCase("html", ContentTypeEnum.Text)]
    [TestCase("css", ContentTypeEnum.Text)]
    [TestCase("h5p", ContentTypeEnum.H5P)]
    [TestCase("pdf", ContentTypeEnum.PDF)]
    public void GetCreateElementInputFields_ValidDragAndDrop_ReturnsCorrectInputFields_ForCorrectFileExtensionAndCorrectFileType(string 
        correctFileExtensionForTest, ContentTypeEnum correctFileTypeForTest)
    {
        var name = "foo";
        var dragAndDropContent =
            new ContentViewModel(
                name,
                correctFileExtensionForTest,
                ""
            );
        var spaceName = "skazzle";

        var systemUnderTest = GetModalDialogInputFieldsFactoryForTesting();


        var modalDialogInputFields =
            systemUnderTest.GetCreateElementInputFields(dragAndDropContent).ToList();

        //Name
        var nameInputField = modalDialogInputFields.ElementAt(0);
        Assert.That(nameInputField.Name, Is.EqualTo("Name"));
        Assert.That(nameInputField.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(nameInputField.Required, Is.True);

        var shortnameInputField = modalDialogInputFields.ElementAt(1);
        Assert.That(shortnameInputField.Name, Is.EqualTo("Shortname"));
        Assert.That(shortnameInputField.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(shortnameInputField.Required, Is.False);


        //Type drop down
        Assert.That(modalDialogInputFields.ElementAt(2), Is.TypeOf<ModalDialogDropdownInputField>());
        var typeDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(2);
        
        Assert.That(typeDropDownInput.Name, Is.EqualTo("Type"));
        Assert.That(typeDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(typeDropDownInput.Required, Is.True);
        Assert.That(typeDropDownInput.ValuesToChoiceMapping.Count(), Is.EqualTo(1));
        var typeChoiceMapping = typeDropDownInput.ValuesToChoiceMapping.ElementAt(0);
        Assert.That(typeChoiceMapping.RequiredValues, Is.Null);
        switch (correctFileTypeForTest)
        {
            case ContentTypeEnum.Image:
            case ContentTypeEnum.Text:
            case ContentTypeEnum.PDF:
            {
                Assert.That(typeChoiceMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(typeChoiceMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                break;
            }
            case ContentTypeEnum.Video:
            {
                Assert.That(typeChoiceMapping.AvailableChoices.Count(), Is.EqualTo(2));
                Assert.That(typeChoiceMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                Assert.That(typeChoiceMapping.AvailableChoices.ElementAt(1), Is.EqualTo(ElementTypeEnum.Activation.ToString()));
                break;
            }
            case ContentTypeEnum.H5P:
            {
                Assert.That(typeChoiceMapping.AvailableChoices.Count(), Is.EqualTo(3));
                Assert.That(typeChoiceMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ElementTypeEnum.Activation.ToString()));
                Assert.That(typeChoiceMapping.AvailableChoices.ElementAt(1), Is.EqualTo(ElementTypeEnum.Interaction.ToString()));
                Assert.That(typeChoiceMapping.AvailableChoices.ElementAt(2), Is.EqualTo(ElementTypeEnum.Test.ToString()));
                break;
            }
            default:
            {
                Assert.Fail("Invalid content type");
                break;
            }
        }

        //Content drop down
        Assert.That(modalDialogInputFields.ElementAt(3), Is.TypeOf<ModalDialogDropdownInputField>());
        var contentDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(3);
        Assert.That(contentDropDownInput.Name, Is.EqualTo("Content"));
        Assert.That(contentDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(contentDropDownInput.Required, Is.True);
        var contentChoiceMappings = contentDropDownInput.ValuesToChoiceMapping.ToList();
        switch (correctFileTypeForTest)
        {
            case ContentTypeEnum.Image:
            {
                Assert.That(contentChoiceMappings.Count, Is.EqualTo(1));

                var firstMapping = contentChoiceMappings.First();
                Assert.That(firstMapping.RequiredValues, Is.Not.Null);
                Assert.That(firstMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(firstMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(firstMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
            
                Assert.That(firstMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(firstMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Image.ToString()));
                break;
            }
            case ContentTypeEnum.Video:
            {
                Assert.That(contentChoiceMappings.Count, Is.EqualTo(2));
            
                var firstMapping = contentChoiceMappings.First();
                var secondMapping = contentChoiceMappings.ElementAt(1);
                Assert.That(firstMapping.RequiredValues, Is.Not.Null);
                Assert.That(firstMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(firstMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(firstMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
            
                Assert.That(firstMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(firstMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Video.ToString()));
            
                Assert.That(secondMapping.RequiredValues, Is.Not.Null);
                Assert.That(secondMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(secondMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(secondMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Activation.ToString()));
            
                Assert.That(secondMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(secondMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Video.ToString()));
                break;
            }
            case ContentTypeEnum.PDF:
            {
                Assert.That(contentChoiceMappings.Count, Is.EqualTo(1));
            
                var firstMapping = contentChoiceMappings.First();
                Assert.That(firstMapping.RequiredValues, Is.Not.Null);
                Assert.That(firstMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(firstMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(firstMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
            
                Assert.That(firstMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(firstMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.PDF.ToString()));
                break;
            }
            case ContentTypeEnum.H5P:
            {
                Assert.That(contentChoiceMappings.Count, Is.EqualTo(3));
            
                var firstMapping = contentChoiceMappings.First();
                var secondMapping = contentChoiceMappings.ElementAt(1);
                var thirdMapping = contentChoiceMappings.ElementAt(2);
                Assert.That(firstMapping.RequiredValues, Is.Not.Null);
                Assert.That(firstMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(firstMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(firstMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Activation.ToString()));
            
                Assert.That(firstMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(firstMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
            
            
                Assert.That(secondMapping.RequiredValues, Is.Not.Null);
                Assert.That(secondMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(secondMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(secondMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Interaction.ToString()));
            
                Assert.That(secondMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(secondMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
            
            
                Assert.That(thirdMapping.RequiredValues, Is.Not.Null);
                Assert.That(thirdMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(thirdMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(thirdMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Test.ToString()));
            
                Assert.That(thirdMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(thirdMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
                break;
            }
        }
        Assert.That(modalDialogInputFields.ElementAt(4).Name, Is.EqualTo("Url"));
        Assert.That(modalDialogInputFields.ElementAt(4).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(4).Required, Is.False);
        
        Assert.That(modalDialogInputFields.ElementAt(5).Name, Is.EqualTo("Authors"));
        Assert.That(modalDialogInputFields.ElementAt(5).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(5).Required, Is.False);

        Assert.That(modalDialogInputFields.ElementAt(6).Name, Is.EqualTo("Description"));
        Assert.That(modalDialogInputFields.ElementAt(6).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(6).Required, Is.False);

        Assert.That(modalDialogInputFields.ElementAt(7).Name, Is.EqualTo("Goals"));
        Assert.That(modalDialogInputFields.ElementAt(7).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(7).Required, Is.False);

        //difficulty: solved
        Assert.That(modalDialogInputFields.ElementAt(8), Is.TypeOf<ModalDialogDropdownInputField>());
        var difficultyDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(8);
        Assert.That(difficultyDropDownInput.Name, Is.EqualTo("Difficulty"));
        Assert.That(difficultyDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(difficultyDropDownInput.Required, Is.False);
        var difficultyChoiceMappings = difficultyDropDownInput.ValuesToChoiceMapping.ToList();
        
        Assert.That(difficultyChoiceMappings.Count, Is.EqualTo(1));
        var difficultyMapping = difficultyChoiceMappings.First();
        
        Assert.That(difficultyMapping.RequiredValues, Is.Null);
        Assert.That(difficultyMapping.AvailableChoices.Count(), Is.EqualTo(4));
        Assert.That(difficultyMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ElementDifficultyEnum.None.ToString()));
        Assert.That(difficultyMapping.AvailableChoices.ElementAt(1), Is.EqualTo(ElementDifficultyEnum.Easy.ToString()));
        Assert.That(difficultyMapping.AvailableChoices.ElementAt(2), Is.EqualTo(ElementDifficultyEnum.Medium.ToString()));
        Assert.That(difficultyMapping.AvailableChoices.ElementAt(3), Is.EqualTo(ElementDifficultyEnum.Hard.ToString()));

        Assert.That(modalDialogInputFields.ElementAt(9).Name, Is.EqualTo("Workload (min)"));
        Assert.That(modalDialogInputFields.ElementAt(9).Type, Is.EqualTo(ModalDialogInputType.Number));
        Assert.That(modalDialogInputFields.ElementAt(9).Required, Is.False);
    
        
    }

    [TestCase("jpg", ContentTypeEnum.Image)]
    [TestCase("png", ContentTypeEnum.Image)]
    [TestCase("webp", ContentTypeEnum.Image)]
    [TestCase("bmp", ContentTypeEnum.Image)]
    [TestCase("txt", ContentTypeEnum.Text)]
    [TestCase("c", ContentTypeEnum.Text)]
    [TestCase("h", ContentTypeEnum.Text)]
    [TestCase("cpp", ContentTypeEnum.Text)]
    [TestCase("cc", ContentTypeEnum.Text)]
    [TestCase("c++", ContentTypeEnum.Text)]
    [TestCase("py", ContentTypeEnum.Text)]
    [TestCase("cs", ContentTypeEnum.Text)]
    [TestCase("js", ContentTypeEnum.Text)]
    [TestCase("php", ContentTypeEnum.Text)]
    [TestCase("html", ContentTypeEnum.Text)]
    [TestCase("css", ContentTypeEnum.Text)]
    [TestCase("h5p", ContentTypeEnum.H5P)]
    [TestCase("pdf", ContentTypeEnum.PDF)]
    public void GetCreateElementInputFields_NoDragAndDrop_ReturnsCorrectInputFields(string 
        correctFileExtensionForTest, ContentTypeEnum correctFileTypeForTest)
    {
        var systemUnderTest = GetModalDialogInputFieldsFactoryForTesting();
        var spaceName = "skazzle";


        var modalDialogInputFields =
            systemUnderTest.GetCreateElementInputFields(null).ToList();
        
        
            Assert.That(modalDialogInputFields.ElementAt(0).Name, Is.EqualTo("Name"));
            Assert.That(modalDialogInputFields.ElementAt(0).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(0).Required, Is.True);

            Assert.That(modalDialogInputFields.ElementAt(1).Name, Is.EqualTo("Shortname"));
            Assert.That(modalDialogInputFields.ElementAt(1).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(1).Required, Is.False);

            //type field: solved
            var typeDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(2);
            Assert.That(typeDropDownInput.Name, Is.EqualTo("Type"));
            Assert.That(typeDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(typeDropDownInput.Required, Is.True);
            Assert.That(typeDropDownInput, Is.TypeOf<ModalDialogDropdownInputField>());
            
            var typeValuesToChoiceList = typeDropDownInput.ValuesToChoiceMapping.ToList();
            Assert.That(typeValuesToChoiceList.Count, Is.EqualTo(1));
            Assert.That(typeValuesToChoiceList.ElementAt(0), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
                
            Assert.That(typeValuesToChoiceList.ElementAt(0).RequiredValues, Is.Null);
            Assert.That(typeValuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(4));
            Assert.That(typeValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
            Assert.That(typeValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(1), Is.EqualTo(ElementTypeEnum.Activation.ToString()));
            Assert.That(typeValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(2), Is.EqualTo(ElementTypeEnum.Interaction.ToString()));
            Assert.That(typeValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(3), Is.EqualTo(ElementTypeEnum.Test.ToString()));
            
            
            //content field: solved
            var contentDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(3);
            Assert.That(contentDropDownInput.Name, Is.EqualTo("Content"));
            Assert.That(contentDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(contentDropDownInput.Required, Is.True);
            Assert.That(contentDropDownInput, Is.TypeOf<ModalDialogDropdownInputField>());
            
            var contentValuesToChoiceList = contentDropDownInput.ValuesToChoiceMapping.ToList();
            Assert.That(contentValuesToChoiceList.Count(), Is.EqualTo(4));
            Assert.That(contentValuesToChoiceList.ElementAt(0), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
            Assert.That(contentValuesToChoiceList.ElementAt(1), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
            Assert.That(contentValuesToChoiceList.ElementAt(2), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
            Assert.That(contentValuesToChoiceList.ElementAt(3), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
            
            Assert.That(contentValuesToChoiceList.ElementAt(0).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
            Assert.That(contentValuesToChoiceList.ElementAt(0).RequiredValues!.Count(), Is.EqualTo(1));
            Assert.That(contentValuesToChoiceList.ElementAt(0).RequiredValues, Contains.Key("Type"));
            Assert.That(contentValuesToChoiceList.ElementAt(0).RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                
            Assert.That(contentValuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(4));
            Assert.That(contentValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Image.ToString()));
            Assert.That(contentValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(1), Is.EqualTo(ContentTypeEnum.Text.ToString()));
            Assert.That(contentValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(2), Is.EqualTo(ContentTypeEnum.Video.ToString()));
            Assert.That(contentValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(3), Is.EqualTo(ContentTypeEnum.PDF.ToString()));
            
            Assert.That(contentValuesToChoiceList.ElementAt(1).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
            Assert.That(contentValuesToChoiceList.ElementAt(1).RequiredValues!.Count(), Is.EqualTo(1));
            Assert.That(contentValuesToChoiceList.ElementAt(1).RequiredValues, Contains.Key("Type"));
            Assert.That(contentValuesToChoiceList.ElementAt(1).RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Activation.ToString()));
                
            Assert.That(contentValuesToChoiceList.ElementAt(1).AvailableChoices.Count(), Is.EqualTo(2));
            Assert.That(contentValuesToChoiceList.ElementAt(1).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
            Assert.That(contentValuesToChoiceList.ElementAt(1).AvailableChoices.ElementAt(1), Is.EqualTo(ContentTypeEnum.Video.ToString()));
            
            Assert.That(contentValuesToChoiceList.ElementAt(2).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
            Assert.That(contentValuesToChoiceList.ElementAt(2).RequiredValues!.Count(), Is.EqualTo(1));
            Assert.That(contentValuesToChoiceList.ElementAt(2).RequiredValues, Contains.Key("Type"));
            Assert.That(contentValuesToChoiceList.ElementAt(2).RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Interaction.ToString()));
                
            Assert.That(contentValuesToChoiceList.ElementAt(2).AvailableChoices.Count(), Is.EqualTo(1));
            Assert.That(contentValuesToChoiceList.ElementAt(2).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
            
            Assert.That(contentValuesToChoiceList.ElementAt(3).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
            Assert.That(contentValuesToChoiceList.ElementAt(3).RequiredValues!.Count(), Is.EqualTo(1));
            Assert.That(contentValuesToChoiceList.ElementAt(3).RequiredValues, Contains.Key("Type"));
            Assert.That(contentValuesToChoiceList.ElementAt(3).RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Test.ToString()));
                
            Assert.That(contentValuesToChoiceList.ElementAt(3).AvailableChoices.Count(), Is.EqualTo(1));
            Assert.That(contentValuesToChoiceList.ElementAt(3).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
            
            Assert.That(modalDialogInputFields.ElementAt(4).Name, Is.EqualTo("Url"));
            Assert.That(modalDialogInputFields.ElementAt(4).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(4).Required, Is.False);

            Assert.That(modalDialogInputFields.ElementAt(5).Name, Is.EqualTo("Authors"));
            Assert.That(modalDialogInputFields.ElementAt(5).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(5).Required, Is.False);

            Assert.That(modalDialogInputFields.ElementAt(6).Name, Is.EqualTo("Description"));
            Assert.That(modalDialogInputFields.ElementAt(6).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(6).Required, Is.False);

            Assert.That(modalDialogInputFields.ElementAt(7).Name, Is.EqualTo("Goals"));
            Assert.That(modalDialogInputFields.ElementAt(7).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(7).Required, Is.False);

            //difficulty
            var difficultyDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(8);
            Assert.That(difficultyDropDownInput.Name, Is.EqualTo("Difficulty"));
            Assert.That(difficultyDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(difficultyDropDownInput.Required, Is.False);
            Assert.That(difficultyDropDownInput, Is.TypeOf<ModalDialogDropdownInputField>());
            
            var difficultyValuesToChoiceList = difficultyDropDownInput.ValuesToChoiceMapping.ToList();
            Assert.That(difficultyValuesToChoiceList.Count(), Is.EqualTo(1));
            Assert.That(difficultyValuesToChoiceList.ElementAt(0).RequiredValues, Is.Null);
                
            Assert.That(difficultyValuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(4));
            Assert.That(difficultyValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ElementDifficultyEnum.None.ToString()));
            Assert.That(difficultyValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(1), Is.EqualTo(ElementDifficultyEnum.Easy.ToString()));
            Assert.That(difficultyValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(2), Is.EqualTo(ElementDifficultyEnum.Medium.ToString()));
            Assert.That(difficultyValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(3), Is.EqualTo(ElementDifficultyEnum.Hard.ToString()));
            

            Assert.That(modalDialogInputFields.ElementAt(9).Name, Is.EqualTo("Workload (min)"));
            Assert.That(modalDialogInputFields.ElementAt(9).Type, Is.EqualTo(ModalDialogInputType.Number));
            Assert.That(modalDialogInputFields.ElementAt(9).Required, Is.False);
        
    }

    [TestCase("abc")]
    [TestCase("xyz")]
    [TestCase("pqr")]
    [TestCase("baz")]
    public void GetCreateElementInputFields_ThrowsException_ForWrongFileExtension(string wrongFileExtensionForTest)
    {
        var name = "foo";
        var dragAndDropContent =
            new ContentViewModel(
                name,
                wrongFileExtensionForTest,
                ""
            );
        var spaceName = "skazzle";
        var systemUnderTest = GetModalDialogInputFieldsFactoryForTesting();
        Assert.Throws<Exception>(()=>systemUnderTest.GetCreateElementInputFields(dragAndDropContent),
            $"Can not map the file extension '{wrongFileExtensionForTest}' to a ContentType ");
    }

    [TestCase("jpg", ContentTypeEnum.Image)]
    [TestCase("png", ContentTypeEnum.Image)]
    [TestCase("webp", ContentTypeEnum.Image)]
    [TestCase("bmp", ContentTypeEnum.Image)]
    [TestCase("txt", ContentTypeEnum.Text)]
    [TestCase("c", ContentTypeEnum.Text)]
    [TestCase("h", ContentTypeEnum.Text)]
    [TestCase("cpp", ContentTypeEnum.Text)]
    [TestCase("cc", ContentTypeEnum.Text)]
    [TestCase("c++", ContentTypeEnum.Text)]
    [TestCase("py", ContentTypeEnum.Text)]
    [TestCase("cs", ContentTypeEnum.Text)]
    [TestCase("js", ContentTypeEnum.Text)]
    [TestCase("php", ContentTypeEnum.Text)]
    [TestCase("html", ContentTypeEnum.Text)]
    [TestCase("css", ContentTypeEnum.Text)]
    [TestCase("h5p", ContentTypeEnum.H5P)]
    [TestCase("pdf", ContentTypeEnum.PDF)]
    public void GetCreateElementInputFields_OverloadedFunction_ValidDragAndDrop_ReturnsCorrectInputFields_ForCorrectFileExtensionAndCorrectFileType(
        string correctFileExtensionForTest, 
        ContentTypeEnum correctFileTypeForTest)
    {
        string nameContentViewModel = "skazzle";
            ContentViewModel dragAndDropContent = new ContentViewModel(
                nameContentViewModel,
                correctFileExtensionForTest,
                ""
            );
            
            string nameSpace = "foo";
            string shortnameSpace = "baz";
            string authorsSpace = "boo";
            string descriptionSpace = "maz";
            string goalsSpace = "moo";
            int requiredPointsSpace = 10;
            IElementViewModel?[] elements = Array.Empty<IElementViewModel?>();
            var spaceLayoutVm = new SpaceLayoutViewModel(){Elements = elements};
            double positionXSpace = 1D;
            double positionYSpace = 2D;
            
            ISpaceViewModel space = 
                new SpaceViewModel(
                    nameSpace,
                    shortnameSpace,
                    authorsSpace,
                    descriptionSpace, 
                    goalsSpace,
                    requiredPointsSpace,
                    spaceLayoutVm,
                    positionXSpace,
                    positionYSpace
                );
            IEnumerable<ISpaceViewModel> spaces = new[]{space};
            
        string worldName = "bazzle";
        
        
        var systemUnderTest = GetModalDialogInputFieldsFactoryForTesting();
        var modalDialogInputFields = systemUnderTest.GetCreateElementInputFields(dragAndDropContent).ToList();
        
        //Name
        var nameInputField = modalDialogInputFields.ElementAt(0);
        Assert.That(nameInputField.Name, Is.EqualTo("Name"));
        Assert.That(nameInputField.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(nameInputField.Required, Is.True);

        var shortnameInputField = modalDialogInputFields.ElementAt(1);
        Assert.That(shortnameInputField.Name, Is.EqualTo("Shortname"));
        Assert.That(shortnameInputField.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(shortnameInputField.Required, Is.False);


        //Type drop down
        Assert.That(modalDialogInputFields.ElementAt(2), Is.TypeOf<ModalDialogDropdownInputField>());
        var typeDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(2);
        
        Assert.That(typeDropDownInput.Name, Is.EqualTo("Type"));
        Assert.That(typeDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(typeDropDownInput.Required, Is.True);
        Assert.That(typeDropDownInput.ValuesToChoiceMapping.Count(), Is.EqualTo(1));
        var typeChoiceMapping = typeDropDownInput.ValuesToChoiceMapping.ElementAt(0);
        Assert.That(typeChoiceMapping.RequiredValues, Is.Null);
        switch (correctFileTypeForTest)
        {
            case ContentTypeEnum.Image:
            case ContentTypeEnum.Text:
            case ContentTypeEnum.PDF:
            {
                Assert.That(typeChoiceMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(typeChoiceMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                break;
            }
            case ContentTypeEnum.Video:
            {
                Assert.That(typeChoiceMapping.AvailableChoices.Count(), Is.EqualTo(2));
                Assert.That(typeChoiceMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                Assert.That(typeChoiceMapping.AvailableChoices.ElementAt(1), Is.EqualTo(ElementTypeEnum.Activation.ToString()));
                break;
            }
            case ContentTypeEnum.H5P:
            {
                Assert.That(typeChoiceMapping.AvailableChoices.Count(), Is.EqualTo(3));
                Assert.That(typeChoiceMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ElementTypeEnum.Activation.ToString()));
                Assert.That(typeChoiceMapping.AvailableChoices.ElementAt(1), Is.EqualTo(ElementTypeEnum.Interaction.ToString()));
                Assert.That(typeChoiceMapping.AvailableChoices.ElementAt(2), Is.EqualTo(ElementTypeEnum.Test.ToString()));
                break;
            }
            default:
            {
                Assert.Fail("Invalid content type");
                break;
            }
        }

        //Content drop down
        Assert.That(modalDialogInputFields.ElementAt(3), Is.TypeOf<ModalDialogDropdownInputField>());
        var contentDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(3);
        Assert.That(contentDropDownInput.Name, Is.EqualTo("Content"));
        Assert.That(contentDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(contentDropDownInput.Required, Is.True);
        var contentChoiceMappings = contentDropDownInput.ValuesToChoiceMapping.ToList();
        switch (correctFileTypeForTest)
        {
            case ContentTypeEnum.Image:
            {
                Assert.That(contentChoiceMappings.Count, Is.EqualTo(1));

                var firstMapping = contentChoiceMappings.First();
                Assert.That(firstMapping.RequiredValues, Is.Not.Null);
                Assert.That(firstMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(firstMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(firstMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
            
                Assert.That(firstMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(firstMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Image.ToString()));
                break;
            }
            case ContentTypeEnum.Video:
            {
                Assert.That(contentChoiceMappings.Count, Is.EqualTo(2));
            
                var firstMapping = contentChoiceMappings.First();
                var secondMapping = contentChoiceMappings.ElementAt(1);
                Assert.That(firstMapping.RequiredValues, Is.Not.Null);
                Assert.That(firstMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(firstMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(firstMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
            
                Assert.That(firstMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(firstMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Video.ToString()));
            
                Assert.That(secondMapping.RequiredValues, Is.Not.Null);
                Assert.That(secondMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(secondMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(secondMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Activation.ToString()));
            
                Assert.That(secondMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(secondMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Video.ToString()));
                break;
            }
            case ContentTypeEnum.PDF:
            {
                Assert.That(contentChoiceMappings.Count, Is.EqualTo(1));
            
                var firstMapping = contentChoiceMappings.First();
                Assert.That(firstMapping.RequiredValues, Is.Not.Null);
                Assert.That(firstMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(firstMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(firstMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
            
                Assert.That(firstMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(firstMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.PDF.ToString()));
                break;
            }
            case ContentTypeEnum.H5P:
            {
                Assert.That(contentChoiceMappings.Count, Is.EqualTo(3));
            
                var firstMapping = contentChoiceMappings.First();
                var secondMapping = contentChoiceMappings.ElementAt(1);
                var thirdMapping = contentChoiceMappings.ElementAt(2);
                Assert.That(firstMapping.RequiredValues, Is.Not.Null);
                Assert.That(firstMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(firstMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(firstMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Activation.ToString()));
            
                Assert.That(firstMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(firstMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
            
            
                Assert.That(secondMapping.RequiredValues, Is.Not.Null);
                Assert.That(secondMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(secondMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(secondMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Interaction.ToString()));
            
                Assert.That(secondMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(secondMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
            
            
                Assert.That(thirdMapping.RequiredValues, Is.Not.Null);
                Assert.That(thirdMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(thirdMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(thirdMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Test.ToString()));
            
                Assert.That(thirdMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(thirdMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
                break;
            }
        }

        Assert.That(modalDialogInputFields.ElementAt(4).Name, Is.EqualTo("Url"));
        Assert.That(modalDialogInputFields.ElementAt(4).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(4).Required, Is.False);
        
        Assert.That(modalDialogInputFields.ElementAt(5).Name, Is.EqualTo("Authors"));
        Assert.That(modalDialogInputFields.ElementAt(5).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(5).Required, Is.False);

        Assert.That(modalDialogInputFields.ElementAt(6).Name, Is.EqualTo("Description"));
        Assert.That(modalDialogInputFields.ElementAt(6).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(6).Required, Is.False);

        Assert.That(modalDialogInputFields.ElementAt(7).Name, Is.EqualTo("Goals"));
        Assert.That(modalDialogInputFields.ElementAt(7).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(7).Required, Is.False);

        //difficulty
        Assert.That(modalDialogInputFields.ElementAt(8), Is.TypeOf<ModalDialogDropdownInputField>());
        var difficultyDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(8);
        Assert.That(difficultyDropDownInput.Name, Is.EqualTo("Difficulty"));
        Assert.That(difficultyDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(difficultyDropDownInput.Required, Is.False);
        var difficultyChoiceMappings = difficultyDropDownInput.ValuesToChoiceMapping.ToList();
        
        Assert.That(difficultyChoiceMappings.Count, Is.EqualTo(1));
        var difficultyMapping = difficultyChoiceMappings.First();
        
        Assert.That(difficultyMapping.RequiredValues, Is.Null);
        Assert.That(difficultyMapping.AvailableChoices.Count(), Is.EqualTo(4));
        Assert.That(difficultyMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ElementDifficultyEnum.None.ToString()));
        Assert.That(difficultyMapping.AvailableChoices.ElementAt(1), Is.EqualTo(ElementDifficultyEnum.Easy.ToString()));
        Assert.That(difficultyMapping.AvailableChoices.ElementAt(2), Is.EqualTo(ElementDifficultyEnum.Medium.ToString()));
        Assert.That(difficultyMapping.AvailableChoices.ElementAt(3), Is.EqualTo(ElementDifficultyEnum.Hard.ToString()));

        Assert.That(modalDialogInputFields.ElementAt(9).Name, Is.EqualTo("Workload (min)"));
        Assert.That(modalDialogInputFields.ElementAt(9).Type, Is.EqualTo(ModalDialogInputType.Number));
        Assert.That(modalDialogInputFields.ElementAt(9).Required, Is.False);

    }
    

    [TestCase("jpg", ContentTypeEnum.Image)]
    [TestCase("png", ContentTypeEnum.Image)]
    [TestCase("webp", ContentTypeEnum.Image)]
    [TestCase("bmp", ContentTypeEnum.Image)]
    [TestCase("txt", ContentTypeEnum.Text)]
    [TestCase("c", ContentTypeEnum.Text)]
    [TestCase("h", ContentTypeEnum.Text)]
    [TestCase("cpp", ContentTypeEnum.Text)]
    [TestCase("cc", ContentTypeEnum.Text)]
    [TestCase("c++", ContentTypeEnum.Text)]
    [TestCase("py", ContentTypeEnum.Text)]
    [TestCase("cs", ContentTypeEnum.Text)]
    [TestCase("js", ContentTypeEnum.Text)]
    [TestCase("php", ContentTypeEnum.Text)]
    [TestCase("html", ContentTypeEnum.Text)]
    [TestCase("css", ContentTypeEnum.Text)]
    [TestCase("mp4", ContentTypeEnum.Video)]
    [TestCase("h5p", ContentTypeEnum.H5P)]
    [TestCase("pdf", ContentTypeEnum.PDF)]
    public void GetCreateElementInputFields_OverloadedFunction_NoDragAndDrop_ReturnsCorrectInputFields(string correctFileExtensionForTest, 
        ContentTypeEnum correctFileTypeForTest)
    {
        ContentViewModel? dragAndDropContent = null;
            
        string nameSpace = "foo";
        string shortnameSpace = "baz";
        string authorsSpace = "boo";
        string descriptionSpace = "maz";
        string goalsSpace = "moo";
        int requiredPointsSpace = 10;
        IElementViewModel?[] elements = Array.Empty<IElementViewModel?>();
        var spaceLayoutVm = new SpaceLayoutViewModel(){Elements = elements};
        double positionXSpace = 1D;
        double positionYSpace = 2D;
            
        ISpaceViewModel space = 
            new SpaceViewModel(
                nameSpace,
                shortnameSpace,
                authorsSpace,
                descriptionSpace, 
                goalsSpace,
                requiredPointsSpace,
                spaceLayoutVm,
                positionXSpace,
                positionYSpace
            );
        IEnumerable<ISpaceViewModel> spaces = new[]{space};
            
        string worldName = "bazzle";
        
        
        var systemUnderTest = GetModalDialogInputFieldsFactoryForTesting();
        var modalDialogInputFields = systemUnderTest.GetCreateElementInputFields(dragAndDropContent).ToList();
        
        Assert.That(modalDialogInputFields.ElementAt(0).Name, Is.EqualTo("Name"));
        Assert.That(modalDialogInputFields.ElementAt(0).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(0).Required, Is.True);

        Assert.That(modalDialogInputFields.ElementAt(1).Name, Is.EqualTo("Shortname"));
        Assert.That(modalDialogInputFields.ElementAt(1).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(1).Required, Is.False);

        //type field: solved
        var typeDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(2);
        Assert.That(typeDropDownInput.Name, Is.EqualTo("Type"));
        Assert.That(typeDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(typeDropDownInput.Required, Is.True);
        Assert.That(typeDropDownInput, Is.TypeOf<ModalDialogDropdownInputField>());
        
        var typeValuesToChoiceList = typeDropDownInput.ValuesToChoiceMapping.ToList();
        Assert.That(typeValuesToChoiceList.Count, Is.EqualTo(1));
        Assert.That(typeValuesToChoiceList.ElementAt(0), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
        Assert.That(typeValuesToChoiceList.ElementAt(0).RequiredValues, Is.Null);
        Assert.That(typeValuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(4));
        
        Assert.That(typeValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
        Assert.That(typeValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(1), Is.EqualTo(ElementTypeEnum.Activation.ToString()));
        Assert.That(typeValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(2), Is.EqualTo(ElementTypeEnum.Interaction.ToString()));
        Assert.That(typeValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(3), Is.EqualTo(ElementTypeEnum.Test.ToString()));
        
            

        //content field: solved
        var contentDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(3);
        Assert.That(contentDropDownInput.Name, Is.EqualTo("Content"));
        Assert.That(contentDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(contentDropDownInput.Required, Is.True);
        Assert.That(contentDropDownInput, Is.TypeOf<ModalDialogDropdownInputField>());
        
        var contentValuesToChoiceList = contentDropDownInput.ValuesToChoiceMapping.ToList();
                
        Assert.That(contentValuesToChoiceList.Count(), Is.EqualTo(4));
        Assert.That(contentValuesToChoiceList.ElementAt(0), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
        Assert.That(contentValuesToChoiceList.ElementAt(1), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
        Assert.That(contentValuesToChoiceList.ElementAt(2), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
        Assert.That(contentValuesToChoiceList.ElementAt(3), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
                
                
        Assert.That(contentValuesToChoiceList.ElementAt(0).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
        Assert.That(contentValuesToChoiceList.ElementAt(0).RequiredValues!.Count(), Is.EqualTo(1));
        Assert.That(contentValuesToChoiceList.ElementAt(0).RequiredValues, Contains.Key("Type"));
        Assert.That(contentValuesToChoiceList.ElementAt(0).RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                
        Assert.That(contentValuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(4));
        Assert.That(contentValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Image.ToString()));
        Assert.That(contentValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(1), Is.EqualTo(ContentTypeEnum.Text.ToString()));
        Assert.That(contentValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(2), Is.EqualTo(ContentTypeEnum.Video.ToString()));
        Assert.That(contentValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(3), Is.EqualTo(ContentTypeEnum.PDF.ToString()));
                
                
        Assert.That(contentValuesToChoiceList.ElementAt(1).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
        Assert.That(contentValuesToChoiceList.ElementAt(1).RequiredValues!.Count(), Is.EqualTo(1));
        Assert.That(contentValuesToChoiceList.ElementAt(1).RequiredValues, Contains.Key("Type"));
        Assert.That(contentValuesToChoiceList.ElementAt(1).RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Activation.ToString()));
                
        Assert.That(contentValuesToChoiceList.ElementAt(1).AvailableChoices.Count(), Is.EqualTo(2));
        Assert.That(contentValuesToChoiceList.ElementAt(1).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
        Assert.That(contentValuesToChoiceList.ElementAt(1).AvailableChoices.ElementAt(1), Is.EqualTo(ContentTypeEnum.Video.ToString()));

                
        Assert.That(contentValuesToChoiceList.ElementAt(2).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
        Assert.That(contentValuesToChoiceList.ElementAt(2).RequiredValues!.Count(), Is.EqualTo(1));
        Assert.That(contentValuesToChoiceList.ElementAt(2).RequiredValues, Contains.Key("Type"));
        Assert.That(contentValuesToChoiceList.ElementAt(2).RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Interaction.ToString()));
                
        Assert.That(contentValuesToChoiceList.ElementAt(2).AvailableChoices.Count(), Is.EqualTo(1));
        Assert.That(contentValuesToChoiceList.ElementAt(2).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));

                
        Assert.That(contentValuesToChoiceList.ElementAt(3).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
        Assert.That(contentValuesToChoiceList.ElementAt(3).RequiredValues!.Count(), Is.EqualTo(1));
        Assert.That(contentValuesToChoiceList.ElementAt(3).RequiredValues, Contains.Key("Type"));
        Assert.That(contentValuesToChoiceList.ElementAt(3).RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Test.ToString()));
                
        Assert.That(contentValuesToChoiceList.ElementAt(3).AvailableChoices.Count(), Is.EqualTo(1));
        Assert.That(contentValuesToChoiceList.ElementAt(3).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
        
        Assert.That(modalDialogInputFields.ElementAt(4).Name, Is.EqualTo("Url"));
        Assert.That(modalDialogInputFields.ElementAt(4).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(4).Required, Is.False);    
            
        Assert.That(modalDialogInputFields.ElementAt(5).Name, Is.EqualTo("Authors"));
        Assert.That(modalDialogInputFields.ElementAt(5).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(5).Required, Is.False);

        Assert.That(modalDialogInputFields.ElementAt(6).Name, Is.EqualTo("Description"));
        Assert.That(modalDialogInputFields.ElementAt(6).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(6).Required, Is.False);

        Assert.That(modalDialogInputFields.ElementAt(7).Name, Is.EqualTo("Goals"));
        Assert.That(modalDialogInputFields.ElementAt(7).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(7).Required, Is.False);

        //difficulty
        var difficultyDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(8);
        Assert.That(difficultyDropDownInput.Name, Is.EqualTo("Difficulty"));
        Assert.That(difficultyDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(difficultyDropDownInput.Required, Is.False);
        Assert.That(difficultyDropDownInput, Is.TypeOf<ModalDialogDropdownInputField>());
        
        var difficultyValuesToChoiceList = difficultyDropDownInput.ValuesToChoiceMapping.ToList();
        Assert.That(difficultyValuesToChoiceList.Count(), Is.EqualTo(1));
        Assert.That(difficultyValuesToChoiceList.ElementAt(0).RequiredValues, Is.Null);
                
        Assert.That(difficultyValuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(4));
        Assert.That(difficultyValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ElementDifficultyEnum.None.ToString()));
        Assert.That(difficultyValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(1), Is.EqualTo(ElementDifficultyEnum.Easy.ToString()));
        Assert.That(difficultyValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(2), Is.EqualTo(ElementDifficultyEnum.Medium.ToString()));
        Assert.That(difficultyValuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(3), Is.EqualTo(ElementDifficultyEnum.Hard.ToString()));
        

        Assert.That(modalDialogInputFields.ElementAt(9).Name, Is.EqualTo("Workload (min)"));
        Assert.That(modalDialogInputFields.ElementAt(9).Type, Is.EqualTo(ModalDialogInputType.Number));
        Assert.That(modalDialogInputFields.ElementAt(9).Required, Is.False);
    }
    
    [TestCase("abc")]
    [TestCase("xyz")]
    [TestCase("pqw")]
    [TestCase("baz")]
    public void GetCreateElementInputFields_OverloadedFunction_ThrowsException_ForWrongFileExtension(string wrongFileExtensionForTest)
    {
        var name = "foo";
        var dragAndDropContent =
            new ContentViewModel(
                name,
                wrongFileExtensionForTest,
                ""
            );
            
        string nameSpace = "skazzle";
        string shortnameSpace = "b";
        string authorsSpace = "boo";
        string descriptionSpace = "maz";
        string goalsSpace = "moo";
        int requiredPointsSpace = 10;
        IElementViewModel?[] elements = Array.Empty<IElementViewModel?>();
        var spaceLayoutVm = new SpaceLayoutViewModel(){Elements = elements};
        double positionXSpace = 1D;
        double positionYSpace = 2D;
            
        ISpaceViewModel space = 
            new SpaceViewModel(
                nameSpace,
                shortnameSpace,
                authorsSpace,
                descriptionSpace, 
                goalsSpace,
                requiredPointsSpace,
                spaceLayoutVm,
                positionXSpace,
                positionYSpace
            );
        IEnumerable<ISpaceViewModel> spaces = new[]{space};
            
        string worldName = "bazzle";
        var systemUnderTest = GetModalDialogInputFieldsFactoryForTesting();
        Assert.Throws<Exception>(()=>systemUnderTest.GetCreateElementInputFields(dragAndDropContent), $"Can not map the file extension '{wrongFileExtensionForTest}' to a ContentType ");
    }
    
    [Test]
    public void GetEditSpaceInputFields_ReturnsCorrectInputFields()
    {
        var systemUnderTest = new ModalDialogInputFieldsFactory();

        var name = "Name";
        var shortname = "Shortname";
        var authors = "Authors";
        var description = "Description";
        var goals = "Goals";

        var modalDialogInputType = ModalDialogInputType.Text;

        var modalDialogInputFields = systemUnderTest.GetEditSpaceInputFields().ToList();
        Assert.Multiple(() =>
        {
            Assert.That(modalDialogInputFields.ElementAt(0).Name, Is.EqualTo(name));
            Assert.That(modalDialogInputFields.ElementAt(0).Type, Is.EqualTo(modalDialogInputType));
            Assert.That(modalDialogInputFields.ElementAt(0).Required, Is.EqualTo(true));

            Assert.That(modalDialogInputFields.ElementAt(1).Name, Is.EqualTo(shortname));
            Assert.That(modalDialogInputFields.ElementAt(1).Type, Is.EqualTo(modalDialogInputType));
            Assert.That(modalDialogInputFields.ElementAt(1).Required, Is.EqualTo(false));

            Assert.That(modalDialogInputFields.ElementAt(2).Name, Is.EqualTo(authors));
            Assert.That(modalDialogInputFields.ElementAt(2).Type, Is.EqualTo(modalDialogInputType));
            Assert.That(modalDialogInputFields.ElementAt(2).Required, Is.EqualTo(false));

            Assert.That(modalDialogInputFields.ElementAt(3).Name, Is.EqualTo(description));
            Assert.That(modalDialogInputFields.ElementAt(3).Type, Is.EqualTo(modalDialogInputType));
            Assert.That(modalDialogInputFields.ElementAt(3).Required, Is.EqualTo(false));

            Assert.That(modalDialogInputFields.ElementAt(4).Name, Is.EqualTo(goals));
            Assert.That(modalDialogInputFields.ElementAt(4).Type, Is.EqualTo(modalDialogInputType));
            Assert.That(modalDialogInputFields.ElementAt(4).Required, Is.EqualTo(false));
        });
    }

    [Test]
    public void GetEditElementInputFields_ReturnsCorrectInputFields()
    {
        var systemUnderTest = GetModalDialogInputFieldsFactoryForTesting();

        var modalDialogInputFields = systemUnderTest.GetEditElementInputFields().ToList();
        
        Assert.That(modalDialogInputFields, Has.Count.EqualTo(9));
        Assert.Multiple(() =>
        {
            Assert.That(modalDialogInputFields.ElementAt(0).Name, Is.EqualTo("Name"));
            Assert.That(modalDialogInputFields.ElementAt(0).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(0).Required, Is.EqualTo(true));

            Assert.That(modalDialogInputFields.ElementAt(1).Name, Is.EqualTo("Shortname"));
            Assert.That(modalDialogInputFields.ElementAt(1).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(1).Required, Is.EqualTo(false));

            Assert.That(modalDialogInputFields.ElementAt(2).Name, Is.EqualTo("Url"));
            Assert.That(modalDialogInputFields.ElementAt(2).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(2).Required, Is.False);
            
            Assert.That(modalDialogInputFields.ElementAt(3).Name, Is.EqualTo("Authors"));
            Assert.That(modalDialogInputFields.ElementAt(3).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(3).Required, Is.EqualTo(false));

            Assert.That(modalDialogInputFields.ElementAt(4).Name, Is.EqualTo("Description"));
            Assert.That(modalDialogInputFields.ElementAt(4).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(4).Required, Is.EqualTo(false));

            Assert.That(modalDialogInputFields.ElementAt(5).Name, Is.EqualTo("Goals"));
            Assert.That(modalDialogInputFields.ElementAt(5).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(5).Required, Is.EqualTo(false));
            
            Assert.That(modalDialogInputFields.ElementAt(6), Is.TypeOf<ModalDialogDropdownInputField>());
            var difficultyDropDownInput = (ModalDialogDropdownInputField) modalDialogInputFields.ElementAt(6);
            Assert.That(difficultyDropDownInput.Name, Is.EqualTo("Difficulty"));
            Assert.That(difficultyDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(difficultyDropDownInput.Required, Is.EqualTo(true));
            Assert.That(difficultyDropDownInput.ValuesToChoiceMapping.Count(), Is.EqualTo(1));

            var mapping = difficultyDropDownInput.ValuesToChoiceMapping.First();
            Assert.That(mapping.RequiredValues, Is.Null);
            Assert.That(mapping.AvailableChoices.ElementAt(0), Is.EqualTo(ElementDifficultyEnum.Easy.ToString()));
            Assert.That(mapping.AvailableChoices.ElementAt(1), Is.EqualTo(ElementDifficultyEnum.Medium.ToString()));
            Assert.That(mapping.AvailableChoices.ElementAt(2), Is.EqualTo(ElementDifficultyEnum.Hard.ToString()));
            Assert.That(mapping.AvailableChoices.ElementAt(3), Is.EqualTo(ElementDifficultyEnum.None.ToString()));
            
            Assert.That(modalDialogInputFields.ElementAt(7).Name, Is.EqualTo("Workload (min)"));
            Assert.That(modalDialogInputFields.ElementAt(7).Type, Is.EqualTo(ModalDialogInputType.Number));
            Assert.That(modalDialogInputFields.ElementAt(7).Required, Is.EqualTo(false));
            
            Assert.That(modalDialogInputFields.ElementAt(7).Name, Is.EqualTo("Workload (min)"));
            Assert.That(modalDialogInputFields.ElementAt(7).Type, Is.EqualTo(ModalDialogInputType.Number));
            Assert.That(modalDialogInputFields.ElementAt(7).Required, Is.EqualTo(false));
            
            Assert.That(modalDialogInputFields.ElementAt(8).Name, Is.EqualTo("Points"));
            Assert.That(modalDialogInputFields.ElementAt(8).Type, Is.EqualTo(ModalDialogInputType.Number));
            Assert.That(modalDialogInputFields.ElementAt(8).Required, Is.EqualTo(false));
        });
    }
    
    [Test]
    public void GetEditPathWayConditionInputFields_ReturnsCorrectInputFields()
    {
        var systemUnderTest = new ModalDialogInputFieldsFactory();

        var modalDialogInputFields = systemUnderTest.GetEditPathWayConditionInputFields().ToList();
        
        Assert.That(modalDialogInputFields.ElementAt(0), Is.TypeOf<ModalDialogDropdownInputField>());
        var conditionDropdownInput = (ModalDialogDropdownInputField) modalDialogInputFields.ElementAt(0);
        Assert.That(conditionDropdownInput.Name, Is.EqualTo("Condition"));
        Assert.That(conditionDropdownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(conditionDropdownInput.Required, Is.EqualTo(true));
        Assert.That(conditionDropdownInput.ValuesToChoiceMapping.Count(), Is.EqualTo(1));
        
        var mapping = conditionDropdownInput.ValuesToChoiceMapping.First();
        Assert.That(mapping.RequiredValues, Is.Null);
        Assert.That(mapping.AvailableChoices.ElementAt(0), Is.EqualTo(ConditionEnum.And.ToString()));
        Assert.That(mapping.AvailableChoices.ElementAt(1), Is.EqualTo(ConditionEnum.Or.ToString()));
    }
    
    [Test]
    public void GetCreateWorldInputFields_ReturnsCorrectInputFields()
    {
        var systemUnderTest = GetModalDialogInputFieldsFactoryForTesting();
        var modalDialogInputFields = systemUnderTest.GetCreateWorldInputFields().ToList();
        
        Assert.That(systemUnderTest.GetCreateWorldInputFields(), Is.TypeOf<ModalDialogInputField[]>());
        Assert.That(modalDialogInputFields.Count(), Is.EqualTo(6));
        
        Assert.That(modalDialogInputFields.ElementAt(0), Is.TypeOf<ModalDialogInputField>());
        Assert.That(modalDialogInputFields.ElementAt(0).Name, Is.EqualTo("Name"));
        Assert.That(modalDialogInputFields.ElementAt(0).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(0).Required, Is.True);
        
        Assert.That(modalDialogInputFields.ElementAt(1), Is.TypeOf<ModalDialogInputField>());
        Assert.That(modalDialogInputFields.ElementAt(1).Name, Is.EqualTo("Shortname"));
        Assert.That(modalDialogInputFields.ElementAt(1).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(1).Required, Is.False);
        
        Assert.That(modalDialogInputFields.ElementAt(2), Is.TypeOf<ModalDialogInputField>());
        Assert.That(modalDialogInputFields.ElementAt(2).Name, Is.EqualTo("Authors"));
        Assert.That(modalDialogInputFields.ElementAt(2).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(2).Required, Is.False);
        
        Assert.That(modalDialogInputFields.ElementAt(3), Is.TypeOf<ModalDialogInputField>());
        Assert.That(modalDialogInputFields.ElementAt(3).Name, Is.EqualTo("Language"));
        Assert.That(modalDialogInputFields.ElementAt(3).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(3).Required, Is.False);
        
        Assert.That(modalDialogInputFields.ElementAt(4), Is.TypeOf<ModalDialogInputField>());
        Assert.That(modalDialogInputFields.ElementAt(4).Name, Is.EqualTo("Description"));
        Assert.That(modalDialogInputFields.ElementAt(4).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(4).Required, Is.False);
        
        Assert.That(modalDialogInputFields.ElementAt(5), Is.TypeOf<ModalDialogInputField>());
        Assert.That(modalDialogInputFields.ElementAt(5).Name, Is.EqualTo("Goals"));
        Assert.That(modalDialogInputFields.ElementAt(5).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(5).Required, Is.False);
    }

    [Test]
    public void GetEditWorldInputFields_ReturnsCorrectInputFields()
    {
        var systemUnderTest = GetModalDialogInputFieldsFactoryForTesting();
        var modalDialogInputFields = systemUnderTest.GetEditWorldInputFields().ToList();
        
        Assert.That(systemUnderTest.GetCreateWorldInputFields(), Is.TypeOf<ModalDialogInputField[]>());
        Assert.That(modalDialogInputFields.Count(), Is.EqualTo(6));
        
        Assert.That(modalDialogInputFields.ElementAt(0), Is.TypeOf<ModalDialogInputField>());
        Assert.That(modalDialogInputFields.ElementAt(0).Name, Is.EqualTo("Name"));
        Assert.That(modalDialogInputFields.ElementAt(0).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(0).Required, Is.True);
        
        Assert.That(modalDialogInputFields.ElementAt(1), Is.TypeOf<ModalDialogInputField>());
        Assert.That(modalDialogInputFields.ElementAt(1).Name, Is.EqualTo("Shortname"));
        Assert.That(modalDialogInputFields.ElementAt(1).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(1).Required, Is.False);
        
        Assert.That(modalDialogInputFields.ElementAt(2), Is.TypeOf<ModalDialogInputField>());
        Assert.That(modalDialogInputFields.ElementAt(2).Name, Is.EqualTo("Authors"));
        Assert.That(modalDialogInputFields.ElementAt(2).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(2).Required, Is.False);
        
        Assert.That(modalDialogInputFields.ElementAt(3), Is.TypeOf<ModalDialogInputField>());
        Assert.That(modalDialogInputFields.ElementAt(3).Name, Is.EqualTo("Language"));
        Assert.That(modalDialogInputFields.ElementAt(3).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(3).Required, Is.False);
        
        Assert.That(modalDialogInputFields.ElementAt(4), Is.TypeOf<ModalDialogInputField>());
        Assert.That(modalDialogInputFields.ElementAt(4).Name, Is.EqualTo("Description"));
        Assert.That(modalDialogInputFields.ElementAt(4).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(4).Required, Is.False);
        
        Assert.That(modalDialogInputFields.ElementAt(5), Is.TypeOf<ModalDialogInputField>());
        Assert.That(modalDialogInputFields.ElementAt(5).Name, Is.EqualTo("Goals"));
        Assert.That(modalDialogInputFields.ElementAt(5).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(5).Required, Is.False);
    }

    private ModalDialogInputFieldsFactory GetModalDialogInputFieldsFactoryForTesting()
    {
        return new ModalDialogInputFieldsFactory();
    }
}