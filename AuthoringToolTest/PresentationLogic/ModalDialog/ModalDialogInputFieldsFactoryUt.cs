using System;
using System.Collections.Generic;
using System.Linq;
using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.ModalDialog;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.ModalDialog;

[TestFixture]

public class ModalDialogInputFieldsFactoryUt
{
    [Test]
    public void GetCreateLearningElementInputFields_Test()
    {
        var systemUnderTest = new ModalDialogInputFieldsFactory();

        for (int contentFileTypeTest = 0; contentFileTypeTest < 8; contentFileTypeTest++)
        {
            string name = "foo";
            string contentFileExtension =
                helper_GetCreateLearningElementInputFields_ReturnsExpectedFileExtension(contentFileTypeTest);

            var contentType =
                helper_GetCreateLearningElementInputFields_ReturnsExpectedContentType(contentFileTypeTest);



            string spaceName = "skazzle";

            LearningContentViewModel? dragAndDropLearningContent =
                new LearningContentViewModel(
                    name,
                    contentFileExtension,
                    new byte[] { }
                );
            
            try
            {
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
                });
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

    private string helper_GetCreateLearningElementInputFields_ReturnsExpectedFileExtension(int FileExtensionTest)
    {
        string fileExtension = "";
        string[] extensionDictionary = new string[7]
        {
            "jpg", 
            "png", 
            "webp", 
            "bmp", 
            "mp4", 
            "h5p",
            "pdf"
        };
        
        if (FileExtensionTest is < 7 and < 0)
        {
            fileExtension = extensionDictionary[FileExtensionTest];
        }
        else
        {
            fileExtension = "abc";
        }
        return fileExtension;
    }

    private ContentTypeEnum? helper_GetCreateLearningElementInputFields_ReturnsExpectedContentType(int contentTestNumber)
    {
        ContentTypeEnum[] extensionTypeDictionary = new ContentTypeEnum[7]
        {
            ContentTypeEnum.Image,
            ContentTypeEnum.Image,
            ContentTypeEnum.Image,
            ContentTypeEnum.Image,
            ContentTypeEnum.Video,
            ContentTypeEnum.H5P,
            ContentTypeEnum.Pdf
        };

        if (contentTestNumber >= 7)
            return null;
        return extensionTypeDictionary[contentTestNumber];

    }
        
    
    private string helper_GetCreateLearningElementInputFields_ReturnWrongFileExtension()
    {
        string[] notHandledExtensions = new[] { "css", "html", "js", "cs" };
        Random random = new Random();
        return notHandledExtensions[random.Next(0, notHandledExtensions.Length)];
    }
    
    [Test]
    public void GetCreateLearningElementInputFields()
    {
        for (int contentFileTypeTest = 0; contentFileTypeTest < 8; contentFileTypeTest++)
        {
            try
            {
                var dragAndDropLearningContentName = "foo";
                var dragAndDropLearningContentType =
                    helper_GetCreateLearningElementInputFields_ReturnsExpectedFileExtension(1);
                var dragAndDropLearningContentContent = new byte[] { };
                LearningContentViewModel? dragAndDropLearningContent =
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
                    (dragAndDropLearningContent, learningSpaces, worldName);


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