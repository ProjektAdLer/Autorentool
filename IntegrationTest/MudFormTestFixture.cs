using AutoMapper;
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
    protected IMapper Mapper { get; set; }
    protected IFormDataContainer<TForm, TEntity> FormDataContainer { get; set; }
    protected TEntity Entity { get; set; }
    protected TForm FormModel { get; set; }
    
    [SetUp]
    public void Setup()
    {
        Snackbar = Substitute.For<ISnackbar>();
        Validator = Substitute.For<IValidationWrapper<TEntity>>();
        Mapper = Substitute.For<IMapper>();
        FormModel = FormModelProvider.Get<TForm>();
        FormDataContainer = new FormDataContainer<TForm, TEntity>(Mapper, FormModel);
        Entity = EntityProvider.Get<TEntity>();
        Mapper.Map<TForm, TEntity>(FormModel).Returns(Entity);
        Context.Services.AddSingleton(Snackbar);
        Context.Services.AddSingleton(Validator);
        Context.Services.AddSingleton(Mapper);
        Context.Services.AddSingleton(FormDataContainer);
        Context.AddLocalizerForTest<TComponent>();
        Context.AddLocalizerFactoryForTest();
    }

    [TearDown]
    public void TearDown()
    {
        Snackbar.Dispose();
    }
}