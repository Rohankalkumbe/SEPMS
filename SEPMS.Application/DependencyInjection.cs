using Microsoft.Extensions.DependencyInjection;
using SEPMS.Application.Abstractions;
using SEPMS.Application.Services;

namespace SEPMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            return services;
        }
    }
}
