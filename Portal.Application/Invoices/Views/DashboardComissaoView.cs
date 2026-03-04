using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Base;

namespace Portal.Application.Invoices.Views
{
    public class DashboardComissaoView : ViewBase
    {
        
        public List<VendedorComissaoTotalView> Vendedores { get; set; } = new();

      
        public decimal TotalComissoesPendentes { get; set; }

        public decimal TotalComissoesPagas { get; set; }

    
        public List<VendedorComissaoTotalView> Top5Vendedores { get; set; } = new();
    }

    public class VendedorComissaoTotalView
    {
        public Guid VendedorId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal TotalComissao { get; set; }
    }
}

