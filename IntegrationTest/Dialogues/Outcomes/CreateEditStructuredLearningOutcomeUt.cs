using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.LearningOutcomes;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using PresentationTest;
using Shared.LearningOutcomes;

namespace IntegrationTest.Dialogues.Outcomes;

[TestFixture]
public class CreateEditStructuredLearningOutcomeUt : MudDialogTestFixture<CreateEditStructuredLearningOutcome>
{
    [SetUp]
    public void Setup()
    {
        PresentationLogic = Substitute.For<IPresentationLogic>();
        Context.Services.AddSingleton(PresentationLogic);
    }

    [TearDown]
    public void Teardown()
    {
        DialogProvider.Dispose();
        Context.Dispose();
        Collection = null;
        Outcome = null;
    }

    private IPresentationLogic PresentationLogic { get; set; }

    private LearningOutcomeCollectionViewModel? Collection { get; set; }

    private StructuredLearningOutcomeViewModel? Outcome { get; set; }

    public IDialogReference Dialog { get; set; }

    private async Task GetDialogAsync()
    {
        var dialogParameters = GetDialogParameters(Collection, Outcome);

        Dialog = await OpenDialogAndGetDialogReferenceAsync(parameters: dialogParameters);
        DialogProvider.Render();
    }

    [Test]
    // ANF-ID: [AHO01]
    public async Task SubmitWithOutCurrentLearningOutcome_VerbNotSet_DoesNotCallAddStructuredLearningOutcome()
    {
        await GetDialogAsync();

        const string whatstr = "New Outcome";
        const string wherebystr = "Whereby";
        const string whatforstr = "What for";

        var textFields = DialogProvider.FindComponents<MudTextField<string>>();
        var whattf = textFields.FirstOrDefault(c =>
            c.Instance.Placeholder == "CreateEditStructuredLearningOutcome.What.Placeholder");

        if (whattf == null)
        {
            Assert.Fail("What text field not found");
        }

        var whatElement = whattf.Find("textarea");
        await whatElement.ChangeAsync(new ChangeEventArgs() { Value = whatstr });

        var wherebytf = textFields.FirstOrDefault(c =>
            c.Instance.Placeholder == "CreateEditStructuredLearningOutcome.Whereby.Placeholder");

        if (wherebytf == null)
        {
            Assert.Fail("Whereby text field not found");
        }

        var wherebyElement = wherebytf.Find("textarea");
        await wherebyElement.ChangeAsync(new ChangeEventArgs() { Value = "Whereby" });

        Assert.That(() => wherebytf.Instance.Value, Is.EqualTo(wherebystr).After(3000, 200));

        var whatfortf = textFields.FirstOrDefault(c =>
            c.Instance.Placeholder == "CreateEditStructuredLearningOutcome.WhatFor.Placeholder");

        if (whatfortf == null)
        {
            Assert.Fail("What for text field not found");
        }

        await whatfortf.Find("textarea").ChangeAsync(new ChangeEventArgs() { Value = whatforstr });

        Assert.That(() => whatfortf.Instance.Value, Is.EqualTo(whatforstr).After(3000, 200));

        var mudBtns = DialogProvider.FindAll("button");
        var submitButton = mudBtns.FirstOrDefault(btns =>
            btns.InnerHtml.Contains("CreateEditStructuredLearningOutcome.Button.Create"));


        Assert.That(submitButton, Is.Not.Null);

        submitButton.Click();

        PresentationLogic.DidNotReceive().AddStructuredLearningOutcome(Arg.Any<LearningOutcomeCollectionViewModel>(),
            Arg.Any<TaxonomyLevel>(), whatstr, Arg.Any<string>(), wherebystr, whatforstr,
            Arg.Any<CultureInfo>());
    }

