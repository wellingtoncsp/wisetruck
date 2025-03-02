using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseTruck.API.Models
{
    public class Viagem
    {
        [Key]
        [Column("cod_viagem")]
        public int Id { get; set; }

        [Required]
        [Column("cod_caminhao")]
        public int CaminhaoId { get; set; }

        [Column("cod_motorista")]
        public int? MotoristaId { get; set; }

        [Required]
        [Column("dsc_origem")]
        public string Origem { get; set; }

        [Required]
        [Column("dsc_destino")]
        public string Destino { get; set; }

        [Required]
        [Column("num_distancia_km")]
        public int DistanciaKm { get; set; }

        [Required]
        [Column("dat_inicio")]
        public DateTime DataInicio { get; set; }

        [Column("dat_fim")]
        public DateTime? DataFim { get; set; }

        [Required]
        [Column("flg_status")]
        public string Status { get; set; }

        // Propriedades de navegação
        [ForeignKey("CaminhaoId")]
        public virtual Caminhao Caminhao { get; set; }

        [ForeignKey("MotoristaId")]
        public virtual Motorista Motorista { get; set; }

        public virtual ICollection<Pedagio> Pedagios { get; set; }
        public virtual ICollection<Abastecimento> Abastecimentos { get; set; }
    }
} 