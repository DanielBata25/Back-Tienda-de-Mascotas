using Business.Core;
using Data.Interfaces;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class ClienteService : ServiceBase<ClienteDto, ClienteDto, Cliente>
    {
        public ClienteService(IClienteRepository repository, ILogger<ClienteService> logger)
            : base(repository, logger) { }
    }
}
