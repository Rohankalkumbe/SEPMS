using SEPMS.Application.Common;
using SEPMS.Domain.Entities;

namespace SEPMS.Application.Abstractions
{
    public interface IEmployeeRepository
    {
        PagedResult<Employee> GetPaged(ListQuery query);
        Employee? GetById(int id);
        Employee? GetByIdWithDepartment(int id);
        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);
    }
}
