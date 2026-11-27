using System.Globalization;
using CRM.WebBlazor.Components;
using CRM.WebBlazor.Service;
using CRM.WebBlazor.Service.Customers;
using CRM.WebBlazor.Service.Identity;
using Microsoft.AspNetCore.Localization;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveServerComponents();

var apiBaseAddress = builder.Configuration["ApiBaseAddress"];
builder.Services.AddHttpClient("DefaultClient", client =>
{
    client.BaseAddress = new Uri(apiBaseAddress!);
});
builder.Services.AddHttpClient("SecureClient", client =>
{
    client.BaseAddress = new Uri(apiBaseAddress!);
});

builder.Services.AddMudServices();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IErrorHandlingService,ErrorHandlingService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<AuthenticatedHttpClientService>();
builder.Services.AddScoped<TokenStore>();
builder.Services.AddScoped<RefreshTokenHandler>();
builder.Services.AddScoped<ICustomersService,CustomersService>();


builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
});


builder.Services.AddLocalization();
var supportedCultures = new[]
{
    new CultureInfo("en"), 
    new CultureInfo("ru")
};
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

if (! app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();