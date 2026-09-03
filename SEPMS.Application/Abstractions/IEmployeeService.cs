using SEPMS.Application.Common;
using SEPMS.Domain.Entities;

namespace SEPMS.Application.Abstractions
{
    public interface IEmployeeService
    {
        PagedResult<Employee> GetPaged(string? search, string? status, string? sort, string? dir, int? departmentId, int page, int pageSize);
        Employee? GetById(int id);
        Employee? GetByIdWithDepartment(int id);
        void Create(Employee employee);
        bool Update(Employee employee);
        bool Delete(int id);
        IReadOnlyList<Department> GetDepartmentLookup();
    }
}
