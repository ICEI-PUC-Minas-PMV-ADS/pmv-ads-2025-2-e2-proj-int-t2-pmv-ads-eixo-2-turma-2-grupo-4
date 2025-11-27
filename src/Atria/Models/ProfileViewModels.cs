using System.ComponentModel.DataAnnotations;

namespace Atria.Models
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome Completo")]
        [MaxLength(150, ErrorMessage = "O nome não pode ter mais de 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "Email")]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Área de Estudo")]
        [MaxLength(200, ErrorMessage = "A área de estudo não pode ter mais de 200 caracteres")]
        public string? AreaEstudo { get; set; }

        [Display(Name = "Trilha de Conhecimento")]
        [MaxLength(500, ErrorMessage = "A trilha de conhecimento não pode ter mais de 500 caracteres")]
        public string? TrilhaConhecimento { get; set; }

        [Display(Name = "Projetos")]
        [MaxLength(1000, ErrorMessage = "Os projetos não podem ter mais de 1000 caracteres")]
        public string? Projetos { get; set; }

        [Display(Name = "Tipo de Usuário")]
        public string? TipoUsuario { get; set; }
    }

    public class ViewProfileViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AreaEstudo { get; set; }
        public string? TrilhaConhecimento { get; set; }
        public string? Projetos { get; set; }
        public string? TipoUsuario { get; set; }
        public DateTime DataCadastro { get; set; }
        public int NumeroSeguidores { get; set; }
        public int NumeroSeguindo { get; set; }
    }
}
