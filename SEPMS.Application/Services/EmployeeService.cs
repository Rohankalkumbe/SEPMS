using SEPMS.Application.Abstractions;
using SEPMS.Application.Common;
using SEPMS.Domain.Entities;

namespace SEPMS.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private static readonly string[] SortColumns = ["name", "email", "phone", "department", "title", "status"];
        private readonly IEmployeeRepository _employees;
        private readonly IDepartmentRepository _departments;

        public EmployeeService(IEmployeeRepository employees, IDepartmentRepository departments)
        {
            _employees = employees;
            _departments = departments;
        }

        public PagedResult<Employee> GetPaged(string? search, string? status, string? sort, string? dir, int? departmentId, int page, int pageSize)
        {
            return _employees.GetPaged(ListQuery.Create(search, status, sort, dir, page, pageSize, SortColumns, departmentId));
        }

        public Employee? GetById(int id) => _employees.GetById(id);

        public Employee? GetByIdWithDepartment(int id) => _employees.GetByIdWithDepartment(id);

        public void Create(Employee employee) => _employees.Add(employee);

        public bool Update(Employee employee)
        {
            var existing = _employees.GetById(employee.EmployeeId);
            if (existing == null)
            {
                return false;
            }

            existing.Name = employee.Name;
            existing.Email = employee.Email;
            existing.Phone = employee.Phone;
            existing.JobTitle = employee.JobTitle;
            existing.DateOfJoining = employee.DateOfJoining;
            existing.IsActive = employee.IsActive;
            existing.DepartmentId = employee.DepartmentId;
            _employees.Update(existing);
            return true;
        }

        public bool Delete(int id)
        {
            var employee = _employees.GetById(id);
            if (employee == null)
            {
                return false;
            }

            _employees.Delete(employee);
            return true;
        }

        public IReadOnlyList<Department> GetDepartmentLookup() => _departments.GetLookup();
    }
}
