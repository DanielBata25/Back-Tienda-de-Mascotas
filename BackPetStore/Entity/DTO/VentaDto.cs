
namespace Entity.DTOs
{
    public class VentaDto : BaseDTO
    {
        public int ClienteId { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Estado { get; set; }
        public string Canal { get; set; }
        public string Observaciones { get; set; }
    }
}
