using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthoringTool.Components;
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
        Action<Tuple<ModalDialog.ModalDialogReturnValue, IDictionary<string, string>?>> onClose = _ => { };
        var dialogType = ModalDialog.ModalDialogType.Ok;
        var inputFields = new List<ModalDialog.ModalDialogInputField>
        {
            new("Test1", ModalDialog.ModalDialogInputType.Text, false),
            new ModalDialog.ModalDialogDropdownInputField("Test2",
                new[] { new ModalDialog.ModalDialogDropdownInputFieldChoiceMapping(null, new[] { "Test1", "Test2" }) },
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

    [Test]
    public void ModalDialog_CallsCallbackWithCorrectDialogReturnValue()
    {
        using var ctx = new Bunit.TestContext();
        
        //ModalDialogType.Ok
        //Ok button
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        Action<Tuple<ModalDialog.ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple =>
        {
            Assert.AreEqual(ModalDialog.ModalDialogReturnValue.Ok, tuple.Item1);
            Assert.AreEqual(null, tuple.Item2);
        };
        var dialogType = ModalDialog.ModalDialogType.Ok;

        var systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose,
            dialogType, null);
        var btn = systemUnderTest.Find("#btn-ok");
        btn.Click();
        
        //ModalDialogType.OkCancel
        //Ok button
        dialogType = ModalDialog.ModalDialogType.OkCancel;

        systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose, dialogType, null);
        btn = systemUnderTest.Find("#btn-ok");
        btn.Click();
        
        //Cancel button
        onClose = tuple =>
                {
                    Assert.AreEqual(ModalDialog.ModalDialogReturnValue.Cancel, tuple.Item1);
                    Assert.AreEqual(null, tuple.Item2);
                };
        
        systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose, dialogType, null);
        btn = systemUnderTest.Find("#btn-cancel");
        btn.Click();
        
        //ModalDialogType.DeleteCancel
        //Cancel button
        dialogType = ModalDialog.ModalDialogType.DeleteCancel;
        
        systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose, dialogType, null);
        btn = systemUnderTest.Find("#btn-cancel");
        btn.Click();
        
        //Delete button
        onClose = tuple =>
                {
                    Assert.AreEqual(ModalDialog.ModalDialogReturnValue.Delete, tuple.Item1);
                    Assert.AreEqual(null, tuple.Item2);
                };
        
        systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose, dialogType, null);
        btn = systemUnderTest.Find("#btn-delete");
        btn.Click();
        
        //ModalDialogType.YesNoCancel
        //Cancel button
        dialogType = ModalDialog.ModalDialogType.YesNoCancel;
        onClose = tuple =>
                {
                    Assert.AreEqual(ModalDialog.ModalDialogReturnValue.Cancel, tuple.Item1);
                    Assert.AreEqual(null, tuple.Item2);
                };
        
        systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose, dialogType, null);
        btn = systemUnderTest.Find("#btn-cancel");
        btn.Click();
        
        
        //No button
        onClose = tuple =>
                {
                    Assert.AreEqual(ModalDialog.ModalDialogReturnValue.No, tuple.Item1);
                    Assert.AreEqual(null, tuple.Item2);
                };
        
        systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose, dialogType, null);
        btn = systemUnderTest.Find("#btn-no");
        btn.Click();
        
        //Yes button
        onClose = tuple =>
                {
                    Assert.AreEqual(ModalDialog.ModalDialogReturnValue.Yes, tuple.Item1);
                    Assert.AreEqual(null, tuple.Item2);
                };
        
        systemUnderTest = CreateRenderedModalDialogComponent(ctx, title, text, onClose, dialogType, null);
        btn = systemUnderTest.Find("#btn-yes");
        btn.Click();
    }
    
    [Test]
    public void ModalDialog_CallsCallbackWithCorrectInputFieldValues()
    {
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        var onCloseCalled = false;
        Action<Tuple<ModalDialog.ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple =>
        {
            var (returnValue, dictionary) = tuple;
            Assert.AreEqual(ModalDialog.ModalDialogReturnValue.Ok, returnValue);
            Assert.NotNull(dictionary);
            
            Assert.AreEqual("foobar123", dictionary!["Test1"]);
            Assert.AreEqual("Bar", dictionary["Test2"]);
            Assert.AreEqual("Baz", dictionary["Test3"]);
            Assert.Throws<KeyNotFoundException>(() => { var _ = dictionary["Test4"];});
            onCloseCalled = true;
        };
        var dialogType = ModalDialog.ModalDialogType.Ok;
        var inputFields = new List<ModalDialog.ModalDialogInputField>
        {
            new("Test1", ModalDialog.ModalDialogInputType.Text, true),
            new ModalDialog.ModalDialogDropdownInputField("Test2",
                new[] { new ModalDialog.ModalDialogDropdownInputFieldChoiceMapping(null, new[] { "Foo", "Bar" }) },
                true),
            new ModalDialog.ModalDialogDropdownInputField("Test3",
                new[] { new ModalDialog.ModalDialogDropdownInputFieldChoiceMapping(new Dictionary<string, string>
                {
                    {"Test2", "Bar"}
                }, new[] { "Foz", "Baz" }) },
                true),
            new("Test4", ModalDialog.ModalDialogInputType.Text)
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
    public void ModalDialog_RefusesToCallCallbackWithMissingRequiredValues()
    {
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        Action<Tuple<ModalDialog.ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple =>
        {
            Assert.Fail("onClose was called, but should not have.");
        };
        var dialogType = ModalDialog.ModalDialogType.Ok;
        var inputFields = new List<ModalDialog.ModalDialogInputField>
        {
            new("Test1", ModalDialog.ModalDialogInputType.Text, true),
            new ModalDialog.ModalDialogDropdownInputField("Test2",
                new[] { new ModalDialog.ModalDialogDropdownInputFieldChoiceMapping(null, new[] { "Foo", "Bar" }) },
                true),
            new ModalDialog.ModalDialogDropdownInputField("Test3",
                new[] { new ModalDialog.ModalDialogDropdownInputFieldChoiceMapping(new Dictionary<string, string>
                {
                    {"Test2", "Bar"}
                }, new[] { "Foz", "Baz" }) },
                true),
            new("Test4", ModalDialog.ModalDialogInputType.Text)
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
    public void ModalDialog_DropdownSelectionRulesChangeAvailableOptions()
    {
        
        using var ctx = new Bunit.TestContext();
        
        var title = "Test Dialog";
        var text = "This is a dialog for automated testing purposes";
        Action<Tuple<ModalDialog.ModalDialogReturnValue, IDictionary<string, string>?>> onClose = tuple =>
        {
            Assert.Fail("onClose was called, but should not have.");
        };
        var dialogType = ModalDialog.ModalDialogType.Ok;
        var inputFields = new List<ModalDialog.ModalDialogInputField>
        {
            new("Test1", ModalDialog.ModalDialogInputType.Text, true),
            new ModalDialog.ModalDialogDropdownInputField("Test2",
                new[] { new ModalDialog.ModalDialogDropdownInputFieldChoiceMapping(null, new[] { "Foo", "Bar" }) },
                true),
            new ModalDialog.ModalDialogDropdownInputField("Test3",
                new[] { new ModalDialog.ModalDialogDropdownInputFieldChoiceMapping(new Dictionary<string, string>
                {
                    {"Test2", "Bar"}
                }, new[] { "Foz", "Baz" }) },
                true),
            new("Test4", ModalDialog.ModalDialogInputType.Text)
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
    public void ModalDialog_InitialValuesArePopulated()
    {
        Assert.Fail("NYI");
    }

    [Test]
    public void ModalDialog_DropdownSelectionRulesRespectsInitialValues()
    {
        Assert.Fail("NYI");
    }
    
    private IRenderedComponent<ModalDialog> CreateRenderedModalDialogComponent(Bunit.TestContext ctx, string title, string text,
        Action<Tuple<ModalDialog.ModalDialogReturnValue, IDictionary<string, string>?>> onClose,
        ModalDialog.ModalDialogType dialogType,
        IEnumerable<ModalDialog.ModalDialogInputField>? inputFields)
    {
        return ctx.RenderComponent<ModalDialog>(parameters => parameters
            .Add(p => p.Title, title)
            .Add(p => p.Text, text)
            .Add(p => p.OnClose, onClose)
            .Add(p => p.DialogType, dialogType)
            .Add(p => p.InputFields, inputFields));
    }
}