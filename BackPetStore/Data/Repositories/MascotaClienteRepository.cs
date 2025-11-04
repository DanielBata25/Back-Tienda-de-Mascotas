using Data.Core;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class MascotaClienteRepository : GenericRepository<MascotaCliente>, IMascotaClienteRepository
    {
        public MascotaClienteRepository(ApplicationDbContext context, ILogger<MascotaClienteRepository> logger)
            : base(context, logger) { }

        public async Task<IEnumerable<MascotaCliente>> GetHistorialPorMascotaAsync(int mascotaId)
        {
            return await _context.MascotaClientes
                .Include(mc => mc.Mascota)
                .Include(mc => mc.Cliente)
                .Where(mc => mc.MascotaId == mascotaId && !mc.IsDeleted)
                .OrderByDescending(mc => mc.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<MascotaCliente>> GetHistorialPorClienteAsync(int clienteId)
        {
            return await _context.MascotaClientes
                .Include(mc => mc.Mascota)
                .Include(mc => mc.Cliente)
                .Where(mc => mc.ClienteId == clienteId && !mc.IsDeleted)
                .OrderByDescending(mc => mc.FechaRegistro)
                .ToListAsync();
        }
    }
}
