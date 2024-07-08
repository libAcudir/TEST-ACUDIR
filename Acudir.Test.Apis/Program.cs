
using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using Interfaces;
using Repository.Repository;
using Utils.Exception;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(builder.Configuration);
//builder.Services.AddAutoMapper(typeof(Program).Assembly);


var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Person, PersonDto>()
       .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.NombreCompleto}"));
});

IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

// Add Filters, tu handler Exceeptions with microsoft'libreries
builder.Services.AddMvc(option =>
{
    option.Filters.Add<ExceptionHandlerFilter>();
});
// Add services to the container.
builder.Services.AddTransient<IPersonRepository, PersonRepository>();
builder.Services.AddTransient<IGenericRepository, GenericRepository>();
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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