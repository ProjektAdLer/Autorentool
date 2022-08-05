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
            
            
            //assignment Field: solved
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
                Assert.That(typeDropDownInput.ValuesToChoiceMapping.ToList().Count, Is.EqualTo(1));
                Assert.That(
                    typeDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0),
                    Is.TypeOf<ModalDialogDropdownInputFieldChoiceMapping>()
                );
                
                Assert.That(typeDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).RequiredValues, Is.EqualTo(null));
                if (correctFileTypeForTest is ContentTypeEnum.Image or ContentTypeEnum.Pdf)
                {
                    Assert.That(typeDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices.Count(), Is.EqualTo(1));
                    Assert.That(typeDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                }
                else if (correctFileTypeForTest is ContentTypeEnum.Video)
                {
                    Assert.That(typeDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices.Count(), Is.EqualTo(2));
                    Assert.That(typeDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                    Assert.That(typeDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices.ElementAt(1), Is.EqualTo(ElementTypeEnum.Activation.ToString()));
                }
                else if (correctFileTypeForTest is ContentTypeEnum.H5P)
                {
                    Assert.That(typeDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices.Count(), Is.EqualTo(3));
                    Assert.That(typeDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ElementTypeEnum.Activation.ToString()));
                    Assert.That(typeDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices
                    .ElementAt(1), Is.EqualTo(ElementTypeEnum.Interaction.ToString()));
                    Assert.That(typeDropDownInput.ValuesToChoiceMapping.ToList().ElementAt(0).AvailableChoices
                    .ElementAt(2), Is.EqualTo(ElementTypeEnum.Test.ToString()));
                }
            }

            //content field: solved
            Assert.That(modalDialogInputFields.ElementAt(5).Name, Is.EqualTo("Content"));
            Assert.That(modalDialogInputFields.ElementAt(5).Type, Is.EqualTo(ModalDialogInputType.Text));
            Assert.That(modalDialogInputFields.ElementAt(5).Required, Is.EqualTo(true));
            Assert.That(modalDialogInputFields.ElementAt(5), Is.TypeOf<ModalDialogDropdownInputField>());
            if (modalDialogInputFields.ElementAt(5) is ModalDialogDropdownInputField contentDropDownInput)
            {
                var valuesToChoiceList = contentDropDownInput.ValuesToChoiceMapping.ToList();
                if (correctFileTypeForTest is ContentTypeEnum.Image)
                {
                    Assert.That(valuesToChoiceList.Count(), Is.EqualTo(1));
                    
                    Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                    Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues, Contains.Key("Type"));
                    Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                    
                    Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(1));
                    Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Image.ToString()));
                }
                else if (correctFileTypeForTest is ContentTypeEnum.Video)
                {
                    Assert.That(valuesToChoiceList.Count(), Is.EqualTo(2));
                    
                    Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                    Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues, Contains.Key("Type"));
                    Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                    
                    Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(1));
                    Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Video.ToString()));
                    
                    Assert.That(valuesToChoiceList.ElementAt(1).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                    Assert.That(valuesToChoiceList.ElementAt(1).RequiredValues, Contains.Key("Type"));
                    Assert.That(valuesToChoiceList.ElementAt(1).RequiredValues["Type"], Is.EqualTo(ElementTypeEnum.Activation.ToString()));
                    
                    Assert.That(valuesToChoiceList.ElementAt(1).AvailableChoices.Count(), Is.EqualTo(1));
                    Assert.That(valuesToChoiceList.ElementAt(1).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Video.ToString()));
                }
                else if (correctFileTypeForTest is ContentTypeEnum.Pdf)
                {
                    Assert.That(valuesToChoiceList.Count(), Is.EqualTo(1));
                    
                    Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                    Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues, Contains.Key("Type"));
                    Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues["Type"], Is.EqualTo(ElementTypeEnum.Transfer.ToString()));
                    
                    Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(1));
                    Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.Pdf.ToString()));
                }
                else if (correctFileTypeForTest is ContentTypeEnum.H5P)
                {
                    Assert.That(valuesToChoiceList.Count(), Is.EqualTo(3));
                    
                    Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                    Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues, Contains.Key("Type"));
                    Assert.That(valuesToChoiceList.ElementAt(0).RequiredValues["Type"], Is.EqualTo(ElementTypeEnum.Activation.ToString()));
                    
                    Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.Count(), Is.EqualTo(1));
                    Assert.That(valuesToChoiceList.ElementAt(0).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
                    
                    
                    Assert.That(valuesToChoiceList.ElementAt(1).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                    Assert.That(valuesToChoiceList.ElementAt(1).RequiredValues, Contains.Key("Type"));
                    Assert.That(valuesToChoiceList.ElementAt(1).RequiredValues["Type"], Is.EqualTo(ElementTypeEnum.Interaction.ToString()));
                    
                    Assert.That(valuesToChoiceList.ElementAt(1).AvailableChoices.Count(), Is.EqualTo(1));
                    Assert.That(valuesToChoiceList.ElementAt(1).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
                    
                    
                    Assert.That(valuesToChoiceList.ElementAt(2).RequiredValues, Is.TypeOf<Dictionary<string,string>>());
                    Assert.That(valuesToChoiceList.ElementAt(2).RequiredValues, Contains.Key("Type"));
                    Assert.That(valuesToChoiceList.ElementAt(2).RequiredValues["Type"], Is.EqualTo(ElementTypeEnum.Test.ToString()));
                    
                    Assert.That(valuesToChoiceList.ElementAt(2).AvailableChoices.Count(), Is.EqualTo(1));
                    Assert.That(valuesToChoiceList.ElementAt(2).AvailableChoices.ElementAt(0), Is.EqualTo(ContentTypeEnum.H5P.ToString()));
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


            //type field: work in progress
            
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
            

            //content field: work in progress
            
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