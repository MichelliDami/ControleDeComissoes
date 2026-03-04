using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Portal.Application.Base;
using Portal.Application.Dashboard.DTOs;
using Portal.Application.Invoices.DTOs;
using Portal.Application.Invoices.Views;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;


namespace Portal.Application.Invoices.Services
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IVendedorRepository _vendedorRepository;
        private readonly IComissaoRepository _comissaoRepository;
        private readonly IValidator<Invoice> _validator;

        public InvoiceAppService(
            IValidator<Invoice> validator,
            IInvoiceRepository invoiceRepository,
            IVendedorRepository vendedorRepository,
            IComissaoRepository comissaoRepository)
        {
            _validator = validator;
            _invoiceRepository = invoiceRepository;
            _vendedorRepository = vendedorRepository;
            _comissaoRepository = comissaoRepository;
        }

        public async Task<ServiceResult> CriarAsync(CriarInvoiceDto dto)
        {

            var vendedor = await _vendedorRepository.GetByIdAsync(dto.VendedorId);

            if (vendedor is null)
            {
                return ServiceResult.Falha("Vendedor não encontrado.");
            }

            if (!vendedor.Ativo)
            {
                return ServiceResult.Falha("Vendedor inativo não pode receber comissões.");
            }

            var invoice = new Invoice(
                dto.DataEmissao,
                dto.VendedorId,
                dto.ClienteNome,
                dto.ClienteDocumento,
                dto.ValorTotal,
                dto.Observacoes
            );

            var validation = await _validator.ValidateAsync(invoice);

            if (!validation.IsValid)
                return ServiceResult.Falha(string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage)));



            await _invoiceRepository.AddAsync(invoice);

            var comissao = new Comissao(
               invoice.Id,
               vendedor.Id,
               invoice.ValorTotal,
               vendedor.PercentualComissao
           );

            invoice.VincularComissao(comissao);
            await _comissaoRepository.AddAsync(comissao);

            await _invoiceRepository.UnitOfWork.CommitAsync();


            return ServiceResult.BemSucedido(InvoiceView.Map(invoice));
        }

        public async Task<ServiceResult> AtualizarAsync(Guid id, AtualizarInvoiceDto dto)
        {


            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice is null)
            {
                return ServiceResult.Falha("Invoice não encontrada.");
            }

            var vendedorIdAtual = invoice.VendedorId;
            var statusAtual = invoice.Status;

            if (statusAtual == InvoiceStatus.Aprovada && dto.VendedorId != vendedorIdAtual)
            {
                return ServiceResult.Falha("Não é permitido alterar o vendedor após aprovação.");

            }

            invoice.AtualizarDados(
                dto.DataEmissao,
                dto.VendedorId,
                dto.ClienteNome,
                dto.ClienteDocumento,
                dto.ValorTotal,
                dto.Status,
                dto.Observacoes
            );

            var validation = await _validator.ValidateAsync(invoice);

            if (!validation.IsValid)
                return ServiceResult.Falha(string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage)));

            var vendedor = await _vendedorRepository.GetByIdAsync(invoice.VendedorId);
            if (vendedor is null)
            {
                return ServiceResult.Falha("Vendedor não encontrado.");

            }

            if (!vendedor.Ativo)
            {
                return ServiceResult.Falha("Vendedor inativo não pode receber comissões.");

            }

            var comissao = await _comissaoRepository.GetByInvoiceIdAsync(invoice.Id);
            if (comissao is null)
            {
                return ServiceResult.Falha("Comissão não encontrada para esta Invoice.");

            }

            comissao.Recalcular(invoice.ValorTotal, vendedor.PercentualComissao);
            await _comissaoRepository.UpdateAsync(comissao);

            await _invoiceRepository.UpdateAsync(invoice);

            return ServiceResult.BemSucedido(InvoiceView.Map(invoice));
        }

        public async Task<ServiceResult> ExcluirAsync(Guid id)
        {

            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice is null)
            {
                return ServiceResult.Falha("Invoice não encontrada.");
            }

            await _invoiceRepository.DeleteAsync(invoice);
            return ServiceResult.BemSucedido();
        }

        public async Task<ServiceResult> ObterAsync(Guid Id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(Id);

            if (invoice is null)
                return ServiceResult.Falha("Invoice não encontrada.");

            return ServiceResult.BemSucedido(InvoiceView.Map(invoice));
        }

        public async Task<ServiceResult> ListarAsync()
        {
            var list = await _invoiceRepository.ListAsync();
            return ServiceResult.BemSucedido(list.Select(InvoiceView.Map).ToList());
        }

        public async Task<ServiceResult> ObterDashboardAsync(DashboardInvoiceParametrosDto parametros)
        {
            var query = (await _invoiceRepository.ListAsync()).AsQueryable();


            if (parametros.DataInicio.HasValue)
                query = query.Where(i => i.DataEmissao >= parametros.DataInicio.Value);

            if (parametros.DataFim.HasValue)
                query = query.Where(i => i.DataEmissao <= parametros.DataFim.Value);

            if (parametros.VendedorId.HasValue)
                query = query.Where(i => i.VendedorId == parametros.VendedorId.Value);

            var invoicesBase = query.ToList();

            var hoje = DateTime.Today;
            var ultimos30 = hoje.AddDays(-30);


            var totalPendentes = invoicesBase.Count(i => i.Status == InvoiceStatus.Pendente);
            var totalAprovadas = invoicesBase.Count(i => i.Status == InvoiceStatus.Aprovada);
            var totalCanceladas = invoicesBase.Count(i => i.Status == InvoiceStatus.Cancelada);

            int? totalStatusFiltrado = null;
            if (parametros.Status.HasValue)
            {
                totalStatusFiltrado = parametros.Status.Value switch
                {
                    InvoiceStatus.Pendente => totalPendentes,
                    InvoiceStatus.Aprovada => totalAprovadas,
                    InvoiceStatus.Cancelada => totalCanceladas,
                    _ => 0
                };
            }

            var view = new DashboardInvoiceView
            {
                TotalInvoicesCriadas = invoicesBase.Count,

                TotalPendentes = totalPendentes,
                TotalAprovadas = totalAprovadas,
                TotalCanceladas = totalCanceladas,

                ValorTotalAprovadas = invoicesBase
                    .Where(i => i.Status == InvoiceStatus.Aprovada)
                    .Sum(i => i.ValorTotal),

                InvoicesUltimos30Dias = invoicesBase.Count(i => i.DataEmissao >= ultimos30),

                StatusFiltrado = parametros.Status,
                TotalDoStatusFiltrado = totalStatusFiltrado
            };

            return ServiceResult.BemSucedido(view);
        }


        public async Task<ServiceResult> ObterDashboardComissoesAsync(DashboardInvoiceParametrosDto parametros)
        {
            var comissoes = await _comissaoRepository.ListAsync();

            // ⚠️ precisa que Invoice esteja carregada (Include no repo)
            // Filtro por período (DataEmissao da Invoice)
            if (parametros.DataInicio.HasValue)
                comissoes = comissoes
                    .Where(c => c.Invoice != null && c.Invoice.DataEmissao >= parametros.DataInicio.Value)
                    .ToList();

            if (parametros.DataFim.HasValue)
                comissoes = comissoes
                    .Where(c => c.Invoice != null && c.Invoice.DataEmissao <= parametros.DataFim.Value)
                    .ToList();

            // Filtro por vendedor
            if (parametros.VendedorId.HasValue)
                comissoes = comissoes
                    .Where(c => c.VendedorId == parametros.VendedorId.Value)
                    .ToList();

            // ✅ Filtro por STATUS da Invoice
            if (parametros.Status.HasValue) // aqui eu estou assumindo que seu dto tem Status (InvoiceStatus?)
                comissoes = comissoes
                    .Where(c => c.Invoice != null && c.Invoice.Status == parametros.Status.Value)
                    .ToList();

            var totalPendentes = comissoes
                .Where(c => c.Status == ComissaoStatus.Pendente)
                .Sum(c => c.ValorComissao);

            var totalPagas = comissoes
                .Where(c => c.Status == ComissaoStatus.Paga)
                .Sum(c => c.ValorComissao);

            var totais = comissoes
                .GroupBy(c => c.VendedorId)
                .Select(g => new
                {
                    VendedorId = g.Key,
                    Total = g.Sum(x => x.ValorComissao)
                })
                .ToList();

            var vendedores = await _vendedorRepository.ListAsync();

            var lista = totais
                .Select(x =>
                {
                    var vendedor = vendedores.FirstOrDefault(v => v.Id == x.VendedorId);

                    return new VendedorComissaoTotalView
                    {
                        VendedorId = x.VendedorId,
                        Nome = vendedor?.Nome ?? "Vendedor",
                        TotalComissao = x.Total
                    };
                })
                .OrderByDescending(x => x.TotalComissao)
                .ToList();

            var view = new DashboardComissaoView
            {
                Vendedores = lista,
                TotalComissoesPendentes = totalPendentes,
                TotalComissoesPagas = totalPagas,
                Top5Vendedores = lista.Take(5).ToList()
            };

            return ServiceResult.BemSucedido(view);
        }
    }
}
