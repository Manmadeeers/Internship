using System.Globalization;
using CRM.WebBlazor.Components;
using CRM.WebBlazor.Service;
using Microsoft.AspNetCore.Localization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveServerComponents();

var apiBaseAddress = builder.Configuration["ApiBaseAddress"];


builder.Services.AddMudServices();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
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
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress!) });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
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