using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Models;

namespace Portal.Domain.Interfaces
{
    public interface IVendedorRepository
    {
        Task<bool> ExisteCpfAsync(string cpf, Guid ignorarId);
        Task<bool> ExisteEmailAsync(string email, Guid ignorarId);
        Task UpdateAsync(Vendedor vendedor);
        Task DeleteAsync(Vendedor vendedor);
        Task<List<Vendedor>> ListAsync();
        Task AddAsync(Vendedor vendedor);
        Task<Vendedor?> GetByIdAsync(Guid id);
    }
}
