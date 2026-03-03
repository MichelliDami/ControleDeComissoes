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
    public class VendedorRepository : IVendedorRepository
    {
        private readonly PortalDbContext _context;

        public VendedorRepository(PortalDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Vendedor vendedor)
        {
            await _context.Vendedores.AddAsync(vendedor);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Vendedor>> ListAsync()
        {
            return await _context.Vendedores.ToListAsync();
        }
        public Task<bool> ExisteCpfAsync(string cpf, Guid ignorarId)
        {
            return _context.Vendedores.AnyAsync(v => v.Cpf == cpf && v.Id != ignorarId);
        }

        public Task<bool> ExisteEmailAsync(string email, Guid ignorarId)
        {
            return _context.Vendedores.AnyAsync(v => v.Email == email && v.Id != ignorarId);
        }
        public async Task<Vendedor?> GetByIdAsync(Guid id)
        {
            return await _context.Vendedores
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task UpdateAsync(Vendedor vendedor)
        {
            _context.Vendedores.Update(vendedor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Vendedor vendedor)
        {
            _context.Vendedores.Remove(vendedor);
            await _context.SaveChangesAsync();
        }

    }
}
