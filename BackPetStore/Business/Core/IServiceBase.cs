namespace Business.Core
{
    public interface IServiceBase<TDto, TRequestDTO, TEntity>
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> GetByIdAsync(int id);
        Task<TRequestDTO> CreateAsync(TRequestDTO dto);
        Task<TRequestDTO> UpdateAsync(TRequestDTO dto);
        Task<bool> DeletePermanentAsync(int id);
        Task<bool> DeleteLogicalAsync(int id);
    }
}
