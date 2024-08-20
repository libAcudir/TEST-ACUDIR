using Acudir.Test.Apis.Extra.Filters;
using Acudir.Test.Apis.Extra.Responses;
using Acudir.Test.Apis.Interfaces.IRepositorys;
using Acudir.Test.Apis.Models.CustomEntities;
using Acudir.Test.Apis.Models.Entities;
using Acudir.Test.Apis.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    // Filters
    options.Filters.Add<GlobalExceptionFilter>();
});

// Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(CustomValidationResponse));
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//"DB"
builder.Services.Configure<DatabaseJsonOption>(builder.Configuration.GetSection("DatabaseJsonOption"));

// Repo
builder.Services.AddSingleton<IPersonaRepository>(serviceProvider =>
{
    DatabaseJsonOption options = serviceProvider.GetRequiredService<IOptions<DatabaseJsonOption>>().Value;

    if (options != null && !string.IsNullOrEmpty(options.FileName))
    {
        string fileName = options.FileName;
        if (File.Exists(fileName))
        {
            string data = File.ReadAllText(fileName);
            List<Persona> personas = JsonConvert.DeserializeObject<List<Persona>>(data);

            return new PersonaRepository(personas, serviceProvider.GetRequiredService<IOptions<DatabaseJsonOption>>());
        }
        else
        {
            throw new FileNotFoundException($"El archivo {fileName} no existe.");
        }
    }
    else
    {
        throw new InvalidOperationException("La configuración de DatabaseJsonOption es inválida.");
    }
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(doc =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    doc.IncludeXmlComments(xmlPath);
});

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