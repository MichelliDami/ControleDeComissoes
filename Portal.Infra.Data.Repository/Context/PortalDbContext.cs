using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.Domain.Models;

namespace Portal.Infra.Data.Repository.Context
{
    public class PortalDbContext : DbContext
    {
        public PortalDbContext(DbContextOptions<PortalDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vendedor> Vendedores => Set<Vendedor>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<Comissao> Comissoes { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PortalDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