    [Test]
    // ANF-ID: [AHO01]
    public async Task SubmitWithOutCurrentLearningOutcome_CallsAddStructuredLearningOutcome()
    {
        await GetDialogAsync();

        const string whatstr = "New Outcome";
        const string wherebystr = "Whereby";
        const string whatforstr = "What for";
        const string verb = "folgern";

        var textFields = DialogProvider.FindComponents<MudTextField<string>>();
        var whattf = textFields.FirstOrDefault(c =>
            c.Instance.Placeholder == "CreateEditStructuredLearningOutcome.What.Placeholder");

        if (whattf == null)
        {
            Assert.Fail("What text field not found");
        }

        var whatElement = whattf.Find("textarea");
        await whatElement.ChangeAsync(new ChangeEventArgs() { Value = whatstr });

        Assert.That(() => whattf.Instance.Value, Is.EqualTo(whatstr).After(3000, 200));

        var mudAutocompletes = DialogProvider.FindComponents<MudAutocomplete<string>>();
        var verbAutoComplete = mudAutocompletes.First().Find("input");

        Assert.That(verbAutoComplete, Is.Not.Null);

        await verbAutoComplete.InputAsync(new ChangeEventArgs() { Value = verb });
        await verbAutoComplete.KeyUpAsync(Key.Enter);

        var wherebytf = textFields.FirstOrDefault(c =>
            c.Instance.Placeholder == "CreateEditStructuredLearningOutcome.Whereby.Placeholder");

        if (wherebytf == null)
        {
            Assert.Fail("Whereby text field not found");
        }

        var wherebyElement = wherebytf.Find("textarea");
        await wherebyElement.ChangeAsync(new ChangeEventArgs() { Value = "Whereby" });

        Assert.That(() => wherebytf.Instance.Value, Is.EqualTo(wherebystr).After(3000, 200));

        var whatfortf = textFields.FirstOrDefault(c =>
            c.Instance.Placeholder == "CreateEditStructuredLearningOutcome.WhatFor.Placeholder");

        if (whatfortf == null)
        {
            Assert.Fail("What for text field not found");
        }

        await whatfortf.Find("textarea").ChangeAsync(new ChangeEventArgs() { Value = whatforstr });

        Assert.That(() => whatfortf.Instance.Value, Is.EqualTo(whatforstr).After(3000, 200));

        var mudBtns = DialogProvider.FindAll("button");
        var submitButton = mudBtns.FirstOrDefault(btns =>
            btns.InnerHtml.Contains("CreateEditStructuredLearningOutcome.Button.Create"));

        DialogProvider.Render();

        Assert.That(submitButton, Is.Not.Null);

        submitButton.Click();

        PresentationLogic.Received().AddStructuredLearningOutcome(Arg.Any<LearningOutcomeCollectionViewModel>(),
            Arg.Any<TaxonomyLevel>(), whatstr, verb, wherebystr, whatforstr,
            Arg.Any<CultureInfo>());
    }

    [Test]
    // ANF-ID: [AHO03]
    public async Task SubmitWithCurrentLearningOutcome_CallsEditStructuredLearningOutcome()
    {
        var currentOutcome = new StructuredLearningOutcomeViewModel(TaxonomyLevel.Level1, "Current Outcome",
            "Previous whereby", "Previous what for", "Previous verb", new CultureInfo("de-DE"));
        Outcome = currentOutcome;

        await GetDialogAsync();

        const string whatstr = "New Outcome";
        const string wherebystr = "Whereby";
        const string whatforstr = "What for";
        const string verb = "folgern";

        DialogProvider.Render();
        var whattf = DialogProvider.FindComponentWithMarkup<MudTextField<string>>("textfield-what");
        // var whattf = textFields.FirstOrDefault(c =>
        //     c.Instance.Placeholder == "CreateEditStructuredLearningOutcome.What.Placeholder");

        // if (whattf == null)
        // {
        //     Assert.Fail("What text field not found");
        // }

        var whatElement = whattf.Find("textarea");
        await whatElement.ChangeAsync(new ChangeEventArgs() { Value = whatstr });

        Assert.That(() => whattf.Instance.Value, Is.EqualTo(whatstr).After(3000, 200));

        var mudAutocompletes = DialogProvider.FindComponents<MudAutocomplete<string>>();
        var verbAutoComplete = mudAutocompletes.First().Find("input");

        Assert.That(verbAutoComplete, Is.Not.Null);

        await verbAutoComplete.InputAsync(new ChangeEventArgs() { Value = verb });
        await verbAutoComplete.KeyUpAsync(Key.Enter);

        var wherebytf = DialogProvider.FindComponentWithMarkup<MudTextField<string>>("textfield-whereby");
        // var wherebytf = textFields.FirstOrDefault(c =>
        //     c.Instance.Placeholder == "CreateEditStructuredLearningOutcome.Whereby.Placeholder");

        // if (wherebytf == null)
        // {
        //     Assert.Fail("Whereby text field not found");
        // }

        var wherebyElement = wherebytf.Find("textarea");
        await wherebyElement.ChangeAsync(new ChangeEventArgs() { Value = "Whereby" });

        Assert.That(() => wherebytf.Instance.Value, Is.EqualTo(wherebystr).After(3000, 200));

        var whatfortf = DialogProvider.FindComponentWithMarkup<MudTextField<string>>("textfield-whatfor");
        // var whatfortf = textFields.FirstOrDefault(c =>
        //     c.Instance.Placeholder == "CreateEditStructuredLearningOutcome.WhatFor.Placeholder");

        // if (whatfortf == null)
        // {
        //     Assert.Fail("What for text field not found");
        // }

        await whatfortf.Find("textarea").ChangeAsync(new ChangeEventArgs() { Value = whatforstr });

        Assert.That(() => whatfortf.Instance.Value, Is.EqualTo(whatforstr).After(3000, 200));

        var mudBtns = DialogProvider.FindAll("button");
        var submitButton = mudBtns.FirstOrDefault(btns =>
            btns.InnerHtml.Contains("CreateEditStructuredLearningOutcome.Button.Edit"));

        Assert.That(submitButton, Is.Not.Null);

        submitButton.Click();

        PresentationLogic.Received().EditStructuredLearningOutcome(Arg.Any<LearningOutcomeCollectionViewModel>(),
            Outcome,
            Arg.Any<TaxonomyLevel>(), whatstr, verb, wherebystr, whatforstr,
            Arg.Any<CultureInfo>());
    }

