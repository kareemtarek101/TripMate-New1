public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<int> CreateAsync(CreateCategoryRequest request);
}