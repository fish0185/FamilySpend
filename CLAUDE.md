# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

FamilySpend is a single-project ASP.NET Core 10 Web API for family allowance / spend control.
A **primary user** (role `PrimaryUser`) holds a balance, invites family **sub-accounts**,
allocates funds to them, and restricts which spending categories each sub-account may use.
Sub-accounts record transactions against their own balance; spends are checked against their
allowed categories.

Every user — primary or sub — owns exactly one `Loan` row, which acts as their wallet.
Moving money means mutating `Loan.Balance`.

## Commands

Run the API (serves `http://localhost:5250`, `https://localhost:7293`; `ASPNETCORE_ENVIRONMENT=Development`):

```bash
dotnet run --project FamilySpend
dotnet build FamilySpend.sln
```

Swagger UI and `/openapi/v1.json` are mapped in Development only.

EF Core migrations (requires `dotnet tool install --global dotnet-ef`, and `~/.dotnet/tools` on PATH):

```bash
dotnet ef migrations add <Name> --context FamilySpendDbContext -o Persistence/Migrations
dotnet ef database update --context FamilySpendDbContext
```

Docker:

```bash
docker build -t fish0185/familyspend -f Dockerfile .   # from repo root
docker compose up                                       # see compose.yaml
```

`compose.yaml` runs two prebuilt images: `fish0185/familyspenddb` as container **`ps`**
(port `55432:5432`) and `fish0185/familyspend` as container **`fs`** (port `8000:8080`).
There is no `build:` stanza, so `docker compose up` does not rebuild from the Dockerfile.

## Architecture

Vertical-slice CQRS via **MediatR**. There are no repositories, no service layer, no
AutoMapper, and no FluentValidation. `FamilySpend/Services/` is declared in the `.csproj`
but is empty.

```
FamilySpend/
├── Program.cs                  # top-level statements; all DI + pipeline config
├── GlobalExceptionHandler.cs
├── StatusCodeHandlerMiddleware.cs / StatusCodeMiddlewareExtensions.cs
├── Controllers/                # 5 thin controllers
├── App/<Name>Command/          # one folder per slice: command + handler + response DTO
├── Infra/Context/              # FamilySpendDbContext
├── Infra/Entities/             # domain entities
└── Persistence/Migrations/     # init, update, update2, update3 + model snapshot
```

Controllers are deliberately thin — they read the caller's id from the claims, stamp it onto
the command, send it, and return `Ok()`:

```csharp
var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
command.UserId = nameIdentifier;
await mediator.Send(command, cancellationToken);
return Ok();
```

MediatR is registered by assembly scan: `RegisterServicesFromAssembly(typeof(Program).Assembly)`,
so a new handler is wired up automatically with no registration step.

### Conventions when adding a slice

Copy `FamilySpend/App/CreateTransactionCommand/CreateTransactionCommandHandler.cs` — it is the
canonical example.

- Create a folder `FamilySpend/App/<Name>Command/`. **The folder name equals the class name**,
  which is why namespaces read `FamilySpend.App.InvitationCommand.InvitationCommand`.
- The command implements `IRequest` (no result) or `IRequest<TResponse>`. Commands double as
  request DTOs bound from the request body.
- `UserId` on a command is `string?` because the controller overwrites it after model binding —
  never trust a caller-supplied user id.
- Queries are also named `...Command` (`GetCategoriesCommand`, `GetCurrentUserCommand`,
  `GetTransactionsCommand`). Keep that naming for consistency.
- Handlers use **primary constructors** and inject `FamilySpendDbContext`,
  `UserManager<ZipUser>`, and/or `RoleManager<IdentityRole>` directly.
- Business rules are enforced with `throw new InvalidOperationException("message")`.
- Response DTOs are hand-mapped with LINQ `Select` in the handler.
- Async EF calls take the `CancellationToken` through to `SaveChangesAsync`.

## Domain model

All entities live in `FamilySpend/Infra/Entities/`. Note the vocabulary is
ZipUser / Loan / FamilyLink / Order / Transaction / OrderCategory — **not** Family / Member / Expense.

| Type | Key properties |
|---|---|
| `BaseEntity` (abstract) | `int Id`, `DateTimeOffset Created`, `DateTimeOffset? LastModified` |
| `ZipUser : IdentityUser` | `bool IsPrimary`, `Loan Loan` |
| `Loan : BaseEntity` | `decimal Balance`, `string UserId`, `[ForeignKey] ZipUser` — the user's wallet |
| `FamilyLink : BaseEntity` | `UserId` (primary) → `FamilyUserId` (sub) |
| `Order : BaseEntity` | `ItemDescription`, `MerchantName` |
| `Transaction : BaseEntity` | `OrderId`, `UserId`, `decimal Amount`, `TransactionType`, `Order` |
| `TransactionType` (enum) | `Credit = 0`, `Debit = 1` |
| `OrderCategory : BaseEntity` | `Name`, `Description` |
| `UserOrderCategory : BaseEntity` | `UserId`, `OrderCategoryId` — per-sub-account allow-list |

