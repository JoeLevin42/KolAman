

using DashbordApi.Configuraion;
using DashbordApi.Services;
using MongoDB.Driver;

var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.


//my-services:
var mongoConfig = new MongoConfiguration
{
    ConnectionString = configuration["Mongo:ConnectionString"]!,
    DatabaseName = configuration["Mongo:DatabaseName"]!
};
builder.Services.AddSingleton(mongoConfig);
builder.Services.AddSingleton<MongoService>();

builder.Services.AddControllers();
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
