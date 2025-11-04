using Business.Core;
using Data.Interfaces;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class VentaService : ServiceBase<VentaDto, VentaDto, Venta>
    {
        public VentaService(IVentaRepository repository, ILogger<VentaService> logger)
            : base(repository, logger) { }
    }
}
