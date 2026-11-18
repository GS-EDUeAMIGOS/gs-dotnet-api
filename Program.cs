using Microsoft.EntityFrameworkCore;
using ViaCidada.Ouvidoria.Api.Data;

// Carrega variáveis do arquivo .env local (se existir) para o processo atual
var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envFilePath))
{
    foreach (var rawLine in File.ReadAllLines(envFilePath))
    {
        var line = rawLine.Trim();
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
            continue;

        var parts = line.Split('=', 2);
        if (parts.Length == 2 && string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(parts[0].Trim())))
        {
            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
        }
    }
}

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OracleConnection") ?? string.Empty;
var oracleUser = Environment.GetEnvironmentVariable("ORACLE_DB_USERNAME");
var oraclePass = Environment.GetEnvironmentVariable("ORACLE_DB_PASSWORD");
var oracleDataSource = Environment.GetEnvironmentVariable("ORACLE_DATA_SOURCE")
    ?? "(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))(CONNECT_DATA=(SID=ORCL)))";

if ((string.IsNullOrWhiteSpace(connectionString) || connectionString.Contains("User Id=;"))
    && !string.IsNullOrWhiteSpace(oracleUser)
    && !string.IsNullOrWhiteSpace(oraclePass))
{
    connectionString = $"Data Source={oracleDataSource};User Id={oracleUser};Password={oraclePass};";
}

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connectionString));

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

