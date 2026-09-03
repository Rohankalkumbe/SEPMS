using SEPMS.Application.Common;
using SEPMS.Domain.Entities;

namespace SEPMS.Application.Abstractions
{
    public interface IDepartmentRepository
    {
        PagedResult<Department> GetPaged(ListQuery query);
        Department? GetById(int id);
        void Add(Department department);
        void Update(Department department);
        void Delete(Department department);
        IReadOnlyList<Department> GetLookup();
    }
}
