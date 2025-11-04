
namespace Entity.DTOs
{
    public class ProductoDto : BaseDTO
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public string Estado { get; set; }
    }
}
