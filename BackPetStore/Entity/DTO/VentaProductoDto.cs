
namespace Entity.DTOs
{
    public class VentaProductoDto : BaseDTO
    {
        public int VentaId { get; set; }
        public int ProductoId { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
