using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NUnit.Framework;
using Presentation.Components.Forms.Models;

namespace IntegrationTest.Forms.World;

public static class WorldFormStaticTestMethods
{
    public static async Task SetAllInputAndTextareaValueToExpected(
        IReadOnlyList<IRenderedComponent<MudTextField<string>>> mudInputs, string expected)
    {
        foreach (var mudInput in mudInputs)
        {
            var inputs = mudInput.FindAll("input", false);
            if (inputs.Any())
            {
                await inputs[0].ChangeAsync(new ChangeEventArgs
                {
                    Value = expected
                });
            }
            else
            {
                var textareas = mudInput.FindAll("textarea", false);
                if (textareas.Any())
                {
                    await textareas[0].ChangeAsync(new ChangeEventArgs
                    {
                        Value = expected
                    });
                }
            }
        }
    }

    public static void AssertAllFieldsAreSetToValue(LearningWorldFormModel formModel, string expected)
    {
        Assert.Multiple(() =>
        {
            Assert.That(() => formModel.Name, Is.EqualTo(expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => formModel.Shortname, Is.EqualTo(expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => formModel.Authors, Is.EqualTo(expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => formModel.Language, Is.EqualTo(expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => formModel.Description, Is.EqualTo(expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => formModel.EvaluationLink, Is.EqualTo(expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => formModel.EvaluationLinkName, Is.EqualTo(expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => formModel.EvaluationLinkText, Is.EqualTo(expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => formModel.EnrolmentKey, Is.EqualTo(expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => formModel.StoryStart, Is.EqualTo(expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => formModel.StoryEnd, Is.EqualTo(expected).After(3).Seconds.PollEvery(250));
        });
    }
}