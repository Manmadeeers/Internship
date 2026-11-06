# Internship

### How to run

The CRM consists of two applications that must run at the same time:

- `CRM.Api` — ASP.NET Core API, SQL Server persistence, and Swagger
- `CRM.WebBlazor` — Blazor Server web interface

#### Prerequisites

- [.NET SDK 9.0.123](https://dotnet.microsoft.com/download/dotnet/9.0) (the required version is pinned in `CRM/global.json`)
- A running SQL Server instance
- An SMTP account if registration and email confirmation need to work

#### 1. Restore the dependencies

Run the following commands from the repository root:

```bash
cd CRM
dotnet restore CRM.sln
```

[IF NOT DONE YET]Trust the ASP.NET Core development certificate so the web application can call the API over HTTPS:

```bash
dotnet dev-certs https --trust
```

#### 2. Configure the API

The API reads its database connection and SMTP credentials from configuration. Store local credentials with .NET user-secrets so they are not committed to Git:

```bash
dotnet user-secrets set "ConnectionStrings:ApplicationDbContextConnection" "<YOUR_CONNECTION_STRING>" --project CRM.Api/CRM.Api.csproj

dotnet user-secrets set "EmailConfiguration:FromName" "CRM" --project CRM.Api/CRM.Api.csproj
dotnet user-secrets set "EmailConfiguration:FromEmail" "<SENDER_EMAIL>" --project CRM.Api/CRM.Api.csproj
dotnet user-secrets set "EmailConfiguration:SmtpServer" "<SMTP_HOST>" --project CRM.Api/CRM.Api.csproj
dotnet user-secrets set "EmailConfiguration:Port" "587" --project CRM.Api/CRM.Api.csproj
dotnet user-secrets set "EmailConfiguration:UserName" "<SMTP_USERNAME>" --project CRM.Api/CRM.Api.csproj
dotnet user-secrets set "EmailConfiguration:Password" "<SMTP_PASSWORD>" --project CRM.Api/CRM.Api.csproj
```

Replace the placeholder values with credentials for your SQL Server and SMTP provider. SMTP configuration is only required for the email-confirmation flow.

#### 3. Create the database

Apply the included migrations(OR USE IDE TOOLS TO DO SO):

```bash
dotnet ef database update \
  --project CRM.DataAccess/CRM.DataAccess.csproj \
  --startup-project CRM.Api/CRM.Api.csproj
```

#### 4. Start the applications

Start the API in one terminal(OR USE IDE TOOLS TO RUN):

```bash
cd CRM
dotnet run --project CRM.Api/CRM.Api.csproj --launch-profile https
```

The API listens at `https://localhost:7250`; Swagger is available at `https://localhost:7250/swagger`.

Start the Blazor application in a second terminal(OR USE IDE TOOLS TO RUN):

```bash
cd CRM
dotnet run --project CRM.WebBlazor/CRM.WebBlazor.csproj --launch-profile https
```

Open `https://localhost:7105` in a browser. Keep both processes running while using the application; stop either process with `Ctrl+C`.
