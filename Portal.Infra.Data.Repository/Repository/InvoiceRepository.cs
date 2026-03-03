using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using Portal.Infra.Data.Repository.Context;

namespace Portal.Infra.Data.Repository.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly PortalDbContext _context;

        public InvoiceRepository(PortalDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Invoice invoice)
        {
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
        }

        public Task<Invoice?> GetByIdAsync(Guid id)
        {
            return _context.Invoices
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<Invoice>> ListAsync()
        {
            return _context.Invoices
                .AsNoTracking()
                .OrderByDescending(x => x.DataEmissao)
                .ToListAsync();
        }
    }
}
