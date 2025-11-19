# ============================================================================
# SCRIPT DE ATUALIZAÇÃO PARA OUTROS DESENVOLVEDORES
# Execute após fazer git pull
# ============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  ATUALIZANDO AMBIENTE LOCAL" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "??  AVISO: Este script vai DROPAR e RECRIAR seu banco de dados local!" -ForegroundColor Yellow
Write-Host ""

# Confirmar ação
$confirmacao = Read-Host "Você fez backup dos dados importantes? (S/N)"
if ($confirmacao -ne "S" -and $confirmacao -ne "s") {
    Write-Host "? Operação cancelada. Faça backup primeiro!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Como fazer backup:" -ForegroundColor Yellow
    Write-Host "  1. Abra MySQL Workbench" -ForegroundColor White
    Write-Host "  2. Execute: SELECT * FROM TB_USUARIO;" -ForegroundColor White
    Write-Host "  3. Exporte para CSV" -ForegroundColor White
    Write-Host ""
    exit
}

# Navegar para o diretório do projeto
$projectPath = "C:\Users\HigorZanhe\Source\Repos\pmv-ads-2025-2-e2-proj-int-t2-pmv-ads-eixo-2-turma-2-grupo-4\src\Atria"

Write-Host "?? Navegando para: $projectPath" -ForegroundColor Cyan
Set-Location $projectPath

# ============================================================================
# PASSO 1: LIMPAR BUILD
# ============================================================================
Write-Host ""
Write-Host "?? Limpando build anterior..." -ForegroundColor Cyan
dotnet clean > $null 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "? Build limpo!" -ForegroundColor Green
} else {
    Write-Host "   ??  Aviso ao limpar build (pode ignorar)" -ForegroundColor Yellow
}

# ============================================================================
# PASSO 2: RESTAURAR PACOTES
# ============================================================================
Write-Host ""
Write-Host "?? Restaurando pacotes NuGet..." -ForegroundColor Cyan
dotnet restore

if ($LASTEXITCODE -eq 0) {
    Write-Host "   ? Pacotes restaurados!" -ForegroundColor Green
} else {
    Write-Host "   ? Erro ao restaurar pacotes!" -ForegroundColor Red
    exit
}

# ============================================================================
# PASSO 3: DROPAR BANCO DE DADOS ANTIGO
# ============================================================================
Write-Host ""
Write-Host "???  Dropando banco de dados antigo..." -ForegroundColor Cyan
dotnet ef database drop --force

if ($LASTEXITCODE -eq 0) {
    Write-Host "   ? Banco de dados dropado!" -ForegroundColor Green
} else {
    Write-Host "   ??  Aviso: Banco pode não existir (tudo bem)" -ForegroundColor Yellow
}

# ============================================================================
# PASSO 4: APLICAR NOVA MIGRATION
# ============================================================================
Write-Host ""
Write-Host "?? Aplicando nova migration..." -ForegroundColor Cyan
dotnet ef database update

if ($LASTEXITCODE -eq 0) {
    Write-Host "   ? Migration aplicada com sucesso!" -ForegroundColor Green
} else {
    Write-Host "   ? Erro ao aplicar migration!" -ForegroundColor Red
Write-Host ""
    Write-Host "Tente manualmente:" -ForegroundColor Yellow
  Write-Host "  1. dotnet ef migrations list" -ForegroundColor White
    Write-Host "  2. dotnet ef database update" -ForegroundColor White
  exit
}

# ============================================================================
# PASSO 5: VERIFICAR
# ============================================================================
Write-Host ""
Write-Host "? Verificando build..." -ForegroundColor Cyan
dotnet build

if ($LASTEXITCODE -eq 0) {
Write-Host "   ? Build compilado com sucesso!" -ForegroundColor Green
} else {
    Write-Host "   ? Erro no build!" -ForegroundColor Red
    exit
}

# ============================================================================
# MENSAGEM FINAL
# ============================================================================
Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  ? ATUALIZAÇÃO CONCLUÍDA!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "?? Próximos passos:" -ForegroundColor Cyan
Write-Host "   1. Execute: dotnet run" -ForegroundColor White
Write-Host "   2. Acesse: http://localhost:5000" -ForegroundColor White
Write-Host "   3. Teste as páginas: /Materiais, /Postagens" -ForegroundColor White
Write-Host ""
Write-Host "?? Documentação:" -ForegroundColor Cyan
Write-Host "   - AVISO_MUDANCAS_BANCO_DADOS.md" -ForegroundColor White
Write-Host "   - REVISAO_COMPLETA_FINAL.md" -ForegroundColor White
Write-Host ""

# Perguntar se deseja iniciar
$iniciar = Read-Host "Deseja iniciar a aplicação agora? (S/N)"
if ($iniciar -eq "S" -or $iniciar -eq "s") {
    Write-Host ""
    Write-Host "?? Iniciando aplicação..." -ForegroundColor Cyan
    Write-Host ""
    dotnet run
}
