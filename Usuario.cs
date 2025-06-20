using System;
using System.ComponentModel.DataAnnotations;

namespace ProvaHigorr.Models
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public required string Nome { get; set; }

        [Required]
        [StringLength(50)]
        public required string Login { get; set; }

        [Required]
        public required string SenhaHash { get; set; } // Senha criptografada
    }
}
