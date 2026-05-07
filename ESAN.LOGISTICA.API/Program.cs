using ESAN.LOGISTICA.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Connection String
var _config = builder.Configuration;
var cnx = _config.GetConnectionString("DevConnection");

// DbContext
builder.Services.AddDbContext<LogisticaDbContext>(
    options => options.UseSqlServer(cnx)
);

// Controllers + evitar ciclos infinitos JSON
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles
    );

// OpenAPI / Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();