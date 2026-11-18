using Microsoft.EntityFrameworkCore;
using ViaCidada.Ouvidoria.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
