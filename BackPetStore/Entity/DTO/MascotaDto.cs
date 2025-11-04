
namespace Entity.DTOs
{
    public class MascotaDto : BaseDTO
    {
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Notas { get; set; }
    }
}
