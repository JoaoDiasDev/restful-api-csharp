using EvolveDb;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using restful_api_joaodias.Business;
using restful_api_joaodias.Business.Implementations;
using restful_api_joaodias.Hypermedia.Enricher;
using restful_api_joaodias.Hypermedia.Filters;
using restful_api_joaodias.Model.Context;
using restful_api_joaodias.Repository.Generic;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddXmlSerializerFormatters();

// Adding Hypermedia, HATEOAS Support
var filterOptions = new HyperMediaFilterOptions();
filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
filterOptions.ContentResponseEnricherList.Add(new BookEnricher());
builder.Services.AddSingleton(filterOptions);

builder.Services.AddApiVersioning();
builder.Services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
builder.Services.AddScoped<IBookBusiness, BookBusinessImplementation>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

Log.Logger = new LoggerConfiguration()
    .WriteTo
    .Console()
    .CreateLogger();

var connection = builder.Configuration.GetSection("MySQLConnection")["MySQLConnectionString"];
builder.Services
    .AddDbContext<MySQLContext>(options => options.UseMySql(connection, MySqlServerVersion.AutoDetect(connection)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
void MigrateDatabase(string? connection)
{
    try
    {
        var evolveConnection = new MySqlConnection(connection);
        var evolve = new Evolve(evolveConnection, msg => Log.Information(msg))
        {
            Locations = new List<string> { "db/migrations", "db/dataset" },
            IsEraseDisabled = true
        };
        evolve.Migrate();
    }
    catch (Exception ex)
    {
        Log.Error("Database migration failed", ex);
        throw;
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    MigrateDatabase(connection);
    app.UseSwagger();
    app.UseSwaggerUI();
};

//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
});

app.Run();
