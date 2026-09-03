using SEPMS.Application.Abstractions;
using SEPMS.Application.Common;
using SEPMS.Domain.Entities;

namespace SEPMS.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private static readonly string[] SortColumns = ["name", "code", "status", "created"];
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public PagedResult<Department> GetPaged(string? search, string? status, string? sort, string? dir, int page, int pageSize)
        {
            return _repository.GetPaged(ListQuery.Create(search, status, sort, dir, page, pageSize, SortColumns));
        }

        public Department? GetById(int id) => _repository.GetById(id);

        public void Create(Department department) => _repository.Add(department);

        public bool Update(Department department)
        {
            var existing = _repository.GetById(department.DepartmentId);
            if (existing == null)
            {
                return false;
            }

            existing.DepartmentName = department.DepartmentName;
            existing.DepartmentCode = department.DepartmentCode;
            existing.Description = department.Description;
            existing.IsActive = department.IsActive;
            _repository.Update(existing);
            return true;
        }

        public bool Delete(int id)
        {
            var department = _repository.GetById(id);
            if (department == null)
            {
                return false;
            }

            _repository.Delete(department);
            return true;
        }

        public IReadOnlyList<Department> GetLookup() => _repository.GetLookup();
    }
}
