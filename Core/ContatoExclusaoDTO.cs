using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class ContatoExclusaoDTO
    {
        [Required]
        public int DDD { get; set; }

        [Required]
        public int Telefone { get; set; }
    }
}
