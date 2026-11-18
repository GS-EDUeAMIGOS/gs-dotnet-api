# Carrega as variaveis do arquivo .env para a sessao atual do PowerShell
# e monta automaticamente ConnectionStrings__OracleConnection para o ASP.NET Core / EF Core.
# Uso (com ponto e espaco na frente para aplicar na sessao atual):
#   . .\scripts\load-env.ps1

param(
    [string]$EnvFile = (Join-Path (Split-Path -Parent $PSScriptRoot) ".env")
)

if (-not (Test-Path $EnvFile)) {
    Write-Error "Arquivo .env nao encontrado em: $EnvFile"
    return
}

$loadedCount = 0
Get-Content -Path $EnvFile -Encoding UTF8 | ForEach-Object {
    $line = $_.Trim()
    if ([string]::IsNullOrWhiteSpace($line) -or $line.StartsWith("#")) {
        return
    }

    $parts = $line.Split("=", 2)
    if ($parts.Length -eq 2) {
        $key = $parts[0].Trim()
        $value = $parts[1].Trim()
        [System.Environment]::SetEnvironmentVariable($key, $value, "Process")
        $loadedCount++
    }
}

$oracleUser = [System.Environment]::GetEnvironmentVariable("ORACLE_DB_USERNAME", "Process")
$oraclePass = [System.Environment]::GetEnvironmentVariable("ORACLE_DB_PASSWORD", "Process")
$oracleDataSource = [System.Environment]::GetEnvironmentVariable("ORACLE_DATA_SOURCE", "Process")
if ([string]::IsNullOrWhiteSpace($oracleDataSource)) {
    $oracleDataSource = "(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))(CONNECT_DATA=(SID=ORCL)))"
}

if (-not [string]::IsNullOrWhiteSpace($oracleUser) -and -not [string]::IsNullOrWhiteSpace($oraclePass)) {
    $connStr = "Data Source=$oracleDataSource;User Id=$oracleUser;Password=$oraclePass;"
    [System.Environment]::SetEnvironmentVariable("ConnectionStrings__OracleConnection", $connStr, "Process")
}

[System.Environment]::SetEnvironmentVariable("DOTNET_ROLL_FORWARD", "Major", "Process")

Write-Host "[OK] $loadedCount variaveis carregadas de $EnvFile para esta sessao (.NET / Oracle)." -ForegroundColor Green
