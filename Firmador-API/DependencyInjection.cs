using Firmador_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Firmador_API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<Repositories.IEmpresaRepository, Repositories.EmpresaRepository>();

            services.AddDbContext<CedestechContext>(options => options.UseOracle(configuration.GetSection("ConnectionStrings:DefaultConnection").Value));
            return services;
        }
    }
}
