using Ekkodale.Services;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Models.Ekkodale;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPersonService, PersonService>();
builder.Services.AddSingleton<IMovieService, MovieService>();

builder.Services.Configure<Neo4jDatabaseSettings>(
                           builder.Configuration.GetSection(nameof(Neo4jDatabaseSettings)));

builder.Services.AddSingleton<INeo4jDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<Neo4jDatabaseSettings>>().Value);

builder.Services.AddSingleton<Neo4jService>();
                           
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Ekkodale Test API By Mahdi",
        Description = "An ASP.NET Core Web API for managing Neo4j DB",
        TermsOfService = new Uri("https://www.ekkodale.com/ekkonews/"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://www.ekkodale.com/leistungen/leistungen-fuer-baubeteiligte/")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://www.ekkodale.com/")
        }
    });
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
