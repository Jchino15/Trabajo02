using System.ComponentModel.DataAnnotations;

namespace Trabajo02.Models
{
    public class usuarios
    {
        [Key] 
        public int usuarios_id { get; set; }
        public string? nombre { get; set; }
        public string? documento { get; set; }
        public char? tipo { get; set; }
        public string? carnet { get; set; }
        public int? carrera_id { get; set; }

    }
}
