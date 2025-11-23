using Atria.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Atria.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets das entidades do domínio
        // Em ApplicationDbContext.cs

        public DbSet<Material> Materiais { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Comunidade> Comunidades { get; set; }
        public DbSet<Postagem> Postagens { get; set; }
        public DbSet<GrupoEstudo> GruposEstudo { get; set; }
        public DbSet<ListaLeitura> ListasLeitura { get; set; }
        public DbSet<Comentario> Comentarios { get; set; } // Adicionada DbSet<Comentario>
        public DbSet<ComentarioAvaliacao> ComentariosAvaliacao { get; set; } // ? NOVO
        public DbSet<CurtidaComentario> CurtidasComentario { get; set; } // NOVO
        public DbSet<CurtidaComentarioAvaliacao> CurtidasComentarioAvaliacao { get; set; } // NOVO

        // Tabelas N:M representadas como entidades
        public DbSet<UsuarioComunidade> UsuariosComunidade { get; set; }
        public DbSet<ListaTemMaterial> ListaTemMateriais { get; set; }
        public DbSet<UsuarioGrupo> UsuariosGrupo { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Mapear tabelas do Identity para convenções de banco
            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("TB_USUARIO");
                b.Property(u => u.Id).HasColumnName("ID_USUARIO");
                b.Property(u => u.UserName).HasColumnName("USERNAME").HasMaxLength(150);
                b.Property(u => u.NormalizedUserName).HasColumnName("NORMALIZED_USERNAME").HasMaxLength(150);
                b.Property(u => u.Email).HasColumnName("EMAIL").HasMaxLength(150).IsRequired();
                b.Property(u => u.NormalizedEmail).HasColumnName("NORMALIZED_EMAIL").HasMaxLength(150);
                b.Property(u => u.EmailConfirmed).HasColumnName("EMAIL_CONFIRMED");
                b.Property(u => u.PasswordHash).HasColumnName("SENHA_HASH");
                b.Property(u => u.SecurityStamp).HasColumnName("SECURITY_STAMP");
                b.Property(u => u.ConcurrencyStamp).HasColumnName("CONCURRENCY_STAMP");
                b.Property(u => u.PhoneNumber).HasColumnName("TELEFONE");
                b.Property(u => u.PhoneNumberConfirmed).HasColumnName("TELEFONE_CONFIRMADO");
                b.Property(u => u.TwoFactorEnabled).HasColumnName("DOIS_FATORES_ATIVADO");
                b.Property(u => u.LockoutEnd).HasColumnName("BLOQUEIO_FIM");
                b.Property(u => u.LockoutEnabled).HasColumnName("BLOQUEIO_ATIVADO");
                b.Property(u => u.AccessFailedCount).HasColumnName("FALHAS_ACESSO");
                b.Property(u => u.Nome).HasColumnName("NOME").HasMaxLength(150).IsRequired();
                b.Property(u => u.DataCadastro).HasColumnName("DATA_CADASTRO");
                b.Property(u => u.TipoUsuario).HasColumnName("TIPO_USUARIO").HasMaxLength(50);
                b.Property(u => u.AreaEstudo).HasColumnName("AREA_ESTUDO").HasMaxLength(200);
                b.Property(u => u.TrilhaConhecimento).HasColumnName("TRILHA_CONHECIMENTO").HasMaxLength(500);
                b.Property(u => u.Projetos).HasColumnName("PROJETOS").HasMaxLength(1000);

                // Email unique
                b.HasIndex(u => u.Email).IsUnique().HasDatabaseName("UX_TB_USUARIO_EMAIL");
            });

            // Mapear IdentityRole<int> para TB_ROLE
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole<int>>(b =>
            {
                b.ToTable("TB_ROLE");
                b.Property(r => r.Id).HasColumnName("ID_ROLE");
                b.Property(r => r.Name).HasColumnName("NOME");
                b.Property(r => r.NormalizedName).HasColumnName("NORMALIZED_NOME");
                b.Property(r => r.ConcurrencyStamp).HasColumnName("CONCURRENCY_STAMP");
            });

            // Mapear outras tabelas de identidade
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<int>>(b =>
            {
                b.ToTable("TB_USER_ROLE");
                b.Property(ur => ur.UserId).HasColumnName("FK_USUARIO");
                b.Property(ur => ur.RoleId).HasColumnName("FK_ROLE");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<int>>(b =>
            {
                b.ToTable("TB_USER_CLAIM");
                b.Property(uc => uc.Id).HasColumnName("ID_CLAIM");
                b.Property(uc => uc.UserId).HasColumnName("FK_USUARIO");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<int>>(b =>
            {
                b.ToTable("TB_USER_LOGIN");
                b.Property(ul => ul.UserId).HasColumnName("FK_USUARIO");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<int>>(b =>
            {
                b.ToTable("TB_USER_TOKEN");
                b.Property(ut => ut.UserId).HasColumnName("FK_USUARIO");
            });

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>>(b =>
            {
                b.ToTable("TB_ROLE_CLAIM");
                b.Property(rc => rc.Id).HasColumnName("ID_ROLE_CLAIM");
                b.Property(rc => rc.RoleId).HasColumnName("FK_ROLE");
            });

            // Mapear entidades de domínio
            builder.Entity<Material>(b =>
            {
                b.ToTable("TB_MATERIAL");
                b.HasKey(m => m.Id).HasName("PK_TB_MATERIAL");
                b.Property(m => m.Id).HasColumnName("ID_MATERIAL");
                b.Property(m => m.Titulo).HasColumnName("TITULO").HasMaxLength(250).IsRequired();
                b.Property(m => m.Descricao).HasColumnName("DESCRICAO");
                b.Property(m => m.Tipo).HasColumnName("TIPO").HasMaxLength(50);
                b.Property(m => m.CAMINHOIMG).HasColumnName("CAMINHOIMG").HasMaxLength(250);
                b.Property(m => m.FKUsuarioCriador).HasColumnName("FK_USUARIO_CRIADOR");
                b.Property(m => m.DataCriacao).HasColumnName("DATA_CRIACAO");
                b.Property(m => m.Status).HasColumnName("STATUS");

                b.HasOne(m => m.Criador)
                    .WithMany()
                    .HasForeignKey(m => m.FKUsuarioCriador)
                    .HasConstraintName("FK_MATERIAL_USUARIO")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Avaliacao>(b =>
            {
                b.ToTable("TB_AVALIACAO");
                b.HasKey(a => a.Id).HasName("PK_TB_AVALIACAO");
                b.Property(a => a.Id).HasColumnName("ID_AVALIACAO");
                b.Property(a => a.Nota).HasColumnName("NOTA");
                b.Property(a => a.TipoAvaliacao).HasColumnName("TIPO_AVALIACAO").HasMaxLength(50);
                b.Property(a => a.FKUsuario).HasColumnName("FK_USUARIO");
                b.Property(a => a.FKMaterial).HasColumnName("FK_MATERIAL");
                b.Property(a => a.Resenha).HasColumnName("RESENHA");

                b.HasOne(a => a.Usuario)
                    .WithMany()
                    .HasForeignKey(a => a.FKUsuario)
                    .HasConstraintName("FK_AVALIACAO_USUARIO")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(a => a.Material)
                    .WithMany(m => m.Avaliacoes)
                    .HasForeignKey(a => a.FKMaterial)
                    .HasConstraintName("FK_AVALIACAO_MATERIAL")
                    .OnDelete(DeleteBehavior.Cascade);

                // Regra: Avaliação única por usuário por material
                b.HasIndex(a => new { a.FKUsuario, a.FKMaterial }).IsUnique().HasDatabaseName("UX_AVALIACAO_USUARIO_MATERIAL");
            });

            builder.Entity<Comunidade>(b =>
            {
                b.ToTable("TB_COMUNIDADE");
                b.HasKey(c => c.Id).HasName("PK_TB_COMUNIDADE");
                b.Property(c => c.Id).HasColumnName("ID_COMUNIDADE");
                b.Property(c => c.Nome).HasColumnName("NOME").HasMaxLength(150).IsRequired();
                b.Property(c => c.Descricao).HasColumnName("DESCRICAO");
                b.Property(c => c.DataCriacao).HasColumnName("DATA_CRIACAO");
            });

            builder.Entity<Postagem>(b =>
            {
                b.ToTable("TB_POSTAGEM");
                b.HasKey(p => p.Id).HasName("PK_TB_POSTAGEM");
                b.Property(p => p.Id).HasColumnName("ID_POSTAGEM");
                b.Property(p => p.Titulo).HasColumnName("TITULO").HasMaxLength(200).IsRequired();
                b.Property(p => p.Conteudo).HasColumnName("CONTEUDO");
                b.Property(p => p.DataPostagem).HasColumnName("DATA_POSTAGEM");
                b.Property(p => p.NoForumGeral).HasColumnName("NOFORUMGERAL");
                b.Property(p => p.FKUsuario).HasColumnName("FK_USUARIO");
                b.Property(p => p.FKComunidade).HasColumnName("FK_COMUNIDADE");
                b.Property(p => p.FKGrupo).HasColumnName("FK_GRUPO");

                b.HasOne(p => p.Usuario)
                    .WithMany()
                    .HasForeignKey(p => p.FKUsuario)
                    .HasConstraintName("FK_POSTAGEM_USUARIO")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(p => p.Comunidade)
                    .WithMany(c => c.Postagens)
                    .HasForeignKey(p => p.FKComunidade)
                    .HasConstraintName("FK_POSTAGEM_COMUNIDADE")
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(p => p.GrupoEstudo)
                    .WithMany(g => g.Postagens)
                    .HasForeignKey(p => p.FKGrupo)
                    .HasConstraintName("FK_POSTAGEM_GRUPO")
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Comentario mapping
            builder.Entity<Comentario>(b =>
            {
                b.ToTable("TB_COMENTARIO");
                b.HasKey(c => c.Id).HasName("PK_TB_COMENTARIO");
                b.Property(c => c.Id).HasColumnName("ID_COMENTARIO");
                b.Property(c => c.Conteudo).HasColumnName("CONTEUDO");
                b.Property(c => c.DataComentario).HasColumnName("DATA_COMENTARIO");
                b.Property(c => c.FKPostagem).HasColumnName("FK_POSTAGEM");
                b.Property(c => c.FKUsuario).HasColumnName("FK_USUARIO");
                b.Property(c => c.FKComentarioPai).HasColumnName("FK_COMENTARIO_PAI");

                b.HasOne(c => c.Postagem)
                        .WithMany(p => p.Comentarios)
                .HasForeignKey(c => c.FKPostagem)
                       .HasConstraintName("FK_COMENTARIO_POSTAGEM")
      .OnDelete(DeleteBehavior.Cascade);

                        b.HasOne(c => c.Usuario)
       .WithMany()
                .HasForeignKey(c => c.FKUsuario)
           .HasConstraintName("FK_COMENTARIO_USUARIO")
            .OnDelete(DeleteBehavior.Cascade);

                // Relacionamento auto-referenciado para comentários aninhados
        b.HasOne(c => c.ComentarioPai)
           .WithMany(c => c.Respostas)
            .HasForeignKey(c => c.FKComentarioPai)
        .HasConstraintName("FK_COMENTARIO_COMENTARIO_PAI")
          .OnDelete(DeleteBehavior.Restrict);
            });
            
   // ? NOVO: ComentarioAvaliacao mapping
   builder.Entity<ComentarioAvaliacao>(b =>
 {
   b.ToTable("TB_COMENTARIO_AVALIACAO");
   b.HasKey(c => c.Id).HasName("PK_TB_COMENTARIO_AVALIACAO");
    b.Property(c => c.Id).HasColumnName("ID_COMENTARIO_AVALIACAO");
    b.Property(c => c.Conteudo).HasColumnName("CONTEUDO").HasMaxLength(500).IsRequired();
   b.Property(c => c.DataComentario).HasColumnName("DATA_COMENTARIO");
          b.Property(c => c.FKAvaliacao).HasColumnName("FK_AVALIACAO");
  b.Property(c => c.FKUsuario).HasColumnName("FK_USUARIO");
        b.Property(c => c.FKComentarioPai).HasColumnName("FK_COMENTARIO_PAI");

      b.HasOne(c => c.Avaliacao)
     .WithMany(a => a.Comentarios)
        .HasForeignKey(c => c.FKAvaliacao)
.HasConstraintName("FK_COMENTARIO_AVALIACAO_AVALIACAO")
.OnDelete(DeleteBehavior.Cascade);

  b.HasOne(c => c.Usuario)
.WithMany()
       .HasForeignKey(c => c.FKUsuario)
 .HasConstraintName("FK_COMENTARIO_AVALIACAO_USUARIO")
           .OnDelete(DeleteBehavior.Cascade);

  // Relacionamento auto-referenciado para comentários de avaliação aninhados
  b.HasOne(c => c.ComentarioPai)
       .WithMany(c => c.Respostas)
  .HasForeignKey(c => c.FKComentarioPai)
  .HasConstraintName("FK_COMENTARIO_AVALIACAO_COMENTARIO_PAI")
.OnDelete(DeleteBehavior.Restrict);
 });
            builder.Entity<GrupoEstudo>(b =>
            {
                b.ToTable("TB_GRUPO_ESTUDO");
                b.HasKey(g => g.Id).HasName("PK_TB_GRUPO_ESTUDO");
                b.Property(g => g.Id).HasColumnName("ID_GRUPO");
                b.Property(g => g.Nome).HasColumnName("NOME").HasMaxLength(150).IsRequired();
                b.Property(g => g.Descricao).HasColumnName("DESCRICAO");
                b.Property(g => g.FKComunidade).HasColumnName("FK_COMUNIDADE");
                b.Property(g => g.DataCriacao).HasColumnName("DATA_CRIACAO");

                b.HasOne(g => g.Comunidade)
                    .WithMany(c => c.GruposEstudo)
                    .HasForeignKey(g => g.FKComunidade)
                    .HasConstraintName("FK_GRUPO_COMUNIDADE")
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<ListaLeitura>(b =>
            {
                b.ToTable("TB_LISTA_LEITURA");
                b.HasKey(l => l.Id).HasName("PK_TB_LISTA_LEITURA");
                b.Property(l => l.Id).HasColumnName("ID_LISTA");
                b.Property(l => l.Nome).HasColumnName("NOME").HasMaxLength(150).IsRequired();
                b.Property(l => l.Descricao).HasColumnName("DESCRICAO");
                b.Property(l => l.FKUsuario).HasColumnName("FK_USUARIO");

                b.HasOne(l => l.Usuario)
                    .WithMany()
                    .HasForeignKey(l => l.FKUsuario)
                    .HasConstraintName("FK_LISTA_USUARIO")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // N:M UsuarioComunidade
            builder.Entity<UsuarioComunidade>(b =>
            {
                b.ToTable("TB_USUARIO_COMUNIDADE");
                b.HasKey("FKUsuario", "FKComunidade").HasName("PK_TB_USUARIO_COMUNIDADE");
                b.Property(u => u.FKUsuario).HasColumnName("FK_USUARIO");
                b.Property(u => u.FKComunidade).HasColumnName("FK_COMUNIDADE");
                b.Property(u => u.DataEntrada).HasColumnName("DATA_ENTRADA");

                b.HasOne(u => u.Usuario)
                    .WithMany()
                    .HasForeignKey(u => u.FKUsuario)
                    .HasConstraintName("FK_USUARIOCOMUNIDADE_USUARIO")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(u => u.Comunidade)
                    .WithMany(c => c.Usuarios)
                    .HasForeignKey(u => u.FKComunidade)
                    .HasConstraintName("FK_USUARIOCOMUNIDADE_COMUNIDADE")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // N:M ListaTemMaterial
            builder.Entity<ListaTemMaterial>(b =>
            {
                b.ToTable("TB_LISTA_TEM_MATERIAL");
                b.HasKey("FKLista", "FKMaterial").HasName("PK_TB_LISTA_TEM_MATERIAL");
                b.Property(p => p.FKLista).HasColumnName("FK_LISTA");
                b.Property(p => p.FKMaterial).HasColumnName("FK_MATERIAL");
                b.Property(p => p.Ordem).HasColumnName("ORDEM");

                b.HasOne(p => p.ListaLeitura)
                    .WithMany(l => l.ListaTemMateriais)
                    .HasForeignKey(p => p.FKLista)
                    .HasConstraintName("FK_LISTATEMMATERIAL_LISTA")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(p => p.Material)
                    .WithMany(m => m.ListaTemMateriais)
                    .HasForeignKey(p => p.FKMaterial)
                    .HasConstraintName("FK_LISTATEMMATERIAL_MATERIAL")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // N:M UsuarioGrupo
            builder.Entity<UsuarioGrupo>(b =>
            {
                b.ToTable("TB_USUARIO_GRUPO");
                b.HasKey("FKUsuario", "FKGrupo").HasName("PK_TB_USUARIO_GRUPO");
                b.Property(p => p.FKUsuario).HasColumnName("FK_USUARIO");
                b.Property(p => p.FKGrupo).HasColumnName("FK_GRUPO");
                b.Property(p => p.DataEntrada).HasColumnName("DATA_ENTRADA");

                b.HasOne(p => p.Usuario)
                    .WithMany()
                    .HasForeignKey(p => p.FKUsuario)
                    .HasConstraintName("FK_USUARIOGRUPO_USUARIO")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(p => p.GrupoEstudo)
                    .WithMany(g => g.Usuarios)
                    .HasForeignKey(p => p.FKGrupo)
                    .HasConstraintName("FK_USUARIOGRUPO_GRUPO")
                    .OnDelete(DeleteBehavior.Cascade);
            });

             // NOVO: CurtidaComentario mapping
  builder.Entity<CurtidaComentario>(b =>
            {
       b.ToTable("TB_CURTIDA_COMENTARIO");
  b.HasKey(c => c.Id).HasName("PK_TB_CURTIDA_COMENTARIO");
    b.Property(c => c.Id).HasColumnName("ID_CURTIDA");
        b.Property(c => c.FKComentario).HasColumnName("FK_COMENTARIO");
           b.Property(c => c.FKUsuario).HasColumnName("FK_USUARIO");
      b.Property(c => c.DataCurtida).HasColumnName("DATA_CURTIDA");
       b.Property(c => c.Tipo).HasColumnName("TIPO").HasMaxLength(10);

              b.HasOne(c => c.Comentario)
     .WithMany(com => com.Curtidas)
   .HasForeignKey(c => c.FKComentario)
      .HasConstraintName("FK_CURTIDA_COMENTARIO")
      .OnDelete(DeleteBehavior.Cascade);

       b.HasOne(c => c.Usuario)
         .WithMany()
  .HasForeignKey(c => c.FKUsuario)
.HasConstraintName("FK_CURTIDA_USUARIO")
.OnDelete(DeleteBehavior.Cascade);

     // Regra: Um usuário só pode curtir um comentário uma vez
   b.HasIndex(c => new { c.FKUsuario, c.FKComentario }).IsUnique().HasDatabaseName("UX_CURTIDA_USUARIO_COMENTARIO");
            });

            // NOVO: CurtidaComentarioAvaliacao mapping
     builder.Entity<CurtidaComentarioAvaliacao>(b =>
            {
        b.ToTable("TB_CURTIDA_COMENTARIO_AVALIACAO");
             b.HasKey(c => c.Id).HasName("PK_TB_CURTIDA_COMENTARIO_AVALIACAO");
         b.Property(c => c.Id).HasColumnName("ID_CURTIDA");
          b.Property(c => c.FKComentarioAvaliacao).HasColumnName("FK_COMENTARIO_AVALIACAO");
  b.Property(c => c.FKUsuario).HasColumnName("FK_USUARIO");
            b.Property(c => c.DataCurtida).HasColumnName("DATA_CURTIDA");
        b.Property(c => c.Tipo).HasColumnName("TIPO").HasMaxLength(10);

          b.HasOne(c => c.ComentarioAvaliacao)
       .WithMany(com => com.Curtidas)
              .HasForeignKey(c => c.FKComentarioAvaliacao)
      .HasConstraintName("FK_CURTIDA_COMENTARIO_AVALIACAO")
       .OnDelete(DeleteBehavior.Cascade);

     b.HasOne(c => c.Usuario)
     .WithMany()
              .HasForeignKey(c => c.FKUsuario)
  .HasConstraintName("FK_CURTIDA_AVALIACAO_USUARIO")
       .OnDelete(DeleteBehavior.Cascade);

       // Regra: Um usuário só pode curtir um comentário de avaliação uma vez
                b.HasIndex(c => new { c.FKUsuario, c.FKComentarioAvaliacao }).IsUnique().HasDatabaseName("UX_CURTIDA_USUARIO_COMENTARIO_AVALIACAO");
    });

            // N:M Mensagem
   builder.Entity<Mensagem>(b =>
   {
       b.ToTable("TB_MENSAGEM");
       b.HasKey(m => m.Id).HasName("PK_TB_MENSAGEM");

       // Configuração: Quem enviou
       b.HasOne(m => m.Remetente)
           .WithMany() // Usuário pode enviar muitas mensagens
           .HasForeignKey(m => m.FKRemetente)
           .HasConstraintName("FK_MENSAGEM_REMETENTE")
           .OnDelete(DeleteBehavior.Restrict); // IMPORTANTE: Restrict para não quebrar o banco

       // Configuração: Chat de Grupo
       b.HasOne(m => m.GrupoEstudo)
           .WithMany(g => g.Mensagens) // Já vamos adicionar essa lista no GrupoEstudo
           .HasForeignKey(m => m.FKGrupo)
           .HasConstraintName("FK_MENSAGEM_GRUPO")
           .OnDelete(DeleteBehavior.Cascade); // Se apagar o grupo, apaga as mensagens dele

       // Configuração: Direct Message (DM)
       b.HasOne(m => m.Destinatario)
           .WithMany()
           .HasForeignKey(m => m.FKDestinatario)
           .HasConstraintName("FK_MENSAGEM_DESTINATARIO")
           .OnDelete(DeleteBehavior.Restrict); // IMPORTANTE: Restrict também
   });
        }
    }
}