using System;
using System.Collections.Generic;
using System.Linq;
using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.ModalDialog;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace AuthoringToolTest.PresentationLogic.ModalDialog;

[TestFixture]

public class ModalDialogInputFieldsFactoryUt
{
    [TestCase("jpg", ContentTypeEnum.Image)]
    [TestCase("png", ContentTypeEnum.Image)]
    [TestCase("webp", ContentTypeEnum.Image)]
    [TestCase("bmp", ContentTypeEnum.Image)]
    [TestCase("mp4", ContentTypeEnum.Video)]
    [TestCase("h5p", ContentTypeEnum.H5P)]
    [TestCase("pdf", ContentTypeEnum.Pdf)]
    public void GetCreateLearningElementInputFields_ValidDragAndDrop_ReturnsCorrectInputFields_ForCorrectFileExtensionAndCorrectFileType(string 
        correctFileExtensionForTest, ContentTypeEnum correctFileTypeForTest)
    {
        var name = "foo";
        var dragAndDropLearningContent =
            new LearningContentViewModel(
                name,
                correctFileExtensionForTest,
                new byte[] { }
            );
        var spaceName = "skazzle";

        var systemUnderTest = GetModalInputFieldsFactoryForTesting();


        var modalDialogInputFields =
            systemUnderTest.GetCreateLearningElementInputFields(dragAndDropLearningContent,
                spaceName).ToList();

        //Name
        var nameInputField = modalDialogInputFields.ElementAt(0);
        Assert.That(nameInputField.Name, Is.EqualTo("Name"));
        Assert.That(nameInputField.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(nameInputField.Required, Is.EqualTo(true));

        var shortnameInputField = modalDialogInputFields.ElementAt(1);
        Assert.That(shortnameInputField.Name, Is.EqualTo("Shortname"));
        Assert.That(shortnameInputField.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(shortnameInputField.Required, Is.EqualTo(true));

        
        
        //Parent drop down
        Assert.That(modalDialogInputFields.ElementAt(2), Is.TypeOf<ModalDialogDropdownInputField>());
        var parentDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(2);
        
        Assert.That(parentDropDownInput.Name, Is.EqualTo("Parent"));
        Assert.That(parentDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(parentDropDownInput.Required, Is.EqualTo(true));
        Assert.That(parentDropDownInput.ValuesToChoiceMapping.Count(), Is.EqualTo(1));
        
        //Parent drop down choice mappings
        var parentChoiceMapping = parentDropDownInput.ValuesToChoiceMapping.ElementAt(0);
        Assert.That(parentChoiceMapping.RequiredValues, Is.EqualTo(null));
        Assert.That(parentChoiceMapping.AvailableChoices.Count(), Is.EqualTo(1));
        Assert.That(parentChoiceMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ElementParentEnum.Space.ToString()));
        
        
        
        //Assignment dropdown
        Assert.That(modalDialogInputFields.ElementAt(3), Is.TypeOf<ModalDialogDropdownInputField>());
        var assignmentDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(3);
        
        Assert.That(assignmentDropDownInput.Name, Is.EqualTo("Assignment"));
        Assert.That(assignmentDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(assignmentDropDownInput.Required, Is.EqualTo(true));
        Assert.That(assignmentDropDownInput.ValuesToChoiceMapping.Count(), Is.EqualTo(1));
        
        //Assignment drop down choice mappings
        var assignmentChoiceMapping = assignmentDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0);
        Assert.That(assignmentChoiceMapping.RequiredValues, Is.Not.Null);
        Assert.That(assignmentChoiceMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
        Assert.That(assignmentChoiceMapping.RequiredValues, Contains.Key("Parent"));
        Assert.That(assignmentChoiceMapping.RequiredValues!["Parent"], Is.EqualTo(ElementParentEnum.Space.ToString()));
        Assert.That(assignmentChoiceMapping.AvailableChoices.Count(), Is.EqualTo(1));
        Assert.That(assignmentChoiceMapping.AvailableChoices.ElementAt(0), Is.EqualTo(spaceName));


        //Type drop down
        Assert.That(modalDialogInputFields.ElementAt(4), Is.TypeOf<ModalDialogDropdownInputField>());
        var typeDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(4);
        
        Assert.That(typeDropDownInput.Name, Is.EqualTo("Type"));
        Assert.That(typeDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(typeDropDownInput.Required, Is.EqualTo(true));
        Assert.That(typeDropDownInput.ValuesToChoiceMapping.Count(), Is.EqualTo(1));
        var typeChoiceMapping = typeDropDownInput.ValuesToChoiceMapping.ElementAt(0);
        Assert.That(typeChoiceMapping.RequiredValues, Is.EqualTo(null));
        switch (correctFileTypeForTest)
        {
            case ContentTypeEnum.Image:
            case ContentTypeEnum.Pdf:
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
        Assert.That(modalDialogInputFields.ElementAt(5), Is.TypeOf<ModalDialogDropdownInputField>());
        var contentDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(5);
        Assert.That(contentDropDownInput.Name, Is.EqualTo("Content"));
        Assert.That(contentDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(contentDropDownInput.Required, Is.EqualTo(true));
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
            case ContentTypeEnum.Pdf:
            {
                Assert.That(contentChoiceMappings.Count, Is.EqualTo(1));
            
                var firstMapping = contentChoiceMappings.First();
                Assert.That(firstMapping.RequiredValues, Is.Not.Null);
                Assert.That(firstMapping.RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(firstMapping.RequiredValues, Contains.Key("Type"));
                Assert.That(firstMapping.RequiredValues!["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
            
                Assert.That(firstMapping.AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(firstMapping.AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Pdf.ToString()));
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

        Assert.That(modalDialogInputFields.ElementAt(6).Name, Is.EqualTo("Authors"));
        Assert.That(modalDialogInputFields.ElementAt(6).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(6).Required, Is.EqualTo(false));

        Assert.That(modalDialogInputFields.ElementAt(7).Name, Is.EqualTo("Description"));
        Assert.That(modalDialogInputFields.ElementAt(7).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(7).Required, Is.EqualTo(true));

        Assert.That(modalDialogInputFields.ElementAt(8).Name, Is.EqualTo("Goals"));
        Assert.That(modalDialogInputFields.ElementAt(8).Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(modalDialogInputFields.ElementAt(8).Required, Is.EqualTo(false));

        //difficulty: solved
        Assert.That(modalDialogInputFields.ElementAt(9), Is.TypeOf<ModalDialogDropdownInputField>());
        var difficultyDropDownInput = (ModalDialogDropdownInputField)modalDialogInputFields.ElementAt(9);
        Assert.That(difficultyDropDownInput.Name, Is.EqualTo("Difficulty"));
        Assert.That(difficultyDropDownInput.Type, Is.EqualTo(ModalDialogInputType.Text));
        Assert.That(difficultyDropDownInput.Required, Is.EqualTo(true));
        var difficultyChoiceMappings = difficultyDropDownInput.ValuesToChoiceMapping.ToList();
        
        Assert.That(difficultyChoiceMappings.Count, Is.EqualTo(1));
        var difficultyMapping = difficultyChoiceMappings.First();
        
        Assert.That(difficultyMapping.RequiredValues, Is.EqualTo(null));
        Assert.That(difficultyMapping.AvailableChoices.Count(), Is.EqualTo(3));
        Assert.That(difficultyMapping.AvailableChoices.ElementAt(0), Is.EqualTo(LearningElementDifficultyEnum.Easy.ToString()));
        Assert.That(difficultyMapping.AvailableChoices.ElementAt(1), Is.EqualTo(LearningElementDifficultyEnum.Medium.ToString()));
        Assert.That(difficultyMapping.AvailableChoices.ElementAt(2), Is.EqualTo(LearningElementDifficultyEnum.Hard.ToString()));

        Assert.That(modalDialogInputFields.ElementAt(10).Name, Is.EqualTo("Workload (min)"));
        Assert.That(modalDialogInputFields.ElementAt(10).Type, Is.EqualTo(ModalDialogInputType.Number));
        Assert.That(modalDialogInputFields.ElementAt(10).Required, Is.EqualTo(false));
    
        
    }

    [TestCase("jpg", ContentTypeEnum.Image)]
    [TestCase("png", ContentTypeEnum.Image)]
    [TestCase("webp", ContentTypeEnum.Image)]
    [TestCase("bmp", ContentTypeEnum.Image)]
    [TestCase("mp4", ContentTypeEnum.Video)]
    [TestCase("h5p", ContentTypeEnum.H5P)]
    [TestCase("pdf", ContentTypeEnum.Pdf)]
    public void GetCreateLearningElementInputFields_ValidDragAndDrop_ReturnsCorrectInputFields_ForDefaultValue(string 
        correctFileExtensionForTest, ContentTypeEnum correctFileTypeForTest)
    {
        var systemUnderTest = GetModalInputFieldsFactoryForTesting();
        var spaceName = "skazzle";


        var modalDialogInputFields =
            systemUnderTest.GetCreateLearningElementInputFields(null,
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
            
            
            //assignment Field
            Assert.That(modalDialogInputFields.ElementAt(3).Name, Is.EqualTo("Assignment"));
            Assert.That(modalDialogInputFields.ElementAt(3).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(3).Required, Is.EqualTo(true));
            Assert.That(modalDialogInputFields.ElementAt(3), Is.TypeOf<ModalDialogDropdownInputField>());
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


            //type field: solved
            
            Assert.That(modalDialogInputFields.ElementAt(4).Name, Is.EqualTo("Type"));
            Assert.That(modalDialogInputFields.ElementAt(4).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(4).Required, Is.EqualTo(true));
            Assert.That(modalDialogInputFields.ElementAt(4), Is.TypeOf<ModalDialogDropdownInputField>());
            if (modalDialogInputFields.ElementAt(4) is ModalDialogDropdownInputField typeDropDownInput)
            {
                var valuesToChoiceList = typeDropDownInput.ValuesToChoiceMapping.ToList();
                Assert.That(valuesToChoiceList.Count, Is.EqualTo(1));
                Assert.That(
                    valuesToChoiceList.ElementAt(0),
                    Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>()
                );
                
                Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues, Is.EqualTo(null));
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(4));
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(1), Is.EqualTo(ElementTypeEnum.Activation.ToString()));
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(2), Is.EqualTo(ElementTypeEnum.Interaction.ToString()));
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(3), Is.EqualTo(ElementTypeEnum.Test.ToString()));
            }
            

            //content field: solved
            
            Assert.That(modalDialogInputFields.ElementAt(5).Name, Is.EqualTo("Content"));
            Assert.That(modalDialogInputFields.ElementAt(5).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(5).Required, Is.EqualTo(true));
            Assert.That(modalDialogInputFields.ElementAt(5), Is.TypeOf<ModalDialogDropdownInputField>());
            if (modalDialogInputFields.ElementAt(5) is ModalDialogDropdownInputField contentDropDownInput)
            {
                var valuesToChoiceList = contentDropDownInput.ValuesToChoiceMapping.ToList();
                
                Assert.That(valuesToChoiceList.Count(), Is.EqualTo(4));
                Assert.That(valuesToChoiceList.ElementAt(0), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
                Assert.That(valuesToChoiceList.ElementAt(1), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
                Assert.That(valuesToChoiceList.ElementAt(2), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
                Assert.That(valuesToChoiceList.ElementAt(3), Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>());
                
                
                Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues.Count(), Is.EqualTo(1));
                Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues, Contains.Key("Type"));
                Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(3));
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Image.ToString()));
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(1), Is.EqualTo(ContentTypeEnum.Video.ToString()));
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(2), Is.EqualTo(ContentTypeEnum.Pdf.ToString()));
                
                
                Assert.That(valuesToChoiceList.ElementAt(1).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(valuesToChoiceList.ElementAt(1).RequiredValues.Count(), Is.EqualTo(1));
                Assert.That(valuesToChoiceList.ElementAt(1).RequiredValues, Contains.Key("Type"));
                Assert.That(valuesToChoiceList.ElementAt(1).RequiredValues["Type"], Is.EqualTo(ElementTypeEnum.Activation.ToString()));
                
                Assert.That(valuesToChoiceList.ElementAt(1).AvailableChoices.Count(), Is.EqualTo(2));
                Assert.That(valuesToChoiceList.ElementAt(1).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
                Assert.That(valuesToChoiceList.ElementAt(1).AvailableChoices.ElementAt(1), Is.EqualTo(ContentTypeEnum.Video.ToString()));

                
                Assert.That(valuesToChoiceList.ElementAt(2).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(valuesToChoiceList.ElementAt(2).RequiredValues.Count(), Is.EqualTo(1));
                Assert.That(valuesToChoiceList.ElementAt(2).RequiredValues, Contains.Key("Type"));
                Assert.That(valuesToChoiceList.ElementAt(2).RequiredValues["Type"], Is.EqualTo(ElementTypeEnum.Interaction.ToString()));
                
                Assert.That(valuesToChoiceList.ElementAt(2).AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(valuesToChoiceList.ElementAt(2).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));

                
                Assert.That(valuesToChoiceList.ElementAt(3).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                Assert.That(valuesToChoiceList.ElementAt(3).RequiredValues.Count(), Is.EqualTo(1));
                Assert.That(valuesToChoiceList.ElementAt(3).RequiredValues, Contains.Key("Type"));
                Assert.That(valuesToChoiceList.ElementAt(3).RequiredValues["Type"], Is.EqualTo(ElementTypeEnum.Test.ToString()));
                
                Assert.That(valuesToChoiceList.ElementAt(3).AvailableChoices.Count(), Is.EqualTo(1));
                Assert.That(valuesToChoiceList.ElementAt(3).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
            }
            
            
            Assert.That(modalDialogInputFields.ElementAt(6).Name, Is.EqualTo("Authors"));
            Assert.That(modalDialogInputFields.ElementAt(6).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(6).Required, Is.EqualTo(false));

            Assert.That(modalDialogInputFields.ElementAt(7).Name, Is.EqualTo("Description"));
            Assert.That(modalDialogInputFields.ElementAt(7).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(7).Required, Is.EqualTo(true));

            Assert.That(modalDialogInputFields.ElementAt(8).Name, Is.EqualTo("Goals"));
            Assert.That(modalDialogInputFields.ElementAt(8).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(8).Required, Is.EqualTo(false));

            //difficulty
            Assert.That(modalDialogInputFields.ElementAt(9).Name, Is.EqualTo("Difficulty"));
            Assert.That(modalDialogInputFields.ElementAt(9).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(9).Required, Is.EqualTo(true));
            Assert.That(modalDialogInputFields.ElementAt(9), Is.TypeOf<ModalDialogDropdownInputField>());
            if (modalDialogInputFields.ElementAt(9) is ModalDialogDropdownInputField difficultyDropDownInput)
            {
                var valuesToChoiceList = difficultyDropDownInput.ValuesToChoiceMapping.ToList();
                Assert.That(valuesToChoiceList.Count(), Is.EqualTo(1));
                Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues, Is.EqualTo(null));
                
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(3));
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(LearningElementDifficultyEnum.Easy.ToString()));
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(1), Is.EqualTo(LearningElementDifficultyEnum.Medium.ToString()));
                Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(2), Is.EqualTo(LearningElementDifficultyEnum.Hard.ToString()));
            }

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

    [Test]
    public void GetCreateLearningElementInputFields_ThrowsException_ForWrongFileType()
    {
        
    }

    

    
    [Test]
    public void GetCreateLearningElementInputFields_AlternateTest()
    {
        
        
    }

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
    
    private ModalDialogInputFieldsFactory GetModalInputFieldsFactoryForTesting()
    {
        return new ModalDialogInputFieldsFactory();
    }
}