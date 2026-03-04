using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Models;

namespace Portal.Domain.Interfaces
{
    public interface IComissaoRepository
    {
        Task AddAsync(Comissao comissao);
        Task UpdateAsync(Comissao comissao);
        Task DeleteAsync(Comissao comissao);

        Task<Comissao?> GetByIdAsync(Guid id);


        Task<Comissao?> GetByInvoiceIdAsync(Guid invoiceId);

        Task<List<Comissao>> ListAsync();
        Task<bool> ExisteComissaoParaVendedorAsync(Guid vendedorId);
    }
}
