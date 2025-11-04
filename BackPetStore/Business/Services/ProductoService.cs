using Business.Core;
using Data.Interfaces;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class ProductoService : ServiceBase<ProductoDto, ProductoDto, Producto>
    {
        public ProductoService(IProductoRepository repository, ILogger<ProductoService> logger)
            : base(repository, logger) { }
    }
}
