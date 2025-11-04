using Data.Core;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ApplicationDbContext context, ILogger<ClienteRepository> logger)
            : base(context, logger) { }

        public async Task<IEnumerable<Cliente>> GetClientesConMascotasAsync()
        {
            return await _context.Clientes
                .Include(c => c.Mascotas)
                .ThenInclude(mc => mc.Mascota)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }
    }
}
