using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application.Invoices.DTOs
{
    public class InvoiceIndicadoresDto

    {
        public int TotalInvoicesCriadas { get; set; }

        public int TotalPendentes { get; set; }
        public int TotalAprovadas { get; set; }
        public int TotalCanceladas { get; set; }

        public decimal ValorTotalInvoicesAprovadas { get; set; }

        public int InvoicesUltimos30Dias { get; set; }
    }
}
