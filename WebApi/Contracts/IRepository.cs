namespace WebApi.Contracts
{
    public interface IRepository<TModel>
    {
        public Task CreateAsync(TModel obj);
        public Task DeleteAsync(TModel obj);
        public Task<TModel> UpdateAsync(TModel obj);
        public Task<IEnumerable<TModel>> GetAllAsync();
        public Task<TModel> GetByIdAsync(int id);
    }
}
