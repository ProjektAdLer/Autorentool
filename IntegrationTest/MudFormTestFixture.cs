using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using TestHelpers;

namespace IntegrationTest;

public class MudFormTestFixture<TComponent, TForm, TEntity> : MudBlazorTestFixture<BaseForm<TForm, TEntity>> where TForm : new()
{
    protected ISnackbar Snackbar { get; set; }
    protected IValidator<TEntity> Validator { get; set; }
    
    [SetUp]
    public void Setup()
    {
        Snackbar = Substitute.For<ISnackbar>();
        Validator = Substitute.For<IValidator<TEntity>>();
        Context.AddLocalizerForTest<TComponent>();
        Context.Services.AddSingleton(Snackbar);
        Context.Services.AddSingleton(Validator);
    }
}