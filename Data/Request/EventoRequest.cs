using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEventos.Data
{
    public partial class EventoRequest
    {
        [Required(ErrorMessage = "El campo 'NombreEvento' es requerido.")]
        [MaxLength(100, ErrorMessage = "El campo 'NombreEvento' no puede exceder los 100 caracteres.")]
        public string NombreEvento { get; set; } = null!;

        [Required(ErrorMessage = "El campo 'FechaEvento' es requerido.")]
        public DateOnly FechaEvento { get; set; }

        [Required(ErrorMessage = "El campo 'HoraEvento' es requerido.")]
        public TimeOnly HoraEvento { get; set; }

        [Required(ErrorMessage = "El campo 'DescripcionEvento' es requerido.")]
        [MaxLength(200, ErrorMessage = "El campo 'DescripcionEvento' no puede exceder los 200 caracteres.")]
        public string DescripcionEvento { get; set; } = null!;

        [Required(ErrorMessage = "El campo 'CostoEvento' es requerido.")]
        public decimal CostoEvento { get; set; }

        [Required(ErrorMessage = "El campo 'EstadoEvento' es requerido.")]
        [MaxLength(20, ErrorMessage = "El campo 'EstadoEvento' no puede exceder los 20 caracteres.")]
        public string EstadoEvento { get; set; } = null!;

        //lugar
        [Required(ErrorMessage = "El campo 'Descripcion' es requerido.")]
        [MaxLength(200, ErrorMessage = "El campo 'Descripcion' no puede exceder los 200 caracteres.")]
        public string DescripcionLugar { get; set; } = null!;

        [Required(ErrorMessage = "El campo 'Latitud' es requerido.")]
        public decimal Latitud { get; set; }

        [Required(ErrorMessage = "El campo 'Longitud' es requerido.")]
        public decimal Longitud { get; set; }
        public List<int>? InvitadosEspeciales { get; set; }
    }
}
