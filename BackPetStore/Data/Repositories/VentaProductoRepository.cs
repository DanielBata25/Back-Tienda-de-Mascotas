using Data.Core;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class VentaProductoRepository : GenericRepository<VentaProducto>, IVentaProductoRepository
    {
        public VentaProductoRepository(ApplicationDbContext context, ILogger<VentaProductoRepository> logger)
            : base(context, logger) { }

        public async Task<IEnumerable<VentaProducto>> GetDetallePorVentaAsync(int ventaId)
        {
            return await _context.VentaProductos
                .Include(vp => vp.Producto)
                .Include(vp => vp.Venta)
                .Where(vp => vp.VentaId == ventaId && !vp.IsDeleted)
                .ToListAsync();
        }
    }
}
