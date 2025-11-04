using Data.Core;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Entity.Context;

namespace Data.Repositories
{
    public class MascotaRepository : GenericRepository<Mascota>, IMascotaRepository
    {
        public MascotaRepository(ApplicationDbContext context, ILogger<MascotaRepository> logger)
            : base(context, logger) { }

        public async Task<IEnumerable<Mascota>> GetMascotasPorClienteAsync(int clienteId)
        {
            return await _context.Mascotas
                .Include(m => m.Clientes)
                .ThenInclude(mc => mc.Cliente)
                .Where(m => m.Clientes.Any(c => c.ClienteId == clienteId) && !m.IsDeleted)
                .ToListAsync();
        }
    }
}
