using AutomatedPassengerBoardingKiosk.API.Contexts;
using AutomatedPassengerBoardingKiosk.API.Services;
using AutomatedPassengerBoardingKiosk.API.Services.Interfaces;
using AutomatedPassengerBoardingKiosk.API.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FlightManifestContext>(options =>
   options.UseSqlServer(defaultConnectionString));

builder.Services.AddScoped<IFormRecognizerService, FormRecognizerService>();
builder.Services.AddScoped<IFaceDetectionService, FaceDetectionService>();
builder.Services.AddScoped<IFlightManifestService, FlightManifestService>();

var azureSettings = new AzureSettings();
builder.Configuration.Bind("AzureSettings", azureSettings);
builder.Services.AddSingleton(azureSettings);

var serviceProvider = builder.Services.BuildServiceProvider();
try
{
    var dbContext = serviceProvider.GetRequiredService<FlightManifestContext>();
    dbContext.Database.Migrate();
}
catch
{
}

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
