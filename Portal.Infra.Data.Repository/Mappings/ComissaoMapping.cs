using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Portal.Domain.Models;

namespace Portal.Infra.Data.Repository.Mappings
{
    public class ComissaoMapping : IEntityTypeConfiguration<Comissao>
    {
        public void Configure(EntityTypeBuilder<Comissao> builder)
        {
            builder.ToTable("Comissao");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.InvoiceId)
                   .IsRequired();

            builder.Property(c => c.VendedorId)
                   .IsRequired();

            builder.Property(c => c.ValorBase)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(c => c.PercentualAplicado)
                   .IsRequired()
                   .HasPrecision(5, 2);

            builder.Property(c => c.ValorComissao)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(c => c.Status)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(c => c.DataCalculo)
                   .IsRequired();

            builder.Property(c => c.DataPagamento)
                   .IsRequired(false);

            // 1:1 Invoice -> Comissao
            builder.HasOne(c => c.Invoice)
                   .WithOne(i => i.Comissao)
                   .HasForeignKey<Comissao>(c => c.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade);

            // índice único (uma comissão por invoice)
            builder.HasIndex(c => c.InvoiceId)
                   .IsUnique();

            // índice para busca por vendedor
            builder.HasIndex(c => c.VendedorId);
        }
    }
}
