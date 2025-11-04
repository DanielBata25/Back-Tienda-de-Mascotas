using Data.Core;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Entity.Context;

namespace Data.Repositories
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        public VentaRepository(ApplicationDbContext context, ILogger<VentaRepository> logger)
            : base(context, logger) { }

        public async Task<IEnumerable<Venta>> GetVentasConPagosAsync()
        {
            return await _context.Ventas
                .Include(v => v.Pagos)
                .Include(v => v.Productos)
                .ThenInclude(vp => vp.Producto)
                .Where(v => !v.IsDeleted)
                .ToListAsync();
        }
    }
}
