using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Portal.Application.Base;
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

            var comissao = new Comissao(
                invoice.Id,
                vendedor.Id,
                invoice.ValorTotal,
                vendedor.PercentualComissao
            );

            await _invoiceRepository.AddAsync(invoice);
            await _comissaoRepository.AddAsync(comissao);

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
                return ServiceResult.Falha( "Não é permitido alterar o vendedor após aprovação.");
               
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
                return ServiceResult.Falha( "Vendedor não encontrado.");
               
            }

            if (!vendedor.Ativo)
            {
                return ServiceResult.Falha( "Vendedor inativo não pode receber comissões.");
                
            }

            var comissao = await _comissaoRepository.GetByInvoiceIdAsync(invoice.Id);
            if (comissao is null)
            {
                return ServiceResult.Falha( "Comissão não encontrada para esta Invoice.");
              
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

    }
}
