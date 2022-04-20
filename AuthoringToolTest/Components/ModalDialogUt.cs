using System;
using System.Collections.Generic;
using AuthoringTool.Components.ModalDialog;
using NUnit.Framework;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace AuthoringToolTest.Components;

[TestFixture]
public class ModalDialogUt
{
    [Test]
    public void ModalDialog_StandardConstructor_AllPropertiesInitialized()
    {
        using var ctx = new Bunit.TestContext();
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = _ => { };
        var dialogType = ModalDialogType.Ok;
        var inputFields = new List<ModalDialogInputField>
        {
            new("Test1", ModalDialogInputType.Text),
            new ModalDialogDropdownInputField("Test2",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(null, new[] { "Test1", "Test2" }) },
                true),
        };

        var systemUnderTest = CreateRenderedModalDialogComponentForTesting(ctx, title, text, onClose,
            dialogType, inputFields);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Title, Is.EqualTo(title));
            Assert.That(systemUnderTest.Instance.Text, Is.EqualTo(text));
            //we need to construct an EventCallback from our action
            Assert.That(systemUnderTest.Instance.OnClose, Is.EqualTo(EventCallback.Factory.Create(onClose.Target ??
                                                         throw new InvalidOperationException("onClose.Target is null"),
                    onClose)));
            Assert.That(systemUnderTest.Instance.DialogType, Is.EqualTo(dialogType));
            Assert.That(systemUnderTest.Instance.InputFields, Is.EqualTo(inputFields));
        });
    }

    [Test]
    public void ModalDialog_XButtonClicked_CancelsDialog()
    {
        using var ctx = new Bunit.TestContext();
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        var onCloseCalled = false;
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple => {
            onCloseCalled = true;
            Assert.Multiple(() =>
            {
                Assert.That(tuple.Item1, Is.EqualTo(ModalDialogReturnValue.Cancel));
                Assert.That(tuple.Item2, Is.Null);
            });
        };
        var dialogType = ModalDialogType.OkCancel;

        var systemUnderTest = CreateRenderedModalDialogComponentForTesting(ctx, title, text, onClose, dialogType);

        var button = systemUnderTest.Find(".close");
        button.Click();
        
        Assert.That(onCloseCalled);
    }

    //ModalDialogType, css selector string, ModalDialogReturnValue
    static object[] ModalDialog_ButtonClicked_CallsCallbackWithCorrectDialogReturnValue_TestCaseSource =
    {
        new object[] { ModalDialogType.Ok, "#btn-ok", ModalDialogReturnValue.Ok },
        new object[] { ModalDialogType.OkCancel, "#btn-ok", ModalDialogReturnValue.Ok },
        new object[] { ModalDialogType.OkCancel, "#btn-cancel", ModalDialogReturnValue.Cancel },
        new object[] { ModalDialogType.DeleteCancel, "#btn-delete", ModalDialogReturnValue.Delete },
        new object[] { ModalDialogType.DeleteCancel, "#btn-cancel", ModalDialogReturnValue.Cancel },
        new object[] { ModalDialogType.YesNoCancel, "#btn-yes", ModalDialogReturnValue.Yes },
        new object[] { ModalDialogType.YesNoCancel, "#btn-no", ModalDialogReturnValue.No },
        new object[] { ModalDialogType.YesNoCancel, "#btn-cancel", ModalDialogReturnValue.Cancel },
        new object[] { ModalDialogType.YesNo, "#btn-yes", ModalDialogReturnValue.Yes},
        new object[] { ModalDialogType.YesNo, "#btn-no", ModalDialogReturnValue.No}
    };
    [Test]
    [TestCaseSource(nameof(ModalDialog_ButtonClicked_CallsCallbackWithCorrectDialogReturnValue_TestCaseSource))]
    public void ModalDialog_ButtonClicked_CallsCallbackWithCorrectDialogReturnValue(ModalDialogType type,
        string cssSelector, ModalDialogReturnValue returnValue)
    {
        using var ctx = new Bunit.TestContext();
        
        //ModalDialogType.Ok
        //Ok button
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        var onCloseCalled = false;
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple => {
            Assert.Multiple(() =>
            {
                var (modalDialogReturnValue, dictionary) = tuple;
                Assert.That(modalDialogReturnValue, Is.EqualTo(returnValue));
                Assert.That(dictionary, Is.EqualTo(null));
            });
            onCloseCalled = true;
        };

        var systemUnderTest = CreateRenderedModalDialogComponentForTesting(ctx, title, text, onClose,
            type);
        var btn = systemUnderTest.Find(cssSelector);
        btn.Click();
        Assert.That(onCloseCalled, Is.EqualTo(true));
    }
    
    [Test]
    public void ModalDialog_EnterKeyPressedOnInputField_SubmitsDialog()
    {
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        var onCloseCalled = false;
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple => {
            Assert.Multiple(() =>
            {
                var (modalDialogReturnValue, dictionary) = tuple;
                Assert.That(modalDialogReturnValue, Is.EqualTo(ModalDialogReturnValue.Ok));
                Assert.That(dictionary!["Test1"], Is.EqualTo("foobar"));
            });
            onCloseCalled = true;
        };
        var dialogType = ModalDialogType.Ok;
        var inputFields = new List<ModalDialogInputField>
        {
            new("Test1", ModalDialogInputType.Text, true),
        };

        var systemUnderTest = CreateRenderedModalDialogComponentForTesting(ctx, title, text, onClose,
            dialogType, inputFields);

        var element = systemUnderTest.Find("#modal-input-field-test1");
        element.Input("foobar");
        element.KeyDown(Key.Enter);

        Assert.That(onCloseCalled, Is.EqualTo(true));
    }
    
    [Test]
    public void ModalDialog_DialogSubmitted_CallsCallbackWithCorrectInputFieldValues()
    {
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        var onCloseCalled = false;
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple =>
        {
            var (returnValue, dictionary) = tuple;
            Assert.Multiple(() =>
            {
                Assert.That(returnValue, Is.EqualTo(ModalDialogReturnValue.Ok));
                Assert.That(dictionary, Is.Not.Null);
            });
            
            Assert.Multiple(() =>
            {
                Assert.That(dictionary!["Test1"], Is.EqualTo("foobar123"));
                Assert.That(dictionary["Test2"], Is.EqualTo("Bar"));
                Assert.That(dictionary["Test3"], Is.EqualTo("Baz"));
                Assert.That(dictionary["Test4"], Is.EqualTo(""));
            });
            onCloseCalled = true;
        };
        var dialogType = ModalDialogType.Ok;
        var inputFields = new List<ModalDialogInputField>
        {
            new("Test1", ModalDialogInputType.Text, true),
            new ModalDialogDropdownInputField("Test2",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(null, new[] { "Foo", "Bar" }) },
                true),
            new ModalDialogDropdownInputField("Test3",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(new Dictionary<string, string>
                {
                    {"Test2", "Bar"}
                }, new[] { "Foz", "Baz" }) },
                true),
            new("Test4", ModalDialogInputType.Text)
        };

        var systemUnderTest = CreateRenderedModalDialogComponentForTesting(ctx, title, text, onClose,
            dialogType, inputFields);
        systemUnderTest.Find("#modal-input-field-test1").Input("foobar123");
        systemUnderTest.Find("#modal-input-drop-test2").Change("Bar");
        systemUnderTest.Find("#modal-input-drop-test3").Change("Baz");
        
        systemUnderTest.Find("#btn-ok").Click();
        
        Assert.That(onCloseCalled, Is.EqualTo(true));
    }

    [Test]
    public void ModalDialog_MissingRequiredValues_RefusesToCallCallback()
    {
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = _ =>
        {
            Assert.Fail("onClose was called, but should not have.");
        };
        var dialogType = ModalDialogType.Ok;
        var inputFields = new List<ModalDialogInputField>
        {
            new("Test1", ModalDialogInputType.Text, true),
            new ModalDialogDropdownInputField("Test2",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(null, new[] { "Foo", "Bar" }) },
                true),
            new ModalDialogDropdownInputField("Test3",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(new Dictionary<string, string>
                {
                    {"Test2", "Bar"}
                }, new[] { "Foz", "Baz" }) },
                true),
            new("Test4", ModalDialogInputType.Text)
        };

        var systemUnderTest = CreateRenderedModalDialogComponentForTesting(ctx, title, text, onClose,
            dialogType, inputFields);
        
        systemUnderTest.Find("#btn-ok").Click();
        Assert.DoesNotThrow(() => systemUnderTest.Find(".modal-input-warning"));
        systemUnderTest.Find("#modal-input-field-test1").Input("foobar123");
        Assert.DoesNotThrow(() => systemUnderTest.Find(".modal-input-warning"));
        systemUnderTest.Find("#modal-input-drop-test2").Change("Bar");
        Assert.DoesNotThrow(() => systemUnderTest.Find(".modal-input-warning"));
        systemUnderTest.Find("#modal-input-drop-test3").Change("Baz");
        Assert.Throws<ElementNotFoundException>(() => systemUnderTest.Find(".modal-input-warning"));
    }

    [Test]
    public void ModalDialog_DropdownSelectionRules_ChangeAvailableOptions()
    {
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = _ =>
        {
            Assert.Fail("onClose was called, but should not have.");
        };
        var dialogType = ModalDialogType.Ok;
        var inputFields = new List<ModalDialogInputField>
        {
            new("Test1", ModalDialogInputType.Text, true),
            new ModalDialogDropdownInputField("Test2",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(null, new[] { "Foo", "Bar" }) },
                true),
            new ModalDialogDropdownInputField("Test3",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(new Dictionary<string, string>
                {
                    {"Test2", "Bar"}
                }, new[] { "Foz", "Baz" }) },
                true),
            new("Test4", ModalDialogInputType.Text)
        };

        var systemUnderTest = CreateRenderedModalDialogComponentForTesting(ctx, title, text, onClose,
            dialogType, inputFields);
        
        Assert.Throws<ElementNotFoundException>(() => systemUnderTest.Find("#modal-input-drop-test3-foz"));
        Assert.Throws<ElementNotFoundException>(() => systemUnderTest.Find("#modal-input-drop-test3-baz"));
        
        systemUnderTest.Find("#modal-input-drop-test2").Change("Bar");
        
        Assert.DoesNotThrow(() => systemUnderTest.Find("#modal-input-drop-test3-foz"));
        Assert.DoesNotThrow(() => systemUnderTest.Find("#modal-input-drop-test3-baz"));
    }

    [Test]
    public void ModalDialog_InitialValues_PopulatedAndReturned()
    {
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        var onCloseCalled = false;
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple =>
        {
            var (returnValue, dictionary) = tuple;
            Assert.Multiple(() =>
            {
                Assert.That(returnValue, Is.EqualTo(ModalDialogReturnValue.Ok));
                Assert.That(dictionary, Is.Not.Null);
            });
            Assert.Multiple(() =>
            {
                Assert.That(dictionary!["Test1"], Is.EqualTo("foobar"));
                Assert.That(dictionary["Test2"], Is.EqualTo("Bar"));
                Assert.That(dictionary["Test3"], Is.EqualTo(""));
            });
            onCloseCalled = true;
        };
        var dialogType = ModalDialogType.Ok;
        var inputFields = new List<ModalDialogInputField>
        {
            new("Test1", ModalDialogInputType.Text, true),
            new ModalDialogDropdownInputField("Test2",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(null, new[] { "Foo", "Bar" }) },
                true),
            new("Test3", ModalDialogInputType.Text)
        };
        var inputFieldsInitialValues = new Dictionary<string, string>
        {
            {"Test1", "foobar"},
            {"Test2", "Bar"}
        };

        var systemUnderTest = CreateRenderedModalDialogComponentForTesting(ctx, title, text, onClose,
            dialogType, inputFields, inputFieldsInitialValues);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Find("#modal-input-field-test1").Attributes["value"]?.Value, Is.EqualTo("foobar"));
            Assert.That(systemUnderTest.Find("#modal-input-drop-test2").Attributes["value"]?.Value, Is.EqualTo("Bar"));
            Assert.That(systemUnderTest.Find("#modal-input-field-test3").Attributes["value"]?.Value, Is.EqualTo(""));
        });
        systemUnderTest.Find("#btn-ok").Click();
        Assert.That(onCloseCalled, Is.EqualTo(true));
    }

    [Test]
    public void ModalDialog_InitialValue_ReturnsCorrectValueAfterChange()
    {
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        var onCloseCalled = false;
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple =>
        {
            var (returnValue, dictionary) = tuple;
            Assert.Multiple(() =>
            {
                Assert.That(returnValue, Is.EqualTo(ModalDialogReturnValue.Ok));
                Assert.That(dictionary, Is.Not.Null);
            });
            Assert.Multiple(() =>
            {
                Assert.That(dictionary!["Test1"], Is.EqualTo("boofar"));
                Assert.That(dictionary["Test2"], Is.EqualTo("Foo"));
                Assert.That(dictionary["Test3"], Is.EqualTo("foobar123"));
            });
            onCloseCalled = true;
        };
        var dialogType = ModalDialogType.Ok;
        var inputFields = new List<ModalDialogInputField>
        {
            new("Test1", ModalDialogInputType.Text, true),
            new ModalDialogDropdownInputField("Test2",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(null, new[] { "Foo", "Bar" }) },
                true),
            new("Test3", ModalDialogInputType.Text)
        };
        var inputFieldsInitialValues = new Dictionary<string, string>
        {
            {"Test1", "foobar"},
            {"Test2", "Bar"}
        };

        var systemUnderTest = CreateRenderedModalDialogComponentForTesting(ctx, title, text, onClose,
            dialogType, inputFields, inputFieldsInitialValues);
        systemUnderTest.Find("#modal-input-field-test1").Input("boofar");
        systemUnderTest.Find("#modal-input-drop-test2").Change("Foo");
        systemUnderTest.Find("#modal-input-field-test3").Input("foobar123");

        systemUnderTest.Find("#btn-ok").Click();
        Assert.That(onCloseCalled, Is.EqualTo(true));
    }

    [Test]
    public void ModalDialog_DropdownSelectionRules_CorrectWhenInitialValuesProvided()
    {
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = _ =>
        {
            Assert.Fail("onclose unexpectedly called");
        };
        var dialogType = ModalDialogType.Ok;
        var inputFields = new List<ModalDialogInputField>
        {
            new ModalDialogDropdownInputField("Test1",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(null, new[] { "Foo", "Bar" }) },
                true),
            new ModalDialogDropdownInputField("Test2",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(new Dictionary<string, string>
                {
                    {"Test1", "Bar"}
                }, new[] { "Foz", "Baz" }) },
                true),
        };
        var inputFieldsInitialValues = new Dictionary<string, string>
        {
            {"Test1", "Bar"},
            {"Test2", "Baz"}
        };

        var systemUnderTest = CreateRenderedModalDialogComponentForTesting(ctx, title, text, onClose,
            dialogType, inputFields, inputFieldsInitialValues);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Find("#modal-input-drop-test1").Attributes["value"]?.Value, Is.EqualTo("Bar"));
            Assert.That(systemUnderTest.Find("#modal-input-drop-test2").Attributes["value"]?.Value, Is.EqualTo("Baz"));
        });
        Assert.DoesNotThrow(() => systemUnderTest.Find("#modal-input-drop-test2-foz"));
        Assert.DoesNotThrow(() => systemUnderTest.Find("#modal-input-drop-test2-baz"));
    }

    private IRenderedComponent<ModalDialog> CreateRenderedModalDialogComponentForTesting(Bunit.TestContext ctx, string title, string text,
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose,
        ModalDialogType dialogType,
        IEnumerable<ModalDialogInputField>? inputFields = null, IDictionary<string,string>? initialValues = null)
    {
        return ctx.RenderComponent<ModalDialog>(parameters => parameters
            .Add(p => p.Title, title)
            .Add(p => p.Text, text)
            .Add(p => p.OnClose, onClose)
            .Add(p => p.DialogType, dialogType)
            .Add(p => p.InputFields, inputFields)
            .Add(p => p.InputFieldsInitialValue, initialValues));
    }
}