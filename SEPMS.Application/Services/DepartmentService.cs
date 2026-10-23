using SEPMS.Application.Abstractions;
using SEPMS.Application.Common;
using SEPMS.Domain.Entities;

namespace SEPMS.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        // ========================================
        // GET PAGED DEPARTMENTS
        // ========================================

        public PagedResult<Department> GetPaged(
            string? search,
            string? status,
            string? sort,
            string? dir,
            int page,
            int pageSize)
        {
            var query = new ListQuery
            {
                Search = search,
                Status = status,
                Sort = sort,
                Dir = dir,
                Page = page,
                PageSize = pageSize
            };

            return _departmentRepository.GetPaged(query);
        }

        // ========================================
        // GET DEPARTMENT BY ID
        // ========================================

        public Department? GetById(int id)
        {
            return _departmentRepository.GetById(id);
        }

        // ========================================
        // CREATE DEPARTMENT
        // ========================================

        public void Create(Department department)
        {
            _departmentRepository.Add(department);
        }

        // ========================================
        // UPDATE DEPARTMENT
        // ========================================

        public bool Update(Department department)
        {
            var existingDepartment =
                _departmentRepository.GetById(department.DepartmentId);

            if (existingDepartment == null)
            {
                return false;
            }

            existingDepartment.DepartmentName = department.DepartmentName;
            existingDepartment.DepartmentCode = department.DepartmentCode;
            existingDepartment.Description = department.Description;
            existingDepartment.IsActive = department.IsActive;

            _departmentRepository.Update(existingDepartment);

            return true;
        }

        // ========================================
        // DELETE DEPARTMENT
        // ========================================

        public bool Delete(int id)
        {
            var department = _departmentRepository.GetById(id);

            if (department == null)
            {
                return false;
            }

            _departmentRepository.Delete(department);

            return true;
        }

        // ========================================
        // DEPARTMENT LOOKUP
        // ========================================

        public IReadOnlyList<Department> GetLookup()
        {
            return _departmentRepository.GetLookup();
        }

        // ========================================
        // DASHBOARD - TOTAL DEPARTMENTS
        // ========================================

        public async Task<int> GetTotalCountAsync()
        {
            return await _departmentRepository.GetTotalCountAsync();
        }
    }
}