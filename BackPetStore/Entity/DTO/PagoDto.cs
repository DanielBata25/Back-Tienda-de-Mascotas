
namespace Entity.DTOs
{
    public class PagoDto : BaseDTO
    {
        public int VentaId { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
        public string EstadoPago { get; set; }
        public string Referencia { get; set; }
    }
}
