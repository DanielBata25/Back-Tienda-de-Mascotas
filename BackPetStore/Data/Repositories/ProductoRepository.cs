using Data.Core;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Entity.Context;

namespace Data.Repositories
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(ApplicationDbContext context, ILogger<ProductoRepository> logger)
            : base(context, logger) { }

        public async Task<IEnumerable<Producto>> GetActivosAsync()
        {
            return await _context.Productos
                .Where(p => p.Estado == "Activo" && !p.IsDeleted)
                .ToListAsync();
        }
    }
}
