using Asp.Versioning;
using Marten;
using MartenEventSourcing.Domain.Models;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.DefaultApiVersion = ApiVersion.Default;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetValue<string>("ConnectionStrings:PostgresDB"));
    options.UseSystemTextJsonForSerialization();
    options.Projections.Add<PolicyProjection>(Marten.Events.Projections.ProjectionLifecycle.Inline);
    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All;
    }
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//For Docker use
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
