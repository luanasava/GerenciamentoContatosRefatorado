using System.ComponentModel.DataAnnotations;

namespace TechChallengeFase3.Models.DTO
{
    public class ContatoExclusaoDTO
    {
        [Required]
        public int DDD { get; set; }

        [Required]
        public int Telefone { get; set; }
    }
}
