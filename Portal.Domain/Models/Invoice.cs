using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Portal.Domain.Models
{
    public class Invoice : Entity
    {
        public int Numero { get; private set; }
        public DateTime DataEmissao { get; private set; }

        public Guid VendedorId { get; private set; }
        public Vendedor Vendedor { get; private set; }

        public string ClienteNome { get; private set; }
        public string ClienteDocumento { get; private set; }

        public decimal ValorTotal { get; private set; }

        public InvoiceStatus Status { get; private set; }

        public string? Observacoes { get; private set; }

        public Comissao? Comissao { get; private set; }

        protected Invoice() { }

        public Invoice(
            DateTime dataEmissao,
            Guid vendedorId,
            string clienteNome,
            string clienteDocumento,
            decimal valorTotal,
            string? observacoes = null)
        {
            DataEmissao = dataEmissao;
            VendedorId = vendedorId;
            ClienteNome = clienteNome;
            ClienteDocumento = clienteDocumento;
            ValorTotal = valorTotal;
            Observacoes = observacoes;
            Status = InvoiceStatus.Pendente;
        }



        public void Aprovar()
        {
            if (Status == InvoiceStatus.Aprovada)
                throw new Exception("Invoice já está aprovada.");

            Status = InvoiceStatus.Aprovada;
        }

        public void AtualizarDados(
        DateTime dataEmissao,
        Guid vendedorId,
        string clienteNome,
        string clienteDocumento,
        decimal valorTotal,
        InvoiceStatus status,
        string? observacoes
         )
        {
            DataEmissao = dataEmissao;
            VendedorId = vendedorId;
            ClienteNome = clienteNome;
            ClienteDocumento = clienteDocumento;
            ValorTotal = valorTotal;
            Status = status;
            Observacoes = observacoes;
        }


        public void VincularComissao(Comissao comissao)
        {
            Comissao = comissao;
        }

        public void Cancelar()
        {
            Status = InvoiceStatus.Cancelada;
        }
    }
}

public enum InvoiceStatus
{
    Pendente = 1,
    Aprovada = 2,
    Cancelada = 3
}