    [Test]
    public async Task Reset_ResetsAllFields()
    {
        await GetDialogAsync();

        const string whatstr = "New Outcome";
        const string wherebystr = "Whereby";
        const string whatforstr = "What for";
        const string verb = "folgern";

        var textFields = DialogProvider.FindComponents<MudTextField<string>>();
        var whattf = textFields.FirstOrDefault(c =>
            c.Instance.Placeholder == "CreateEditStructuredLearningOutcome.What.Placeholder");

        if (whattf == null)
        {
            Assert.Fail("What text field not found");
        }

        var whatElement = whattf.Find("textarea");
        await whatElement.ChangeAsync(new ChangeEventArgs() { Value = whatstr });

        Assert.That(() => whattf.Instance.Value, Is.EqualTo(whatstr).After(3000, 200));

        var mudAutocompletes = DialogProvider.FindComponents<MudAutocomplete<string>>();
        var verbAutoComplete = mudAutocompletes.First().Find("input");

        Assert.That(verbAutoComplete, Is.Not.Null);

        await verbAutoComplete.InputAsync(new ChangeEventArgs() { Value = verb });
        await verbAutoComplete.KeyUpAsync(Key.Enter);

        var wherebytf = textFields.FirstOrDefault(c =>
            c.Instance.Placeholder == "CreateEditStructuredLearningOutcome.Whereby.Placeholder");

        if (wherebytf == null)
        {
            Assert.Fail("Whereby text field not found");
        }

        var wherebyElement = wherebytf.Find("textarea");
        await wherebyElement.ChangeAsync(new ChangeEventArgs() { Value = "Whereby" });

        Assert.That(() => wherebytf.Instance.Value, Is.EqualTo(wherebystr).After(3000, 200));

        var whatfortf = textFields.FirstOrDefault(c =>
            c.Instance.Placeholder == "CreateEditStructuredLearningOutcome.WhatFor.Placeholder");

        if (whatfortf == null)
        {
            Assert.Fail("What for text field not found");
        }

        await whatfortf.Find("textarea").ChangeAsync(new ChangeEventArgs() { Value = whatforstr });

        Assert.That(() => whatfortf.Instance.Value, Is.EqualTo(whatforstr).After(3000, 200));

        var mudBtns = DialogProvider.FindAll("button");
        var resetButton = mudBtns.FirstOrDefault(btns =>
            btns.InnerHtml.Contains("CreateEditStructuredLearningOutcome.Reset"));

        Assert.That(resetButton, Is.Not.Null);

        resetButton.Click();

        Assert.That(() => whattf.Instance.Value, Is.EqualTo(string.Empty).After(3000, 200));
        Assert.That(() => wherebytf.Instance.Value, Is.EqualTo(string.Empty).After(3000, 200));
        Assert.That(() => whatfortf.Instance.Value, Is.EqualTo(string.Empty).After(3000, 200));
    }

    private DialogParameters GetDialogParameters(
        LearningOutcomeCollectionViewModel? collection = null, StructuredLearningOutcomeViewModel? outcome = null)
    {
        collection ??= new LearningOutcomeCollectionViewModel();
        return new DialogParameters<CreateEditStructuredLearningOutcome>
        {
            { nameof(CreateEditStructuredLearningOutcome.LearningOutcomes), collection },
            { nameof(CreateEditStructuredLearningOutcome.CurrentLearningOutcome), outcome },
            { nameof(CreateEditStructuredLearningOutcome.DebounceInterval), 0 }
        };
    }
}