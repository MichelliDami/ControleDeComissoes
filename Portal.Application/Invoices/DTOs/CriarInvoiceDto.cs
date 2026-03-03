using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application.Invoices.DTOs
{
    public class CriarInvoiceDto
    {
        public DateTime DataEmissao { get; set; }
        public Guid VendedorId { get; set; }

        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteDocumento { get; set; } = string.Empty;

        public decimal ValorTotal { get; set; }
        public string? Observacoes { get; set; }
    }
}
