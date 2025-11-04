    using Entity.Context;             
    using Microsoft.EntityFrameworkCore; 
    using Microsoft.Extensions.Logging;
    using System.Text.Json;

    namespace Data.Core
    {
        public class GenericRepository<T> : IRepository<T> where T : class
        {
            protected readonly ApplicationDbContext _context;
            private readonly ILogger _logger;

            public GenericRepository(ApplicationDbContext context, ILogger<GenericRepository<T>> logger)
            {
                _context = context;
                _logger = logger;
            }

            public virtual async Task<IEnumerable<T>> GetAllAsync()
            {
                return await _context.Set<T>()
                    .Where(p => EF.Property<bool>(p, "IsDeleted") == false)
                    .ToListAsync();
            }

            public async Task<T?> GetByIdAsync(int id)
            {
                return await _context.Set<T>().FindAsync(id);
            }

            public async Task<T> AddAsync(T entity)
            {
                await _context.Set<T>().AddAsync(entity);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al agregar entidad {typeof(T).Name}: {JsonSerializer.Serialize(entity)}");
                    throw;
                }
                return entity;
            }

            public async Task<bool> UpdateAsync(T entity)
            {
                _context.Set<T>().Update(entity);
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al actualizar entidad {typeof(T).Name}: {JsonSerializer.Serialize(entity)}");
                    throw;
                }
            }

            public async Task<bool> DeleteAsync(int id)
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null) return false;

                _context.Set<T>().Remove(entity);
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al eliminar físicamente entidad {typeof(T).Name}: {JsonSerializer.Serialize(entity)}");
                    throw;
                }
            }

            public async Task<bool> DeleteLogicalAsync(int id)
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null) return false;

                var prop = entity.GetType().GetProperty("IsDeleted");
                if (prop != null)
                {
                    prop.SetValue(entity, true);
                    try
                    {
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error en eliminación lógica de entidad {typeof(T).Name}: {JsonSerializer.Serialize(entity)}");
                        throw;
                    }
                }

                _logger.LogWarning($"La entidad {typeof(T).Name} no tiene propiedad IsDeleted");
                return false;
            }
        }
    }
