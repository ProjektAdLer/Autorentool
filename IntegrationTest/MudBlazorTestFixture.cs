using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using PresentationTest;
using TestContext = Bunit.TestContext;

namespace IntegrationTest;

public class MudBlazorTestFixture<T> where T : ComponentBase
{
    protected IStringLocalizer<T> Localizer { get; set; }
    protected TestContext Context { get; set; }
    
    protected IRenderedComponent<MudDialogProvider> DialogProvider { get; set; }

    [SetUp]
    public void Setup()
    {
        Context = new TestContext();
        Context.AddMudBlazorTestServices();
        Localizer = Substitute.For<IStringLocalizer<T>>();
        Localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        Localizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(ci =>
            new LocalizedString(ci.Arg<string>() + string.Concat(ci.Arg<object[]>()),
                ci.Arg<string>() + string.Concat(ci.Arg<object[]>())));
        Context.Services.AddSingleton(Localizer);
        DialogProvider = Context.RenderComponent<MudDialogProvider>();
    }

    protected async Task<IDialogReference> OpenDialogAndGetDialogReferenceAsync(string? title = null, DialogOptions? options = null, DialogParameters? parameters = null)
    {
        var service = (DialogService)Context.Services.GetService<IDialogService>()!;
        IDialogReference? reference = null;
        if (title is null)
        {
            if(parameters is null)
                await DialogProvider.InvokeAsync(() => reference = service.Show<T>());
            else
            {
                title = "foo";
                await DialogProvider.InvokeAsync(() => reference = service.Show<T>(title, parameters));
            }
        }
        else
        {
            if(options is null)
                await DialogProvider.InvokeAsync(() => reference = service.Show<T>(title));
            else
            {
                if (parameters is null)
                    await DialogProvider.InvokeAsync(() => reference = service.Show<T>(title, options));
                else
                    await DialogProvider.InvokeAsync(() =>
                        reference = service.Show<T>(title, parameters, options));
            }
        }
        return reference!;
    }
}