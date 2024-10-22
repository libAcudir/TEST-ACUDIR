
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Interfaces;
using Services;
using Services.Interfaces;
using ServiceStack;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddAutoMapper(typeof(Mapping));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Acudir.Test", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
var dataTest = System.IO.File.ReadAllText(@"Test.json");

IWebHostEnvironment environment = app.Environment;
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