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
    public class ComissaoRepository : IComissaoRepository
    {
        private readonly PortalDbContext _context;

        public ComissaoRepository(PortalDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Comissao comissao)
        {
            await _context.Comissoes.AddAsync(comissao);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comissao comissao)
        {
            _context.Comissoes.Update(comissao);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Comissao comissao)
        {
            _context.Comissoes.Remove(comissao);
            await _context.SaveChangesAsync();
        }

        public async Task<Comissao?> GetByIdAsync(Guid id)
        {
            return await _context.Comissoes
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comissao?> GetByInvoiceIdAsync(Guid invoiceId)
        {
            return await _context.Comissoes
                .FirstOrDefaultAsync(c => c.InvoiceId == invoiceId);
        }

        public async Task<List<Comissao>> ListAsync()
        {
            return await _context.Comissoes.ToListAsync();
        }

        public Task<bool> ExisteComissaoParaVendedorAsync(Guid vendedorId)
        {
            return _context.Comissoes.AnyAsync(c => c.VendedorId == vendedorId);
        }
    }
}
