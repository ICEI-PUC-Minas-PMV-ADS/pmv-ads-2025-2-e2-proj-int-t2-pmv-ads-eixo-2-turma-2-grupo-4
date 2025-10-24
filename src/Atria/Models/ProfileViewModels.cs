using System.ComponentModel.DataAnnotations;

namespace Atria.Models
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "O nome � obrigat�rio")]
        [Display(Name = "Nome Completo")]
        [MaxLength(150, ErrorMessage = "O nome n�o pode ter mais de 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email � obrigat�rio")]
        [EmailAddress(ErrorMessage = "Email inv�lido")]
        [Display(Name = "Email")]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "�rea de Estudo")]
        [MaxLength(200, ErrorMessage = "A �rea de estudo n�o pode ter mais de 200 caracteres")]
        public string? AreaEstudo { get; set; }

        [Display(Name = "Trilha de Conhecimento")]
        [MaxLength(500, ErrorMessage = "A trilha de conhecimento n�o pode ter mais de 500 caracteres")]
        public string? TrilhaConhecimento { get; set; }

        [Display(Name = "Projetos")]
        [MaxLength(1000, ErrorMessage = "Os projetos n�o podem ter mais de 1000 caracteres")]
        public string? Projetos { get; set; }

        [Display(Name = "Tipo de Usu�rio")]
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
    }
}
