using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Base;
using Portal.Application.Invoices.Comissoes.Views;
using Portal.Application.Invoices.DTOs;
using Portal.Application.Vendedores.Views;
using Portal.Domain.Models;

namespace Portal.Application.Invoices.Views
{
    public class InvoiceView : ViewBase
    {
        public int Numero { get; set; }
        public DateTime DataEmissao { get; set; }

        public Guid VendedorId { get; set; }
        public VendedorView Vendedor { get; set; }

        public string ClienteNome { get; set; }
        public string ClienteDocumento { get; set; }

        public decimal ValorTotal { get; set; }

        public InvoiceStatus Status { get; set; }

        public string? Observacoes { get; set; }

        public ComissaoView? ComissaoView { get; set; }

        public static InvoiceView Map(Invoice invoice)
        {
            return new InvoiceView
            {
                Id = invoice.Id,
                Numero = invoice.Numero,
                DataEmissao = invoice.DataEmissao,
                VendedorId = invoice.VendedorId,
                ClienteNome = invoice.ClienteNome,
                ClienteDocumento = invoice.ClienteDocumento,
                ValorTotal = invoice.ValorTotal,
                Status = invoice.Status,
                Observacoes = invoice.Observacoes,

                ComissaoView = invoice.Comissao is null ? null : ComissaoView.Map(invoice.Comissao)

            };
        }
    }

}
