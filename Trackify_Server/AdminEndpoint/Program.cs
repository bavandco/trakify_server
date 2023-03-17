using AdminEndpoint.Token;
using Application.Interfaces.Contexts;
using Application.Repositories;
using Infrastructure.IdentityConfig;
using MainEndpoint.Extensions;

using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
string connection = Configuration["ConnectionString:SqlServer"];
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));
builder.Services.AddIdentityService(Configuration);
builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
builder.Services.AddTransient<CreateToken, CreateToken>();
builder.Services.AddTransient<UserTokenRepository, UserTokenRepository>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
