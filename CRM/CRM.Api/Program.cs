using System.Diagnostics;
using System.Text;
using CRM.Api.AuthorizationConstraints;
using CRM.DataAccess;
using CRM.Model.IdentityModels;
using CRM.Service.Identity;
using CRM.Utility;
using CRM.Utility.IUtility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using IAuthenticationService = CRM.Service.Identity.IAuthenticationService;
using TokenHandler = CRM.Utility.TokenHandler;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ??
                       throw new InvalidOperationException(
                           "Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CRM API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.AddScoped<IApplicationEmailSender, ApplicationEmailSender>();

builder.Services.Configure<TokenConfiguration>(builder.Configuration.GetSection("TokenConfiguration"));
builder.Services.AddSingleton<ITokenHandler,TokenHandler>();

builder.Services.AddScoped<IUserService,UserService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme= JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
        ValidIssuer= builder.Configuration["TokenConfiguration:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenConfiguration:SecretKey"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddTransient<IAuthorizationHandler, AgeHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AgeLimit",policy=>policy.Requirements.Add(new AgeRequirement(18)));
});

var app = builder.Build();

app.Use(async (context, next) =>
{  
    var sw = Stopwatch.StartNew();
    await next();
    Console.WriteLine($"{context.Request.Path} {sw.ElapsedMilliseconds} ms");  
});
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API V1"); });

    var url = "https://localhost:7250/swagger";
    Process.Start(new ProcessStartInfo
    {
        FileName = url,
        UseShellExecute = true
    });
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    "areas",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.Run();