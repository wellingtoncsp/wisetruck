using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseTruck.API.Models
{
    public class Pedagio
    {
        [Key]
        [Column("cod_pedagio")]
        public int Id { get; set; }

        [Required]
        [Column("cod_viagem")]
        public int ViagemId { get; set; }

        [Required]
        [Column("num_valor")]
        public decimal Valor { get; set; }

        [Required]
        [Column("dsc_localizacao")]
        public string Localizacao { get; set; }

        [Required]
        [Column("dat_pagamento")]
        public DateTime DataPagamento { get; set; }

        // Propriedade de navegação
        [ForeignKey("ViagemId")]
        public virtual Viagem Viagem { get; set; }
    }
} 