
namespace Entity.DTOs
{
    public class MascotaClienteDto : BaseDTO
    {
        public int MascotaId { get; set; }
        public string MascotaNombre { get; set; }

        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}
