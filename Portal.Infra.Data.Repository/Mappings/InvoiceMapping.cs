using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Portal.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Portal.Infra.Data.Repository.Mappings
{
    public class InvoiceMapping : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");

            builder.HasKey(x => x.Id);


            builder.Property(x => x.Numero)
             .ValueGeneratedOnAdd()
             .UseIdentityColumn()
             .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.HasIndex(x => x.Numero)
                   .IsUnique();

            builder.Property(x => x.DataEmissao)
                   .IsRequired();

            builder.Property(x => x.ClienteNome)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.ClienteDocumento)
                   .IsRequired()
                   .HasMaxLength(14);

            builder.Property(x => x.ValorTotal)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(x => x.Observacoes)
                   .HasMaxLength(500);

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasConversion<int>();


            builder.HasOne(x => x.Vendedor)
                   .WithMany()
                   .HasForeignKey(x => x.VendedorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

