using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Models
{
    public class Comissao : Entity
    {
        public Guid InvoiceId { get; private set; }
        public Guid VendedorId { get; private set; }

        public decimal ValorBase { get; private set; }
        public decimal PercentualAplicado { get; private set; }
        public decimal ValorComissao { get; private set; }

        public ComissaoStatus Status { get; private set; }
        public DateTime DataCalculo { get; private set; }
        public DateTime? DataPagamento { get; private set; }

        public Invoice Invoice { get; private set; } = default!; 

        private Comissao() { }

        public Comissao(Guid invoiceId, Guid vendedorId, decimal valorBase, decimal percentualAplicado)
        {
            InvoiceId = invoiceId;
            VendedorId = vendedorId;

            ValorBase = valorBase;
            PercentualAplicado = percentualAplicado;

            ValorComissao = Calcular(valorBase, percentualAplicado);

            Status = ComissaoStatus.Pendente;
            DataCalculo = DateTime.Now;
            DataPagamento = null;
        }


        public void Cancelar()
        {
            Status = ComissaoStatus.Cancelada;
            DataPagamento = null;
        }

        private static decimal Calcular(decimal valorBase, decimal percentualAplicado)
        {
     
            return Math.Round(valorBase * (percentualAplicado / 100m), 2);
        }

        public void Recalcular(decimal valorBase, decimal percentualAplicado)
        {
            ValorBase = valorBase;
            PercentualAplicado = percentualAplicado;
            ValorComissao = Calcular(valorBase, percentualAplicado);
            DataCalculo = DateTime.Now;
        }
    }

    public enum ComissaoStatus
    {
        Pendente = 1,
        Paga = 2,
        Cancelada = 3
    }


}
