using Firmador_API.Exceptions;
using Firmador_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Firmador_API.Repositories
{
    public interface IEmpresaRepository
    {
        Task<Empresa> GetEmpresaByCodEmpresa(short empresa);
    }

    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly CedestechContext _context;

        public EmpresaRepository(CedestechContext context)
        {
            this._context = context;
        }

        public async Task<Empresa> GetEmpresaByCodEmpresa(short empresa)
        {

            var result = await _context.Empresas
                .Where(t => t.CodEmpresa == empresa)
                .FirstOrDefaultAsync();

            if (result is null)
            {
                throw new NotFoundException();
            }

            return result;
        }
    }
}
