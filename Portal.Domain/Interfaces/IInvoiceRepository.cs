using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Models;

namespace Portal.Domain.Interfaces
{
    public interface IInvoiceRepository
    {
        Task AddAsync(Invoice invoice);
        Task UpdateAsync(Invoice invoice);
        Task DeleteAsync(Invoice invoice);

        Task<Invoice?> GetByIdAsync(Guid id);
        Task<List<Invoice>> ListAsync();
    }
}
