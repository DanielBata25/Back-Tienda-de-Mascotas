using Data.Core;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Entity.Context;

namespace Data.Repositories
{
    public class PagoRepository : GenericRepository<Pago>, IPagoRepository
    {
        public PagoRepository(ApplicationDbContext context, ILogger<PagoRepository> logger)
            : base(context, logger) { }

        public async Task<IEnumerable<Pago>> GetPagosPorVentaAsync(int ventaId)
        {
            return await _context.Pagos
                .Where(p => p.VentaId == ventaId && !p.IsDeleted)
                .ToListAsync();
        }
    }
}
