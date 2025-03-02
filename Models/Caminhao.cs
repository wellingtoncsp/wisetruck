using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseTruck.API.Models
{
    public class Caminhao
    {
        [Key]
        [Column("cod_caminhao")]
        public int Id { get; set; }

        [Required]
        [Column("dsc_modelo")]
        public string Modelo { get; set; }

        [Required]
        [Column("num_placa")]
        public string Placa { get; set; }

        [Required]
        [Column("dat_fabricacao")]
        public DateTime DataFabricacao { get; set; }

        [Required]
        [Column("flg_status")]
        public string Status { get; set; }
    }
}