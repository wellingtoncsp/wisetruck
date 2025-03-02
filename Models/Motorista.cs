using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseTruck.API.Models
{
    public class Motorista
    {
        [Key]
        [Column("cod_motorista")]
        public int Id { get; set; }

        [Required]
        [Column("nom_motorista")]
        public string Nome { get; set; }

        [Required]
        [Column("num_cnh")]
        public string CNH { get; set; }

        [Required]
        [Column("dat_validade_cnh")]
        public DateTime ValidadeCNH { get; set; }
    }
}