# ?? GUIA DE MUDANÇAS NO BACKEND - SISTEMA ATRIA

**Data:** 19/01/2025  
**Versão:** 1.0  
**Autor:** Equipe de Desenvolvimento

---

## ?? ÍNDICE

1. [Visão Geral](#visão-geral)
2. [Mudanças em Materiais](#mudanças-em-materiais)
3. [Mudanças em Postagens](#mudanças-em-postagens)
4. [Mudanças em Avaliações](#mudanças-em-avaliações)
5. [Mudanças em Comentários](#mudanças-em-comentários)
6. [Banco de Dados](#banco-de-dados)
7. [Controllers](#controllers)
8. [Models](#models)

---

## ?? VISÃO GERAL

Este documento descreve as principais mudanças realizadas no backend do sistema Atria, focando em:
- **Materiais** - Sistema de materiais acadêmicos com avaliações
- **Postagens** - Sistema de postagens em comunidades e fórum geral
- **Avaliações** - Sistema de avaliação de materiais
- **Comentários** - Sistema de comentários em postagens e avaliações

---

## ?? MUDANÇAS EM MATERIAIS

### **MateriaisController.cs**

#### **1. Método Index**
```csharp
public async Task<IActionResult> Index(string? tipo, string? ordenacao)
{
    var query = _context.Materiais
   .Include(m => m.Criador)
        .Include(m => m.Avaliacoes)
        .AsQueryable();

    // Filtrar por tipo
    if (!string.IsNullOrEmpty(tipo))
        query = query.Where(m => m.Tipo == tipo);

    // Aplicar ordenação
    query = ordenacao switch
  {
        "titulo" => query.OrderBy(m => m.Titulo),
        "tipo" => query.OrderBy(m => m.Tipo).ThenBy(m => m.Titulo),
  "recentes" => query.OrderByDescending(m => m.DataCriacao),
        _ => query.OrderByDescending(m => m.DataCriacao)
    };

    return View(await query.ToListAsync());
}
```

**Funcionalidades:**
- ? Filtro por tipo de material
- ? Ordenação (título, tipo, recentes)
- ? Carrega avaliações e criador

#### **2. Método Details**
```csharp
public async Task<IActionResult> Details(int? id)
{
    var material = await _context.Materiais
        .Include(m => m.Criador)
        .Include(m => m.Avaliacoes!)
            .ThenInclude(a => a.Usuario)
        .Include(m => m.Avaliacoes!)
            .ThenInclude(a => a.Comentarios!)
            .ThenInclude(c => c.Usuario)
        .FirstOrDefaultAsync(m => m.Id == id);
    
    return View(material);
}
```

**Funcionalidades:**
- ? Carrega material com avaliações
- ? Carrega comentários das avaliações
- ? Carrega usuários relacionados

#### **3. Método Create**
```csharp
[HttpPost]
public async Task<IActionResult> Create([Bind("Titulo,Descricao,Tipo,Status")] Material material)
{
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    
    material.FKUsuarioCriador = userId;
  material.DataCriacao = DateTime.UtcNow;
    material.Status = material.Status ?? "Pendente";

  _context.Add(material);
    await _context.SaveChangesAsync();

    TempData["SuccessMessage"] = "Material criado com sucesso!";
    return RedirectToAction(nameof(Index));
}
```

**Funcionalidades:**
- ? Vincula usuário criador automaticamente
- ? Define data de criação
- ? Status padrão "Pendente"

---

## ?? MUDANÇAS EM POSTAGENS

### **PostagensController.cs**

#### **1. Método Index**
```csharp
public async Task<IActionResult> Index()
{
    var postagens = await _context.Postagens
        .Include(p => p.Usuario)
        .Include(p => p.Comunidade)
    .Include(p => p.GrupoEstudo)
        .Where(p => p.Titulo != null && p.Conteudo != null)
        .ToListAsync();

    var comunidades = await _context.Comunidades
     .OrderBy(c => c.Nome)
  .ToListAsync();

    ViewBag.Comunidades = comunidades;
    return View(postagens);
}
```

**Funcionalidades:**
- ? Lista postagens com relacionamentos
- ? Filtra postagens válidas
- ? Carrega comunidades para filtro

#### **2. Método Details**
```csharp
public async Task<IActionResult> Details(int? id)
{
    var postagem = await _context.Postagens
.Include(p => p.Usuario)
        .Include(p => p.Comunidade)
        .Include(p => p.GrupoEstudo)
   .Include(p => p.Comentarios!)
            .ThenInclude(c => c.Usuario)
        .FirstOrDefaultAsync(p => p.Id == id);
    
    return View(postagem);
}
```

**Funcionalidades:**
- ? Carrega postagem com comentários
- ? Carrega usuários dos comentários
- ? **VERSÃO SIMPLIFICADA** - Sem comentários aninhados

#### **3. Método Create**
```csharp
[HttpPost]
public async Task<IActionResult> Create([Bind("Titulo,Conteudo,FKComunidade,FKGrupo,NoForumGeral")] Postagem postagem)
{
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    
 postagem.FKUsuario = userId;
  postagem.DataPostagem = DateTime.UtcNow;

    // Garantir valores padrão para FKs opcionais
    if (!postagem.FKComunidade.HasValue || postagem.FKComunidade == 0)
        postagem.FKComunidade = null;
    
    if (!postagem.FKGrupo.HasValue || postagem.FKGrupo == 0)
        postagem.FKGrupo = null;

    _context.Add(postagem);
    await _context.SaveChangesAsync();

    TempData["SuccessMessage"] = "Postagem criada com sucesso!";
    return RedirectToAction(nameof(Index));
}
```

**Funcionalidades:**
- ? Vincula usuário automaticamente
- ? Define data de postagem
- ? Permite postagem sem comunidade (fórum geral)
- ? Permite postagem sem grupo

#### **4. Método Edit**
```csharp
[HttpPost]
public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Conteudo,FKComunidade,FKGrupo,NoForumGeral")] Postagem postagemEditada)
{
    var postagemOriginal = await _context.Postagens.FindAsync(id);
    
 // Atualizar APENAS campos editáveis
    postagemOriginal.Titulo = postagemEditada.Titulo;
    postagemOriginal.Conteudo = postagemEditada.Conteudo;
    postagemOriginal.FKComunidade = postagemEditada.FKComunidade;
    postagemOriginal.FKGrupo = postagemEditada.FKGrupo;
    postagemOriginal.NoForumGeral = postagemEditada.NoForumGeral;

    // FK_USUARIO e DATA_POSTAGEM preservados!
    
    _context.Update(postagemOriginal);
    await _context.SaveChangesAsync();

    TempData["SuccessMessage"] = "Postagem atualizada com sucesso!";
    return RedirectToAction(nameof(Index));
}
```

**Funcionalidades:**
- ? Preserva usuário criador
- ? Preserva data de criação
- ? Atualiza apenas campos editáveis

---

## ? MUDANÇAS EM AVALIAÇÕES

### **AvaliacoesController.cs**

#### **1. Método Create**
```csharp
[HttpPost]
public async Task<IActionResult> Create([Bind("Nota,Resenha,FKMaterial")] Avaliacao avaliacao)
{
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

    // Verificar se usuário já avaliou
    var avaliacaoExistente = await _context.Avaliacoes
     .FirstOrDefaultAsync(a => a.FKUsuario == userId && a.FKMaterial == avaliacao.FKMaterial);

    if (avaliacaoExistente != null)
    {
        TempData["ErrorMessage"] = "Você já avaliou este material.";
return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
    }

    avaliacao.FKUsuario = userId;
    avaliacao.TipoAvaliacao = "Comum";

    _context.Add(avaliacao);
    await _context.SaveChangesAsync();

    TempData["SuccessMessage"] = "Avaliação criada com sucesso!";
    return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
}
```

**Funcionalidades:**
- ? Vincula usuário automaticamente
- ? Previne avaliações duplicadas
- ? Define tipo de avaliação
- ? Redireciona para detalhes do material

---

## ?? MUDANÇAS EM COMENTÁRIOS

### **ComentarioController.cs**

#### **Método Create**
```csharp
[HttpPost]
public async Task<IActionResult> Create([Bind("Conteudo,FKPostagem")] Comentario comentario)
{
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

    comentario.FKUsuario = userId;
    comentario.DataComentario = DateTime.UtcNow;

    _context.Add(comentario);

    // Atualiza visibilidade da postagem
    var postagem = await _context.Postagens.FindAsync(comentario.FKPostagem);
    if (postagem != null)
    {
        postagem.SetVisibleOnGeral();
        _context.Update(postagem);
    }

    await _context.SaveChangesAsync();

    TempData["SuccessMessage"] = "Comentário adicionado com sucesso!";
    return RedirectToAction("Details", "Postagens", new { id = comentario.FKPostagem });
}
```

**Funcionalidades:**
- ? Vincula usuário automaticamente
- ? Define data do comentário
- ? Atualiza visibilidade da postagem
- ? **VERSÃO SIMPLIFICADA** - Sem comentários aninhados

### **ComentarioAvaliacaoController.cs**

#### **Método Create**
```csharp
[HttpPost]
public async Task<IActionResult> Create([Bind("Conteudo,FKAvaliacao")] ComentarioAvaliacao comentario)
{
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

    comentario.FKUsuario = userId;
    comentario.DataComentario = DateTime.Now;

    _context.Add(comentario);
    await _context.SaveChangesAsync();

    TempData["SuccessMessage"] = "Comentário adicionado com sucesso!";
    
    // Redirecionar para a página do material
    var avaliacao = await _context.Avaliacoes.FindAsync(comentario.FKAvaliacao);
    return RedirectToAction("Details", "Materiais", new { id = avaliacao.FKMaterial });
}
```

**Funcionalidades:**
- ? Vincula usuário automaticamente
- ? Define data do comentário
- ? Redireciona para detalhes do material
- ? **VERSÃO SIMPLIFICADA** - Sem comentários aninhados

---

## ??? BANCO DE DADOS

### **Estrutura Atual**

#### **TB_COMENTARIO**
```sql
CREATE TABLE TB_COMENTARIO (
    ID_COMENTARIO INT AUTO_INCREMENT PRIMARY KEY,
    CONTEUDO TEXT NOT NULL,
    DATA_COMENTARIO DATETIME NOT NULL,
    FK_USUARIO INT NOT NULL,
 FK_POSTAGEM INT NOT NULL,
    
    FOREIGN KEY (FK_USUARIO) REFERENCES TB_USUARIO(ID_USUARIO) ON DELETE CASCADE,
  FOREIGN KEY (FK_POSTAGEM) REFERENCES TB_POSTAGEM(ID_POSTAGEM) ON DELETE CASCADE
);
```

**? REMOVIDO:** `FK_COMENTARIO_PAI` (comentários aninhados)

#### **TB_COMENTARIO_AVALIACAO**
```sql
CREATE TABLE TB_COMENTARIO_AVALIACAO (
    ID_COMENTARIO_AVALIACAO INT AUTO_INCREMENT PRIMARY KEY,
    CONTEUDO VARCHAR(500) NOT NULL,
    DATA_COMENTARIO DATETIME NOT NULL,
    FK_USUARIO INT NOT NULL,
    FK_AVALIACAO INT NOT NULL,
    
    FOREIGN KEY (FK_USUARIO) REFERENCES TB_USUARIO(ID_USUARIO) ON DELETE CASCADE,
    FOREIGN KEY (FK_AVALIACAO) REFERENCES TB_AVALIACAO(ID_AVALIACAO) ON DELETE CASCADE
);
```

**? REMOVIDO:** `FK_COMENTARIO_PAI` (comentários aninhados)

---

## ?? MODELS

### **Comentario.cs**
```csharp
public class Comentario
{
    public int Id { get; set; }
    public string Conteudo { get; set; } = string.Empty;
    public DateTime DataComentario { get; set; } = DateTime.UtcNow;
 public int FKUsuario { get; set; } = 0;
    public ApplicationUser? Usuario { get; set; }
    public int FKPostagem { get; set; } = 0;
    public Postagem? Postagem { get; set; }
}
```

**? REMOVIDO:**
- `FKComentarioPai`
- `ComentarioPai`
- `Respostas`

### **ComentarioAvaliacao.cs**
```csharp
public class ComentarioAvaliacao
{
    public int Id { get; set; }
    public string Conteudo { get; set; } = string.Empty;
    public DateTime DataComentario { get; set; } = DateTime.UtcNow;
    public int FKUsuario { get; set; } = 0;
    public ApplicationUser? Usuario { get; set; }
    public int FKAvaliacao { get; set; } = 0;
    public Avaliacao? Avaliacao { get; set; }
}
```

**? REMOVIDO:**
- `FKComentarioPai`
- `ComentarioPai`
- `Respostas`

---

## ?? RESUMO DAS MUDANÇAS

### **? FUNCIONALIDADES ATIVAS:**

| Funcionalidade | Status |
|----------------|--------|
| Materiais - CRUD | ? Funcionando |
| Materiais - Filtros | ? Funcionando |
| Materiais - Ordenação | ? Funcionando |
| Postagens - CRUD | ? Funcionando |
| Postagens - Comunidades | ? Funcionando |
| Postagens - Fórum Geral | ? Funcionando |
| Avaliações - Criar | ? Funcionando |
| Avaliações - Validação | ? Funcionando |
| Comentários em Postagens | ? Funcionando |
| Comentários em Avaliações | ? Funcionando |

### **? FUNCIONALIDADES REMOVIDAS:**

| Funcionalidade | Status |
|----------------|--------|
| Comentários Aninhados | ? Removido |
| Respostas (Replies) | ? Removido |
| Threads de Discussão | ? Removido |

---

## ?? CONFIGURAÇÃO DO ApplicationDbContext

### **Comentario Mapping**
```csharp
builder.Entity<Comentario>(b =>
{
    b.ToTable("TB_COMENTARIO");
    b.HasKey(c => c.Id);
    b.Property(c => c.Conteudo).IsRequired();
    b.Property(c => c.DataComentario);
  
    b.HasOne(c => c.Postagem)
        .WithMany(p => p.Comentarios)
        .HasForeignKey(c => c.FKPostagem)
        .OnDelete(DeleteBehavior.Cascade);

    b.HasOne(c => c.Usuario)
        .WithMany()
 .HasForeignKey(c => c.FKUsuario)
        .OnDelete(DeleteBehavior.Cascade);
});
```

**? REMOVIDO:** Relacionamento auto-referencial

### **ComentarioAvaliacao Mapping**
```csharp
builder.Entity<ComentarioAvaliacao>(b =>
{
    b.ToTable("TB_COMENTARIO_AVALIACAO");
    b.HasKey(c => c.Id);
    b.Property(c => c.Conteudo).HasMaxLength(500).IsRequired();
    b.Property(c => c.DataComentario);
    
  b.HasOne(c => c.Avaliacao)
   .WithMany(a => a.Comentarios)
    .HasForeignKey(c => c.FKAvaliacao)
        .OnDelete(DeleteBehavior.Cascade);

    b.HasOne(c => c.Usuario)
   .WithMany()
        .HasForeignKey(c => c.FKUsuario)
        .OnDelete(DeleteBehavior.Cascade);
});
```

**? REMOVIDO:** Relacionamento auto-referencial

---

## ?? NOTAS FINAIS

### **Decisões de Design:**
1. **Comentários Aninhados Removidos** - Optamos por simplicidade
2. **Validação no Backend** - Todas as validações estão nos controllers
3. **TempData para Mensagens** - Mensagens de sucesso/erro via TempData
4. **Redirecionamentos Inteligentes** - Sempre redirecionam para página relevante

### **Boas Práticas Implementadas:**
- ? Bind apenas campos necessários
- ? Validação de ModelState
- ? Try-catch para operações críticas
- ? TempData para feedback ao usuário
- ? Include para carregar relacionamentos
- ? Nullable FKs para relacionamentos opcionais

### **Segurança:**
- ? [Authorize] nos controllers
- ? ValidateAntiForgeryToken em POST
- ? Claims para identificar usuário
- ? Validação de ownership implícita (FK_USUARIO)

---

## ?? PRÓXIMOS PASSOS RECOMENDADOS

1. **Testes Unitários** - Criar testes para controllers
2. **Logs Estruturados** - Implementar logging adequado
3. **Paginação** - Adicionar paginação em Index
4. **Busca Avançada** - Implementar busca por texto
5. **Cache** - Implementar cache para listas

---

**Documento criado em:** 19/01/2025  
**Última atualização:** 19/01/2025
**Versão do Sistema:** 1.0
