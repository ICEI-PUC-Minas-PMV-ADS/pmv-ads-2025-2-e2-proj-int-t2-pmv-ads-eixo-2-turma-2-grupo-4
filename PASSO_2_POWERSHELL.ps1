# ============================================================================
# PASSO 2: RECRIAR MIGRATIONS - Execute no PowerShell
# ============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  RECRIANDO BANCO DE DADOS" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Navegar para o diretório correto
$projectPath = "C:\Users\HigorZanhe\Source\Repos\pmv-ads-2025-2-e2-proj-int-t2-pmv-ads-eixo-2-turma-2-grupo-4\src\Atria"
Set-Location $projectPath

Write-Host "?? Diretório: $projectPath" -ForegroundColor Green
Write-Host ""

# ============================================================================
# CRIAR NOVA MIGRATION
# ============================================================================
Write-Host "?? Criando nova migration..." -ForegroundColor Cyan
dotnet ef migrations add InitialCreate

if ($LASTEXITCODE -eq 0) {
    Write-Host "   ? Migration criada com sucesso!" -ForegroundColor Green
} else {
    Write-Host "   ? Erro ao criar migration!" -ForegroundColor Red
    exit
}

Write-Host ""

# ============================================================================
# APLICAR MIGRATION NO BANCO
# ============================================================================
Write-Host "?? Aplicando migration no banco de dados..." -ForegroundColor Cyan
dotnet ef database update

if ($LASTEXITCODE -eq 0) {
    Write-Host "   ? Banco de dados criado com sucesso!" -ForegroundColor Green
} else {
    Write-Host "   ? Erro ao aplicar migration!" -ForegroundColor Red
    exit
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  ? SUCESSO!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Agora execute: dotnet run" -ForegroundColor Yellow
Write-Host ""
