using Business.Core;
using Data.Interfaces;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class MascotaService : ServiceBase<MascotaDto, MascotaDto, Mascota>
    {
        public MascotaService(IMascotaRepository repository, ILogger<MascotaService> logger)
            : base(repository, logger) { }
    }
}
