# Internship

## 

### Как запустить

#### CRM состоит из двух приложений, которые должны работать одновременно:

- `CRM.Api` — ASP.NET Core API, SQL Server persistence, Swagger
- `CRM.WebBlazor` — Blazor Server web interface

#### Предусловия

- [.NET SDK 9.0.123](https://dotnet.microsoft.com/download/dotnet/9.0) (необходимая версия в `CRM/global.json`)
- Запущенная база данных SQL Server
- Запущенный Mailpit сервер

Для выполнения двух последних пунктов из вышеуказаного из директории Internship выполнить:

```bash
docker compose up -d
```

#### 1. Восстановление зависимостей

Из корневой директории(Internship) выполнить следующее:

```bash
cd CRM
dotnet restore CRM.sln
```

[ТОЛЬКО ЕСЛИ ЭТО НЕОБХОДИМО] Довериться development-сертификатам
ASP.NET Core для запуска https-профиля приложений. Для этого выполнить:

```bash
dotnet dev-certs https --trust
```

#### 2. Настроить API

API нужны строка подключения к БД и другие нестройки, не попадающие в коммпиты. Поэтому необходимо настроить dotnet user-secrets(secrest.json):

```bash
dotnet user-secrets set "ConnectionStrings:ApplicationDbContextConnection" "<YOUR_CONNECTION_STRING>" --project CRM.Api/CRM.Api.csproj

dotnet user-secrets set "EmailConfiguration:FromEmail" "<SENDER_EMAIL>" --project CRM.Api/CRM.Api.csproj

dotnet user-secrets set "EmailConfiguration:SmtpServer" "<SMTP_HOST>" --project CRM.Api/CRM.Api.csproj
```
Либо вручную вставить необходимые значения напрямую в <b>secrets.json</b> файл

#### 3. Создание необходимых объектов БД

Необходимо применить миграции. Для этого можно выполнить следующую команду:

```bash
dotnet ef database update \
  --project CRM.DataAccess/CRM.DataAccess.csproj \
  --startup-project CRM.Api/CRM.Api.csproj
```

Или использовать соответствующие инструменты IDE

#### 4. Запустить приложения

Как было описано выше, API и WebBlazor приложения должны быть запущены вместе.

Для этого можно в отдельном терминали запустить API, для чего использовать следующую команду:

```bash
cd CRM
dotnet run --project CRM.Api/CRM.Api.csproj --launch-profile https
```

API работает этому URI: `https://localhost:7250`; А Swagger доступен по этому URI: `https://localhost:7250/swagger`.

Далее можно в отдельном терминали запустить WebBlazor приложение для чего использовать следующую команду:

```bash
cd CRM
dotnet run --project CRM.WebBlazor/CRM.WebBlazor.csproj --launch-profile https
```

WebBlazor приложение доступно по следующему URI:  `https://localhost:7105`.


#### <b>Либо можно использовать средства IDE для одновременного запуска обоих приложений с https профилями<b>
