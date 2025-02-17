using Core;
using System.ComponentModel.DataAnnotations;
using TechChallengeFase1.Models.Entity;

namespace TechChallengeFase1.Models.DTO
{
    public class ContatosDTO
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Nome inválido, informe no mínimo 3 caracteres")]
        [MaxLength(50, ErrorMessage = "Nome excedeu o tamanho permitido")]
        public string Nome { get; set; }
        [Required]
        public int DDD { get; set; }
        [Required]
        public int Telefone { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Regiao { get; set; }

        public ContatosDTO(Contato contato, string regiao)
        {
            Id = contato.Id;
            Nome = contato.Nome;
            DDD = contato.DDD;
            Telefone = contato.Telefone;
            Email = contato.Email;
            Regiao = regiao;
        }

    }
}

