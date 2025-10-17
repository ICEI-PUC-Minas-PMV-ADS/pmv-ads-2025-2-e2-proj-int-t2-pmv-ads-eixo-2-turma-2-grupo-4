#!/usr/bin/env bash
set -euo pipefail

# Script para extrair a pasta "src/4. Api" com hist�rico e empurrar para novo reposit�rio GitHub
# Uso: bash scripts/extract-and-push.sh

NEW_REMOTE_URL="https://github.com/ThiagoLuigiM/Atria-Legacy.git"
SUBTREE_PATH="src/4. Api"
BRANCH_NAME="atria-extract"
TARGET_BRANCH="main"
REMOTE_NAME="atria-legacy"

# Verifica��es iniciais
if ! command -v git >/dev/null 2>&1; then
  echo "git n�o encontrado. Instale git e execute novamente." >&2
  exit 1
fi

if [ -n "$(git status --porcelain)" ]; then
  echo "Existem altera��es n�o comitadas. Por favor commit ou stash antes de continuar." >&2
  git status --porcelain
  exit 1
fi

# Garantir estamos na branch main atualizada
git fetch origin
git checkout main
git pull origin main

# Criar branch com hist�rico somente do subtree
echo "Criando branch com hist�rico da pasta '$SUBTREE_PATH'..."
git subtree split -P "$SUBTREE_PATH" -b "$BRANCH_NAME"

# Adicionar remote se n�o existir
if ! git remote get-url "$REMOTE_NAME" >/dev/null 2>&1; then
  git remote add "$REMOTE_NAME" "$NEW_REMOTE_URL"
  echo "Remote '$REMOTE_NAME' adicionado -> $NEW_REMOTE_URL"
else
  echo "Remote '$REMOTE_NAME' j� existe. Usarei a URL configurada." 
fi

# Push da branch extra�da para o novo reposit�rio
echo "Enviando branch '$BRANCH_NAME' para '$REMOTE_NAME' como '$TARGET_BRANCH'..."
git push "$REMOTE_NAME" "$BRANCH_NAME:$TARGET_BRANCH"

cat <<'EOF'
Conclu�do: o hist�rico da pasta foi enviado ao novo reposit�rio.
Pr�ximos passos sugeridos (execute localmente):

# 1) Verificar o novo reposit�rio no GitHub
# 2) Remover a pasta do reposit�rio atual (mantendo c�pia local):
#    git rm -r --cached "src/4. Api"
#    git commit -m "Remover API (movida para reposit�rio separado: Atria-Legacy)"
#    git push origin main

# Se preferir remover tamb�m do disco:
#    git rm -r "src/4. Api"
#    git commit -m "Remover API (movida para reposit�rio separado: Atria-Legacy)"
#    git push origin main

# Se quiser manter o projeto extra�do como subm�dulo:
#    git submodule add $NEW_REMOTE_URL src/legacy/AtriaApi
#    git commit -m "Adicionar Atria-Legacy como submodule"
#    git push origin main
EOF
