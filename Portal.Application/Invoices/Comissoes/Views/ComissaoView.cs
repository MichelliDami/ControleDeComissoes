using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Base;
using Portal.Domain.Models;

namespace Portal.Application.Invoices.Comissoes.Views
{
   public  class ComissaoView :ViewBase
    {
        public Guid InvoiceId { get;  set; }
        public Guid VendedorId { get;  set; }

        public decimal ValorBase { get;  set; }
        public decimal PercentualAplicado { get;  set; }
        public decimal ValorComissao { get;  set; }

        public ComissaoStatus Status { get;  set; }
        public DateTime DataCalculo { get;  set; }
        public DateTime? DataPagamento { get;  set; }

        public static ComissaoView Map(Comissao c) => new()
        {
            Id = c.Id,
            InvoiceId = c.InvoiceId,
            VendedorId = c.VendedorId,
            ValorBase = c.ValorBase,
            PercentualAplicado = c.PercentualAplicado,
            ValorComissao = c.ValorComissao,
            Status = c.Status,
            DataCalculo = c.DataCalculo,
            DataPagamento = c.DataPagamento
        };
    }
}
