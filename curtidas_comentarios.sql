-- Script para adicionar sistema de curtidas em comentários
-- Execute este script no seu banco de dados MySQL

USE `atria_db`;

-- ===================================================================
-- TB_CURTIDA_COMENTARIO: Tabela de curtidas em comentários de postagens
-- ===================================================================

CREATE TABLE IF NOT EXISTS `TB_CURTIDA_COMENTARIO` (
    `ID_CURTIDA` int NOT NULL AUTO_INCREMENT,
    `FK_COMENTARIO` int NOT NULL,
    `FK_USUARIO` int NOT NULL,
    `DATA_CURTIDA` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `TIPO` varchar(10) NOT NULL DEFAULT 'LIKE',
    PRIMARY KEY (`ID_CURTIDA`),
    UNIQUE KEY `UX_CURTIDA_USUARIO_COMENTARIO` (`FK_USUARIO`, `FK_COMENTARIO`),
    KEY `FK_CURTIDA_COMENTARIO_idx` (`FK_COMENTARIO`),
    KEY `FK_CURTIDA_USUARIO_idx` (`FK_USUARIO`),
    CONSTRAINT `FK_CURTIDA_COMENTARIO` 
        FOREIGN KEY (`FK_COMENTARIO`) 
        REFERENCES `TB_COMENTARIO` (`ID_COMENTARIO`) 
        ON DELETE CASCADE,
    CONSTRAINT `FK_CURTIDA_USUARIO` 
        FOREIGN KEY (`FK_USUARIO`) 
        REFERENCES `TB_USUARIO` (`ID_USUARIO`) 
        ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ===================================================================
-- TB_CURTIDA_COMENTARIO_AVALIACAO: Tabela de curtidas em comentários de avaliações
-- ===================================================================

CREATE TABLE IF NOT EXISTS `TB_CURTIDA_COMENTARIO_AVALIACAO` (
    `ID_CURTIDA` int NOT NULL AUTO_INCREMENT,
    `FK_COMENTARIO_AVALIACAO` int NOT NULL,
    `FK_USUARIO` int NOT NULL,
    `DATA_CURTIDA` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `TIPO` varchar(10) NOT NULL DEFAULT 'LIKE',
 PRIMARY KEY (`ID_CURTIDA`),
    UNIQUE KEY `UX_CURTIDA_USUARIO_COMENTARIO_AVALIACAO` (`FK_USUARIO`, `FK_COMENTARIO_AVALIACAO`),
    KEY `FK_CURTIDA_COMENTARIO_AVALIACAO_idx` (`FK_COMENTARIO_AVALIACAO`),
    KEY `FK_CURTIDA_AVALIACAO_USUARIO_idx` (`FK_USUARIO`),
    CONSTRAINT `FK_CURTIDA_COMENTARIO_AVALIACAO` 
        FOREIGN KEY (`FK_COMENTARIO_AVALIACAO`) 
        REFERENCES `TB_COMENTARIO_AVALIACAO` (`ID_COMENTARIO_AVALIACAO`) 
        ON DELETE CASCADE,
    CONSTRAINT `FK_CURTIDA_AVALIACAO_USUARIO` 
        FOREIGN KEY (`FK_USUARIO`) 
        REFERENCES `TB_USUARIO` (`ID_USUARIO`) 
        ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Verificação final
SELECT 'Tabelas de curtidas criadas com sucesso!' AS Status;
