using Microsoft.Extensions.Logging;
using Mapster;
using Data.Core;
using Utilities;

namespace Business.Core
{
    /// <summary>
    /// Clase base abstracta para servicios de negocio que encapsula operaciones CRUD genéricas
    /// sobre cualquier entidad, utilizando un patrón de repositorio y mapeo automático con Mapster.
    /// </summary>
    /// <typeparam name="TDto">Tipo de objeto de transferencia de datos (DTO).</typeparam>
    /// <typeparam name="TEntity">Tipo de entidad del dominio correspondiente en la base de datos.</typeparam>
    public abstract class ServiceBase<TDto, TRequestDTO, TEntity> : IServiceBase<TDto, TRequestDTO, TEntity>
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly ILogger _logger;

        protected ServiceBase(IRepository<TEntity> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                return entities.Adapt<IEnumerable<TDto>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de {Entity}", typeof(TEntity).Name);
                throw;
            }
        }

        public virtual async Task<TDto> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    throw new EntityNotFoundException(typeof(TEntity).Name, id);

                return entity.Adapt<TDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro con ID {Id} de {Entity}", id, typeof(TEntity).Name);
                throw;
            }
        }

        public virtual async Task<TRequestDTO> CreateAsync(TRequestDTO dto)
        {
            try
            {
                var entity = dto.Adapt<TEntity>();
                var created = await _repository.AddAsync(entity);
                return created.Adapt<TRequestDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear entidad {Entity}", typeof(TEntity).Name);
                throw;
            }
        }

        public virtual async Task<TRequestDTO> UpdateAsync(TRequestDTO dto)
        {
            try
            {
                var entity = dto.Adapt<TEntity>();
                var updated = await _repository.UpdateAsync(entity);
                if (!updated)
                    throw new Exception("Update failed");

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar entidad {Entity}", typeof(TEntity).Name);
                throw;
            }
        }

        public virtual async Task<bool> DeletePermanentAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente el registro con ID {Id} de {Entity}", id, typeof(TEntity).Name);
                throw;
            }
        }

        public virtual async Task<bool> DeleteLogicalAsync(int id)
        {
            try
            {
                return await _repository.DeleteLogicalAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente el registro con ID {Id} de {Entity}", id, typeof(TEntity).Name);
                throw;
            }
        }
    }
}
