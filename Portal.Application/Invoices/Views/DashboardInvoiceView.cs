using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Base;

namespace Portal.Application.Invoices.Views
{
    public class DashboardInvoiceView : ViewBase
    {
        public int TotalInvoicesCriadas { get; set; }

        public int TotalPendentes { get; set; }
        public int TotalAprovadas { get; set; }
        public int TotalCanceladas { get; set; }

        public decimal ValorTotalAprovadas { get; set; }

        public int InvoicesUltimos30Dias { get; set; }

        public InvoiceStatus? StatusFiltrado { get; set; }
        public int? TotalDoStatusFiltrado { get; set; }
    }
}
