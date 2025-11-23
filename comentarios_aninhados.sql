-- Script para adicionar suporte a comentários aninhados (estilo Reddit)
-- Execute este script no seu banco de dados MySQL

-- IMPORTANTE: Ajuste o nome do banco de dados abaixo conforme seu ambiente
-- Verifique no appsettings.json qual é o nome do banco (geralmente 'atria_db')
USE `atria_db`;

-- ===================================================================
-- TB_COMENTARIO: Adicionar coluna FK_COMENTARIO_PAI (se não existir)
-- ===================================================================

-- Verificar e adicionar coluna FK_COMENTARIO_PAI
SET @exists_comentario = (SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = DATABASE() 
    AND TABLE_NAME = 'TB_COMENTARIO' 
    AND COLUMN_NAME = 'FK_COMENTARIO_PAI');

SET @sql_comentario_col = IF(@exists_comentario = 0,
    'ALTER TABLE `TB_COMENTARIO` ADD COLUMN `FK_COMENTARIO_PAI` int NULL',
    'SELECT ''Coluna FK_COMENTARIO_PAI já existe em TB_COMENTARIO'' AS Info');

PREPARE stmt FROM @sql_comentario_col;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Verificar e adicionar foreign key para TB_COMENTARIO
SET @exists_fk_comentario = (SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
    WHERE TABLE_SCHEMA = DATABASE() 
    AND TABLE_NAME = 'TB_COMENTARIO' 
    AND CONSTRAINT_NAME = 'FK_COMENTARIO_COMENTARIO_PAI');

SET @sql_comentario_fk = IF(@exists_fk_comentario = 0,
    'ALTER TABLE `TB_COMENTARIO` 
   ADD CONSTRAINT `FK_COMENTARIO_COMENTARIO_PAI` 
   FOREIGN KEY (`FK_COMENTARIO_PAI`) 
     REFERENCES `TB_COMENTARIO` (`ID_COMENTARIO`) 
     ON DELETE RESTRICT',
    'SELECT ''FK_COMENTARIO_COMENTARIO_PAI já existe'' AS Info');

PREPARE stmt FROM @sql_comentario_fk;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- ===================================================================
-- TB_COMENTARIO_AVALIACAO: Adicionar coluna FK_COMENTARIO_PAI (se não existir)
-- ===================================================================

-- Verificar e adicionar coluna FK_COMENTARIO_PAI
SET @exists_avaliacao = (SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = DATABASE() 
    AND TABLE_NAME = 'TB_COMENTARIO_AVALIACAO' 
    AND COLUMN_NAME = 'FK_COMENTARIO_PAI');

SET @sql_avaliacao_col = IF(@exists_avaliacao = 0,
'ALTER TABLE `TB_COMENTARIO_AVALIACAO` ADD COLUMN `FK_COMENTARIO_PAI` int NULL',
    'SELECT ''Coluna FK_COMENTARIO_PAI já existe em TB_COMENTARIO_AVALIACAO'' AS Info');

PREPARE stmt FROM @sql_avaliacao_col;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Verificar e adicionar foreign key para TB_COMENTARIO_AVALIACAO
SET @exists_fk_avaliacao = (SELECT COUNT(*) 
  FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
    WHERE TABLE_SCHEMA = DATABASE() 
    AND TABLE_NAME = 'TB_COMENTARIO_AVALIACAO' 
    AND CONSTRAINT_NAME = 'FK_COMENTARIO_AVALIACAO_COMENTARIO_PAI');

SET @sql_avaliacao_fk = IF(@exists_fk_avaliacao = 0,
    'ALTER TABLE `TB_COMENTARIO_AVALIACAO` 
     ADD CONSTRAINT `FK_COMENTARIO_AVALIACAO_COMENTARIO_PAI` 
     FOREIGN KEY (`FK_COMENTARIO_PAI`) 
     REFERENCES `TB_COMENTARIO_AVALIACAO` (`ID_COMENTARIO_AVALIACAO`) 
     ON DELETE RESTRICT',
    'SELECT ''FK_COMENTARIO_AVALIACAO_COMENTARIO_PAI já existe'' AS Info');

PREPARE stmt FROM @sql_avaliacao_fk;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- ===================================================================
-- Verificação Final
-- ===================================================================

SELECT 
 'TB_COMENTARIO' AS Tabela,
    COLUMN_NAME AS Coluna,
    COLUMN_TYPE AS Tipo,
  IS_NULLABLE AS Permite_Nulo
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() 
AND TABLE_NAME = 'TB_COMENTARIO' 
AND COLUMN_NAME = 'FK_COMENTARIO_PAI'

UNION ALL

SELECT 
    'TB_COMENTARIO_AVALIACAO' AS Tabela,
    COLUMN_NAME AS Coluna,
    COLUMN_TYPE AS Tipo,
    IS_NULLABLE AS Permite_Nulo
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() 
AND TABLE_NAME = 'TB_COMENTARIO_AVALIACAO' 
AND COLUMN_NAME = 'FK_COMENTARIO_PAI';

SELECT '? Script executado com sucesso!' AS Status;
