using MG.DataStorage.Business.Handlers;
using MG.DataStorage.Business.Services;
using MG.DataStorage.Core.Interfaces;
using MG.DataStorage.Infrastructure.Caching;
using MG.DataStorage.Infrastructure.Configuration;
using MG.DataStorage.Infrastructure.Database;
using MG.DataStorage.Infrastructure.FileStorage;
using MG.DataStorage.Core.DTOs;

var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

var builder = WebApplication.CreateBuilder(args);


// Config binding
builder.Services.Configure<CacheSettings>(configuration.GetSection(CacheSettings.CONFIG_NAME));
builder.Services.Configure<FileStorageSettings>(configuration.GetSection(FileStorageSettings.CONFIG_NAME));
builder.Services.Configure<SecuritySettings>(configuration.GetSection(SecuritySettings.CONFIG_NAME));
builder.Services.Configure<PostgreSqlSettings>(builder.Configuration.GetSection("PostgreSqlSettings"));
//

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// cache strategy services
builder.Services.AddSingleton<InMemoryCacheService>();
builder.Services.AddSingleton<RedisCacheService>();
builder.Services.AddSingleton<HybridCacheService>();

// File + DB
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IDataRepository, PostgreSqlDataService>();

// Factory
builder.Services.AddScoped<IDataProviderFactory, DataProviderFactory>();

// Final service abstraction for controllers
builder.Services.AddScoped<IDataRetrievalService>(sp =>
{
    var factory = sp.GetRequiredService<IDataProviderFactory>();

    // Composing the chain
    var memory = new CacheHandler(factory.CreateDataSource(DataSource.Cache));
    var file = new FileHandler(factory.CreateDataSource(DataSource.File));
    var db = new PostgreSqlHandler(factory.CreateDataSource(DataSource.Database));

    memory.SetNext(file).SetNext(db);

    // Wrap chain in service abstraction
    return new DataRetrievalService(memory);
});



builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();