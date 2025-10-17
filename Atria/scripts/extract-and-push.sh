#!/usr/bin/env bash
set -euo pipefail

# Script para extrair a pasta "src/4. Api" com histórico e empurrar para novo repositório GitHub
# Uso: bash scripts/extract-and-push.sh

NEW_REMOTE_URL="https://github.com/ThiagoLuigiM/Atria-Legacy.git"
SUBTREE_PATH="src/4. Api"
BRANCH_NAME="atria-extract"
TARGET_BRANCH="main"
REMOTE_NAME="atria-legacy"

# Verificações iniciais
if ! command -v git >/dev/null 2>&1; then
  echo "git não encontrado. Instale git e execute novamente." >&2
  exit 1
fi

if [ -n "$(git status --porcelain)" ]; then
  echo "Existem alterações não comitadas. Por favor commit ou stash antes de continuar." >&2
  git status --porcelain
  exit 1
fi

# Garantir estamos na branch main atualizada
git fetch origin
git checkout main
git pull origin main

# Criar branch com histórico somente do subtree
echo "Criando branch com histórico da pasta '$SUBTREE_PATH'..."
git subtree split -P "$SUBTREE_PATH" -b "$BRANCH_NAME"

# Adicionar remote se não existir
if ! git remote get-url "$REMOTE_NAME" >/dev/null 2>&1; then
  git remote add "$REMOTE_NAME" "$NEW_REMOTE_URL"
  echo "Remote '$REMOTE_NAME' adicionado -> $NEW_REMOTE_URL"
else
  echo "Remote '$REMOTE_NAME' já existe. Usarei a URL configurada." 
fi

# Push da branch extraída para o novo repositório
echo "Enviando branch '$BRANCH_NAME' para '$REMOTE_NAME' como '$TARGET_BRANCH'..."
git push "$REMOTE_NAME" "$BRANCH_NAME:$TARGET_BRANCH"

cat <<'EOF'
Concluído: o histórico da pasta foi enviado ao novo repositório.
Próximos passos sugeridos (execute localmente):

# 1) Verificar o novo repositório no GitHub
# 2) Remover a pasta do repositório atual (mantendo cópia local):
#    git rm -r --cached "src/4. Api"
#    git commit -m "Remover API (movida para repositório separado: Atria-Legacy)"
#    git push origin main

# Se preferir remover também do disco:
#    git rm -r "src/4. Api"
#    git commit -m "Remover API (movida para repositório separado: Atria-Legacy)"
#    git push origin main

# Se quiser manter o projeto extraído como submódulo:
#    git submodule add $NEW_REMOTE_URL src/legacy/AtriaApi
#    git commit -m "Adicionar Atria-Legacy como submodule"
#    git push origin main
EOF
