using Business.Core;
using Data.Interfaces;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class VentaProductoService : ServiceBase<VentaProductoDto, VentaProductoDto, VentaProducto>
    {
        public VentaProductoService(IVentaProductoRepository repository, ILogger<VentaProductoService> logger)
            : base(repository, logger) { }
    }
}
