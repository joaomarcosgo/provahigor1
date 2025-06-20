using System;
using System.ComponentModel.DataAnnotations;

namespace ProvaHigorr.Models
{
    public class Cliente
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(18)]
        public string? CodigoFiscal { get; set; } // CPF ou CNPJ

        [Required]
        [StringLength(15)]
        public string? InscricaoEstadual { get; set; }

        [Required]
        [StringLength(100)]
        public string? Nome { get; set; }

        [StringLength(100)]
        public string? NomeFantasia { get; set; }

        [Required]
        [StringLength(200)]
        public string? Endereco { get; set; }

        [Required]
        [StringLength(10)]
        public string? Numero { get; set; }

        [Required]
        [StringLength(100)]
        public string? Bairro { get; set; }

        [Required]
        [StringLength(100)]
        public string? Cidade { get; set; }

        [Required]
        [StringLength(2)]
        public string? Estado { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        public string? ImagemPath { get; set; } // Caminho da imagem
    }
}
