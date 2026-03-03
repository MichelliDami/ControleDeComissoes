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
    public class VendedorMapping : IEntityTypeConfiguration<Vendedor>
    {
        public void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            builder.ToTable("Vendedor");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Cpf)
                   .IsRequired()
                   .HasMaxLength(11);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.PercentualComissao)
                   .IsRequired()
                   .HasPrecision(5, 2);

            builder.Property(x => x.Ativo)
                   .IsRequired();

            builder.Property(x => x.DataCadastro)
                   .IsRequired();

            builder.HasIndex(x => x.Cpf)
                   .IsUnique();

            builder.HasIndex(x => x.Email)
                   .IsUnique();
        }
    }
}
