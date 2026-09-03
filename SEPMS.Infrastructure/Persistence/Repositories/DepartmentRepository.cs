using SEPMS.Application.Abstractions;
using SEPMS.Application.Common;
using SEPMS.Domain.Entities;
using SEPMS.Infrastructure.Persistence;

namespace SEPMS.Infrastructure.Persistence.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public PagedResult<Department> GetPaged(ListQuery query)
        {
            var departments = _context.Departments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var term = query.Search;
                departments = departments.Where(d =>
                    d.DepartmentName.Contains(term)
                    || d.DepartmentCode.Contains(term)
                    || (d.Description != null && d.Description.Contains(term)));
            }

            if (query.Status == "active")
            {
                departments = departments.Where(d => d.IsActive);
            }
            else if (query.Status == "inactive")
            {
                departments = departments.Where(d => !d.IsActive);
            }

            departments = (query.Sort, query.Dir) switch
            {
                ("code", "desc") => departments.OrderByDescending(d => d.DepartmentCode),
                ("code", _) => departments.OrderBy(d => d.DepartmentCode),
                ("status", "desc") => departments.OrderByDescending(d => d.IsActive),
                ("status", _) => departments.OrderBy(d => d.IsActive),
                ("created", "desc") => departments.OrderByDescending(d => d.CreatedDate),
                ("created", _) => departments.OrderBy(d => d.CreatedDate),
                ("name", "desc") => departments.OrderByDescending(d => d.DepartmentName),
                _ => departments.OrderBy(d => d.DepartmentName)
            };

            var totalCount = departments.Count();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)query.PageSize));
            query.Page = Math.Clamp(query.Page, 1, totalPages);

            return new PagedResult<Department>
            {
                Items = departments.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToList(),
                TotalCount = totalCount,
                Search = query.Search,
                Status = query.Status,
                Sort = query.Sort,
                Dir = query.Dir,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalInDatabase = _context.Departments.Count(),
                ActiveInDatabase = _context.Departments.Count(d => d.IsActive)
            };
        }

        public Department? GetById(int id) => _context.Departments.Find(id);

        public void Add(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }

        public void Update(Department department)
        {
            _context.SaveChanges();
        }

        public void Delete(Department department)
        {
            _context.Departments.Remove(department);
            _context.SaveChanges();
        }

        public IReadOnlyList<Department> GetLookup() =>
            _context.Departments.OrderBy(d => d.DepartmentName).ToList();
    }
}
