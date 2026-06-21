public interface IPackageService
{
    Task<List<PackageDto>> GetAllAsync();
    Task<PackageDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreatePackageRequest request);
    Task<bool> UpdateAsync(int id, UpdatePackageRequest request);
    Task<bool> DeleteAsync(int id);
}