# ============================================================================
# SCRIPT AUTOMÁTICO - RECRIAR BANCO DE DADOS DO ZERO
# Execute este script no PowerShell
# ============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  RECRIAR BANCO DE DADOS DO ZERO" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Verificar se está no diretório correto
$expectedPath = "C:\Users\HigorZanhe\Source\Repos\pmv-ads-2025-2-e2-proj-int-t2-pmv-ads-eixo-2-turma-2-grupo-4\src\Atria"
$currentPath = Get-Location

if ($currentPath.Path -ne $expectedPath) {
    Write-Host "Mudando para o diretório correto..." -ForegroundColor Yellow
    Set-Location $expectedPath
}

Write-Host "Diretório atual: $((Get-Location).Path)" -ForegroundColor Green
Write-Host ""

# ============================================================================
# PASSO 1: CONFIRMAR AÇÃO
# ============================================================================
Write-Host "??  ATENÇÃO! Este script vai:" -ForegroundColor Red
Write-Host "   1. Deletar todas as migrations" -ForegroundColor Red
Write-Host "   2. Dropar o banco de dados 'atria'" -ForegroundColor Red
Write-Host "   3. Recriar tudo do zero" -ForegroundColor Red
Write-Host ""
$confirmacao = Read-Host "Deseja continuar? (S/N)"

if ($confirmacao -ne "S" -and $confirmacao -ne "s") {
    Write-Host "Operação cancelada pelo usuário." -ForegroundColor Yellow
    exit
}

# ============================================================================
# PASSO 2: DELETAR MIGRATIONS ANTIGAS
# ============================================================================
Write-Host ""
Write-Host "?? PASSO 1: Deletando migrations antigas..." -ForegroundColor Cyan

if (Test-Path "Migrations") {
    Remove-Item -Path "Migrations\*" -Recurse -Force
    Write-Host "   ? Migrations deletadas com sucesso!" -ForegroundColor Green
} else {
    Write-Host "   ??  Pasta Migrations não encontrada (provavelmente já foi deletada)" -ForegroundColor Yellow
}

# ============================================================================
# PASSO 3: CRIAR NOVA MIGRATION
# ============================================================================
Write-Host ""
Write-Host "?? PASSO 2: Criando nova migration..." -ForegroundColor Cyan

try {
    dotnet ef migrations add InitialCreate
Write-Host "   ? Migration criada com sucesso!" -ForegroundColor Green
} catch {
    Write-Host "   ? Erro ao criar migration!" -ForegroundColor Red
    Write-Host "   Erro: $_" -ForegroundColor Red
    exit
}

# ============================================================================
# PASSO 4: APLICAR MIGRATION
# ============================================================================
Write-Host ""
Write-Host "?? PASSO 3: Aplicando migration no banco de dados..." -ForegroundColor Cyan
Write-Host "   (Isso vai dropar e recriar o banco automaticamente)" -ForegroundColor Yellow

try {
    dotnet ef database drop --force
    Write-Host "   ? Banco de dados dropado!" -ForegroundColor Green
} catch {
    Write-Host "   ??  Aviso ao dropar banco (provavelmente não existia): $_" -ForegroundColor Yellow
}

try {
    dotnet ef database update
    Write-Host "   ? Banco de dados recriado com sucesso!" -ForegroundColor Green
} catch {
    Write-Host "   ? Erro ao aplicar migration!" -ForegroundColor Red
    Write-Host "   Erro: $_" -ForegroundColor Red
    exit
}

# ============================================================================
# PASSO 5: MENSAGEM FINAL
# ============================================================================
Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  ? PROCESSO CONCLUÍDO COM SUCESSO!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "?? Próximos passos:" -ForegroundColor Cyan
Write-Host "   1. Execute: dotnet run" -ForegroundColor White
Write-Host "   2. Acesse: http://localhost:5000" -ForegroundColor White
Write-Host "   3. Teste as páginas de Materiais e Postagens" -ForegroundColor White
Write-Host ""
Write-Host "?? Para verificar o banco no MySQL Workbench:" -ForegroundColor Cyan
Write-Host "   USE atria;" -ForegroundColor White
Write-Host "   SHOW TABLES;" -ForegroundColor White
Write-Host "   DESCRIBE TB_POSTAGEM;" -ForegroundColor White
Write-Host "   DESCRIBE TB_MATERIAL;" -ForegroundColor White
Write-Host ""

# Perguntar se deseja iniciar a aplicação
$iniciar = Read-Host "Deseja iniciar a aplicação agora? (S/N)"
if ($iniciar -eq "S" -or $iniciar -eq "s") {
    Write-Host ""
 Write-Host "?? Iniciando aplicação..." -ForegroundColor Cyan
    dotnet run
}
