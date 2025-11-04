using Business.Core;
using Data.Interfaces;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class PagoService : ServiceBase<PagoDto, PagoDto, Pago>
    {
        public PagoService(IPagoRepository repository, ILogger<PagoService> logger)
            : base(repository, logger) { }
    }
}
