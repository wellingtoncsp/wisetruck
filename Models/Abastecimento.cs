using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseTruck.API.Models
{
    public class Abastecimento
    {
        [Key]
        [Column("cod_abastecimento")]
        public int Id { get; set; }

        [Required]
        [Column("cod_viagem")]
        public int ViagemId { get; set; }

        [Required]
        [Column("num_litros")]
        public decimal Litros { get; set; }

        [Required]
        [Column("num_valor")]
        public decimal Valor { get; set; }

        [Required]
        [Column("dsc_localizacao")]
        public string Localizacao { get; set; }

        [Required]
        [Column("dat_abastecimento")]
        public DateTime DataAbastecimento { get; set; }

        // Propriedade de navegação
        [ForeignKey("ViagemId")]
        public virtual Viagem Viagem { get; set; }
    }
} 