Relationships actually configured: `ZipUser` 1—1 `Loan` (FK `Loans.UserId`, unique, cascade) and
`Transaction` *—1 `Order` (FK `Transactions.OrderId`, cascade). `FamilyLink.UserId` /
`FamilyUserId`, `Transaction.UserId`, and `UserOrderCategory.UserId` are plain strings with
indexes but **no FK constraints**. `Transaction` has no link to `OrderCategory`.

`OnModelCreating` in `FamilySpend/Infra/Context/FamilySpendDbContext.cs` configures only: the
`FamilyLink` primary key, an index on `FamilyLink.UserId`, a unique index on
`(UserId, FamilyUserId)`, and a unique index on `UserOrderCategory (UserId, OrderCategoryId)`.

Behaviours worth knowing:

- A new primary user is seeded with `Loan.Balance = 1000`, and the `PrimaryUser` role is
  created on demand.
- An invited sub-account is created with the hard-coded password **`Test.1234`** and
  `Loan.Balance = 0`.
- `AllocateFunding` moves money between loans; a negative amount claws funds back.
- `CreateTransaction` validates the category only for non-primary users; `Debit` checks the
  balance then subtracts, `Credit` adds.
- `RemoveFamily` folds the sub-account's balance into the primary's loan and deletes the
  `FamilyLink`; the sub `ZipUser` and `Loan` rows remain.

## API surface

All controllers are `[ApiController]` with `[Route("api/[controller]")]`.

| Verb | Route | Auth |
|---|---|---|
| POST | `/api/User` | `[AllowAnonymous]` |
| GET | `/api/User` | `[Authorize]` |
| POST | `/api/Invitation` | `Roles = "PrimaryUser"` |
| POST | `/api/Invitation/remove` | `Roles = "PrimaryUser"` |
| POST | `/api/Funding/allocate` | `Roles = "PrimaryUser"` |
| POST | `/api/Funding/add/{amount:int}` | `Roles = "PrimaryUser"` |
| GET | `/api/OrderCategory` | `[Authorize]` |
| POST | `/api/OrderCategory/user` | `Roles = "PrimaryUser"` |
| DELETE | `/api/OrderCategory/user?email=&category=` | `Roles = "PrimaryUser"` |
| GET | `/api/Transaction` | `[Authorize]` |
| POST | `/api/Transaction` | `[Authorize]` |

`GET /api/OrderCategory` returns all categories. `GET /api/Transaction` returns only the
caller's own transactions, with `Include(Order)`.

`app.MapIdentityApi<ZipUser>()` additionally maps the standard Identity endpoints at the
**root** (not under `/api`): `/register`, `/login`, `/refresh`, `/confirmEmail`,
`/resendConfirmationEmail`, `/forgotPassword`, `/resetPassword`, `/manage/2fa`, `/manage/info`.

## Request pipeline

Configured in `FamilySpend/Program.cs`, in this order:

`UseExceptionHandler` → `UseStatusCodeHandler` (custom) → `UseCors("AllowAll")` →
`UseAuthorization` → `UseStatusCodePages` → `MapControllers` → `MapIdentityApi` →
(Development only) `MapOpenApi` / `UseSwagger` / `UseSwaggerUI` → `UseHttpsRedirection`.

- `GlobalExceptionHandler.cs` — an `IExceptionHandler` registered via
  `AddExceptionHandler<GlobalExceptionHandler>()` + `AddProblemDetails()`. Maps
  `ArgumentException` → 400 and everything else → 500, writing a `ProblemDetails` body.
  Because handlers signal rule violations with `InvalidOperationException`, business-rule
  failures surface as **HTTP 500** with the message in `Detail`.
- `StatusCodeHandlerMiddleware.cs` — on the return path, rewrites empty 401/403 responses
  into `application/problem+json`. Exposed by the `UseStatusCodeHandler()` extension in
  `StatusCodeMiddlewareExtensions.cs`.
- CORS policy `"AllowAll"` permits any origin, method, and header.

## Configuration notes

- **The database connection string is hard-coded in `Program.cs`**, not read from config.
  `ConnectionStrings:AppDb` in `appsettings.json` exists but is never used — nothing calls
  `GetConnectionString`. Switching between local and Docker means editing the source: use
  `Host=localhost` to run against a local Postgres, `Host=ps` to run against the compose
  container. Running `dotnet run` while the string says `Host=ps` will fail to resolve the host.
- **Migrations are applied manually.** There is no `Database.Migrate()` call at startup, so
  run `dotnet ef database update` before first use.
- **`OrderCategories` has no code-based seed data.** Categories must be inserted by hand
  (the prebuilt `fish0185/familyspenddb` image carries them).
- `FamilySpend/FamilySpend.http` is stale scaffolding — it still targets a
  `/weatherforecast/` endpoint that no longer exists.
- `FamilySpend/Services/` is declared in the `.csproj` but empty.
- Both `AddOpenApi()` and `AddSwaggerGen()` are registered, so two OpenAPI stacks are present.
- `FamilySpend/Readme.txt` holds the original setup notes, including the `docker network` /
  `docker run` variant of the compose setup.

## Testing

There is **no test project** and no test framework package anywhere in the repo. Verification
is manual: run the API and exercise it through Swagger UI (Development only) or an HTTP client.
Register or create a primary user first, then `POST /login` to obtain a token, since every
route except `POST /api/User` requires authentication.
