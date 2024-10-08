﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDesafio2.Models
{
    public class OfertaEmpleo
    {
        [Key]
        public int OfertaId { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreOferta { get; set; }

        [Required]
        [StringLength(255)]
        public string DescripcionOferta { get; set; }

        [Required]
        [StringLength(255)]
        public string Requisitos { get; set; }

        [Required]
        public decimal Salario { get; set; }

        [Required]
        public DateTime FechaPublicacion { get; set; }

        [Required]
        public string Contacto { get; set; }

        [ForeignKey("Usuarios")]
        public string UsuarioId { get; set; }
        //public UsuarioDTO UsuarioOfertador { get; set; }
    }
}
