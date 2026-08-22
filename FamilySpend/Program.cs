using FamilySpend.App.InvitationCommand;
using FamilySpend.Infra.Context;
using FamilySpend.Infra.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FamilySpendDbContext>(
    options => options.UseNpgsql("Host=localhost;Port=5432;Database=FamilySpend;Username=postgres;Password=postgres;"));

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ZipUser>()
    .AddEntityFrameworkStores<FamilySpendDbContext>();

// Create and configure mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

app.UseAuthorization();

// For API routing
app.MapControllers(); 
app.MapIdentityApi<ZipUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();