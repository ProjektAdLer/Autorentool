using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AuthoringTool;
using AuthoringTool.Components;
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
            new("Test1", ModalDialogInputType.Text, false),
            new ModalDialogDropdownInputField("Test2",
                new[] { new ModalDialogDropdownInputFieldChoiceMapping(null, new[] { "Test1", "Test2" }) },
                true),
        };

        var systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose,
            dialogType, inputFields);
        
        Assert.AreEqual(title, systemUnderTest.Instance.Title);
        Assert.AreEqual(text, systemUnderTest.Instance.Text);
        //we need to construct an EventCallback from our action
        Assert.AreEqual(EventCallback.Factory.Create(onClose.Target ??
                                                     throw new InvalidOperationException("onClose.Target is null"),
                onClose),
            systemUnderTest.Instance.OnClose);
        Assert.AreEqual(dialogType, systemUnderTest.Instance.DialogType);
        Assert.AreEqual(inputFields, systemUnderTest.Instance.InputFields);
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
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple =>
        {
            Assert.AreEqual(returnValue, tuple.Item1);
            Assert.AreEqual(null, tuple.Item2);
            onCloseCalled = true;
        };
        var dialogType = type;

        var systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose,
            dialogType, null, null);
        var btn = systemUnderTest.Find(cssSelector);
        btn.Click();
        Assert.AreEqual(true, onCloseCalled);
    }
    
    [Test]
    public void ModalDialog_EnterKeyPressedOnInputField_SubmitsDialog()
    {
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        var onCloseCalled = false;
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple =>
        {
            Assert.AreEqual(ModalDialogReturnValue.Ok, tuple.Item1);
            Assert.AreEqual("foobar", tuple.Item2!["Test1"]);
            onCloseCalled = true;
        };
        var dialogType = ModalDialogType.Ok;
        var inputFields = new List<ModalDialogInputField>
        {
            new("Test1", ModalDialogInputType.Text, true),
        };

        var systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose,
            dialogType, inputFields, null);

        var element = systemUnderTest.Find("#modal-input-field-test1");
        element.Input("foobar");
        element.KeyDown(Key.Enter);

        Assert.AreEqual(true, onCloseCalled);
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
            Assert.AreEqual(ModalDialogReturnValue.Ok, returnValue);
            Assert.NotNull(dictionary);
            
            Assert.AreEqual("foobar123", dictionary!["Test1"]);
            Assert.AreEqual("Bar", dictionary["Test2"]);
            Assert.AreEqual("Baz", dictionary["Test3"]);
            Assert.AreEqual("", dictionary["Test4"]);
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

        var systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose,
            dialogType, inputFields);
        systemUnderTest.Find("#modal-input-field-test1").Input("foobar123");
        systemUnderTest.Find("#modal-input-drop-test2").Change("Bar");
        systemUnderTest.Find("#modal-input-drop-test3").Change("Baz");
        
        systemUnderTest.Find("#btn-ok").Click();
        
        Assert.AreEqual(true, onCloseCalled);
    }

    [Test]
    public void ModalDialog_MissingRequiredValues_RefusesToCallCallback()
    {
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple =>
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

        var systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose,
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
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple =>
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

        var systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose,
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
            Assert.AreEqual(ModalDialogReturnValue.Ok, returnValue);
            Assert.NotNull(dictionary);
            
            Assert.AreEqual("foobar", dictionary["Test1"]);
            Assert.AreEqual("Bar", dictionary["Test2"]);
            Assert.AreEqual("", dictionary["Test3"]);
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

        var systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose,
            dialogType, inputFields, inputFieldsInitialValues);
        Assert.AreEqual("foobar", systemUnderTest.Find("#modal-input-field-test1").Attributes["value"]?.Value);
        Assert.AreEqual("Bar", systemUnderTest.Find("#modal-input-drop-test2").Attributes["value"]?.Value);
        Assert.AreEqual("", systemUnderTest.Find("#modal-input-field-test3").Attributes["value"]?.Value);

        systemUnderTest.Find("#btn-ok").Click();
        Assert.AreEqual(true, onCloseCalled);
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
            Assert.AreEqual(ModalDialogReturnValue.Ok, returnValue);
            Assert.NotNull(dictionary);
            
            Assert.AreEqual("boofar", dictionary["Test1"]);
            Assert.AreEqual("Foo", dictionary["Test2"]);
            Assert.AreEqual("foobar123", dictionary["Test3"]);
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

        var systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose,
            dialogType, inputFields, inputFieldsInitialValues);
        systemUnderTest.Find("#modal-input-field-test1").Input("boofar");
        systemUnderTest.Find("#modal-input-drop-test2").Change("Foo");
        systemUnderTest.Find("#modal-input-field-test3").Input("foobar123");

        systemUnderTest.Find("#btn-ok").Click();
        Assert.AreEqual(true, onCloseCalled);
    }

    [Test]
    public void ModalDialog_DropdownSelectionRules_CorrectWhenInitialValuesProvided()
    {
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        Action<Tuple<ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple =>
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

        var systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose,
            dialogType, inputFields, inputFieldsInitialValues);

        Assert.AreEqual("Bar", systemUnderTest.Find("#modal-input-drop-test1").Attributes["value"]?.Value);
        Assert.AreEqual("Baz", systemUnderTest.Find("#modal-input-drop-test2").Attributes["value"]?.Value);
        Assert.DoesNotThrow(() => systemUnderTest.Find("#modal-input-drop-test2-foz"));
        Assert.DoesNotThrow(() => systemUnderTest.Find("#modal-input-drop-test2-baz"));
    }
    
    private IRenderedComponent<ModalDialog> CreateRenderedModalDialogComponent(Bunit.TestContext ctx, string title, string text,
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