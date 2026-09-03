using SEPMS.Application.Common;
using SEPMS.Domain.Entities;

namespace SEPMS.Application.Abstractions
{
    public interface IDepartmentService
    {
        PagedResult<Department> GetPaged(string? search, string? status, string? sort, string? dir, int page, int pageSize);
        Department? GetById(int id);
        void Create(Department department);
        bool Update(Department department);
        bool Delete(int id);
        IReadOnlyList<Department> GetLookup();
    }
}
