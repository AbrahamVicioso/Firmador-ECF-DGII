using Firmador_API.Exceptions;
using Firmador_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Firmador_API.Repositories
{
    public interface IEmpresaRepository
    {
        Task<Empresa> GetEmpresaAsync(string schema, short empresa);
    }

    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly CedestechContext _context;

        public EmpresaRepository(CedestechContext context)
        {
            this._context = context;
        }

        public async Task<Empresa?> GetEmpresaAsync(string schema, short empresa)
        {
            if (!Regex.IsMatch(schema, @"^[A-Z][A-Z0-9_]*$"))
                throw new ArgumentException("Schema inválido");

            return await _context.Empresas
                .FromSqlRaw($@"
                    SELECT *
                    FROM {schema}.COM_CAT_EMPRESAS
                    WHERE COD_EMPRESA = {{0}}",
                    empresa)
                .FirstOrDefaultAsync();
        }
    }
}
