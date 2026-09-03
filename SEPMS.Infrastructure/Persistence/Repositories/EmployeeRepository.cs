using Microsoft.EntityFrameworkCore;
using SEPMS.Application.Abstractions;
using SEPMS.Application.Common;
using SEPMS.Domain.Entities;
using SEPMS.Infrastructure.Persistence;

namespace SEPMS.Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public PagedResult<Employee> GetPaged(ListQuery query)
        {
            var employees = _context.Employees.Include(e => e.Department).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var term = query.Search;
                employees = employees.Where(e =>
                    e.Name.Contains(term)
                    || e.Email.Contains(term)
                    || (e.Phone != null && e.Phone.Contains(term))
                    || e.JobTitle.Contains(term)
                    || (e.Department != null && e.Department.DepartmentName.Contains(term)));
            }

            if (query.Status == "active")
            {
                employees = employees.Where(e => e.IsActive);
            }
            else if (query.Status == "inactive")
            {
                employees = employees.Where(e => !e.IsActive);
            }

            if (query.DepartmentId is > 0)
            {
                employees = employees.Where(e => e.DepartmentId == query.DepartmentId);
            }

            employees = (query.Sort, query.Dir) switch
            {
                ("email", "desc") => employees.OrderByDescending(e => e.Email),
                ("email", _) => employees.OrderBy(e => e.Email),
                ("phone", "desc") => employees.OrderByDescending(e => e.Phone),
                ("phone", _) => employees.OrderBy(e => e.Phone),
                ("department", "desc") => employees.OrderByDescending(e => e.Department!.DepartmentName),
                ("department", _) => employees.OrderBy(e => e.Department!.DepartmentName),
                ("title", "desc") => employees.OrderByDescending(e => e.JobTitle),
                ("title", _) => employees.OrderBy(e => e.JobTitle),
                ("status", "desc") => employees.OrderByDescending(e => e.IsActive),
                ("status", _) => employees.OrderBy(e => e.IsActive),
                ("name", "desc") => employees.OrderByDescending(e => e.Name),
                _ => employees.OrderBy(e => e.Name)
            };

            var totalCount = employees.Count();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)query.PageSize));
            query.Page = Math.Clamp(query.Page, 1, totalPages);

            return new PagedResult<Employee>
            {
                Items = employees.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToList(),
                TotalCount = totalCount,
                Search = query.Search,
                Status = query.Status,
                Sort = query.Sort,
                Dir = query.Dir,
                Page = query.Page,
                PageSize = query.PageSize,
                DepartmentId = query.DepartmentId,
                TotalInDatabase = _context.Employees.Count(),
                ActiveInDatabase = _context.Employees.Count(e => e.IsActive)
            };
        }

        public Employee? GetById(int id) => _context.Employees.Find(id);

        public Employee? GetByIdWithDepartment(int id) =>
            _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.EmployeeId == id);

        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void Update(Employee employee)
        {
            _context.SaveChanges();
        }

        public void Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}
