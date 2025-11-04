using Business.Core;
using Data.Interfaces;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class MascotaClienteService : ServiceBase<MascotaClienteDto, MascotaClienteDto, MascotaCliente>
    {
        public MascotaClienteService(IMascotaClienteRepository repository, ILogger<MascotaClienteService> logger)
            : base(repository, logger) { }
    }
}
