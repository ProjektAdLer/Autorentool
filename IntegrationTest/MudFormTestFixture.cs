using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using TestHelpers;

namespace IntegrationTest;

public class MudFormTestFixture<TComponent, TForm, TEntity> : MudBlazorTestFixture<BaseForm<TForm, TEntity>> where TForm : class, new() where TEntity : class
{
    protected ISnackbar Snackbar { get; set; }
    protected IValidationWrapper<TEntity> Validator { get; set; }
    protected IFormDataContainer<TForm, TEntity> FormDataContainer { get; set; }
    protected TEntity Entity { get; set; }
    protected TForm FormModel { get; set; }
    
    [SetUp]
    public void Setup()
    {
        Snackbar = Substitute.For<ISnackbar>();
        Validator = Substitute.For<IValidationWrapper<TEntity>>();
        Context.AddLocalizerForTest<TComponent>();
        FormDataContainer = Substitute.For<IFormDataContainer<TForm, TEntity>>();
        FormModel = FormModelProvider.Get<TForm>();
        Entity = EntityProvider.Get<TEntity>();
        FormDataContainer.FormModel.Returns(FormModel);
        FormDataContainer.GetMappedEntity().Returns(Entity);
        Context.Services.AddSingleton(Snackbar);
        Context.Services.AddSingleton(Validator);
        Context.Services.AddSingleton(FormDataContainer);
    }
}