using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Invoices.DTOs;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using Portal.Domain.Validation;
using Portal.Domain.Validation.Invoices;

namespace Portal.Application.Invoices.Services
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IVendedorRepository _vendedorRepository;
        private readonly IComissaoRepository _comissaoRepository;

        public InvoiceAppService(
            IInvoiceRepository invoiceRepository,
            IVendedorRepository vendedorRepository,
            IComissaoRepository comissaoRepository
            )
        {
            _invoiceRepository = invoiceRepository;
            _vendedorRepository = vendedorRepository;
            _comissaoRepository = comissaoRepository;
        }

        public async Task<(ValidationResult Validation, InvoiceResponseDto? Data)> CriarAsync(CriarInvoiceDto dto)
        {
            var validation = new ValidationResult();
 
            var vendedor = await _vendedorRepository.GetByIdAsync(dto.VendedorId);
            if (vendedor is null)
            {
                validation.Add(nameof(dto.VendedorId), "Vendedor não encontrado.");
                return (validation, null);
            }

            if (!vendedor.Ativo)
            {
                validation.Add("Vendedor", "Vendedor inativo não pode receber comissões.");
                return (validation, null);
            }

            
            var invoice = new Invoice(
                dto.DataEmissao,
                dto.VendedorId,
                dto.ClienteNome,
                dto.ClienteDocumento,
                dto.ValorTotal,
                dto.Observacoes
            );
       
            var invoiceValidation = new CadastroInvoiceValidator().Validate(invoice);
            if (!invoiceValidation.IsValid)
                return (invoiceValidation, null);

            var comissao = new Comissao(
             invoice.Id,
             vendedor.Id,
             invoice.ValorTotal,
             vendedor.PercentualComissao
             );

            await _invoiceRepository.AddAsync(invoice);
            await _comissaoRepository.AddAsync(comissao);

            return (new ValidationResult(), Map(invoice));
        }

        public async Task<(ValidationResult Validation, InvoiceResponseDto? Data)> AtualizarAsync(Guid id, AtualizarInvoiceDto dto)
        {
            var validation = new ValidationResult();

            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice is null)
            {
                validation.Add("Id", "Invoice não encontrada.");
                return (validation, null);
            }


            var result = invoice.AtualizarDados(
                dto.DataEmissao,
                dto.VendedorId,
                dto.ClienteNome,
                dto.ClienteDocumento,
                dto.ValorTotal,
                dto.Status,
                dto.Observacoes
              
            );

            if (!result.IsValid)
                return (result, null);

            validation = new CadastroInvoiceValidator().Validate(invoice);
            if (!validation.IsValid)
                return (validation, null);

            var vendedor = await _vendedorRepository.GetByIdAsync(invoice.VendedorId);
            if (vendedor is null)
            {
                validation.Add("VendedorId", "Vendedor não encontrado.");
                return (validation, null);
            }

            if (!vendedor.Ativo)
            {
                validation.Add("VendedorId", "Vendedor inativo não pode receber comissões.");
                return (validation, null);
            }

            var comissao = await _comissaoRepository.GetByInvoiceIdAsync(invoice.Id);

            if (comissao is null)
            {
                validation.Add("Comissao", "Comissão não encontrada para esta Invoice.");
                return (validation, null);
            }

            comissao.Recalcular(invoice.ValorTotal, vendedor.PercentualComissao);
            await _comissaoRepository.UpdateAsync(comissao);

            await _invoiceRepository.UpdateAsync(invoice);

            return (new ValidationResult(), Map(invoice));
        }

        public async Task<ValidationResult> ExcluirAsync(Guid id)
        {
            var validation = new ValidationResult();

            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice is null)
            {
                validation.Add("Id", "Invoice não encontrada.");
                return validation;
            }

            await _invoiceRepository.DeleteAsync(invoice);
            return validation;
        }

        public async Task<InvoiceResponseDto?> ObterAsync(Guid id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            return invoice is null ? null : Map(invoice);
        }

        public async Task<List<InvoiceResponseDto>> ListarAsync()
        {
            var list = await _invoiceRepository.ListAsync();
            return list.Select(Map).ToList();
        }

        private static InvoiceResponseDto Map(Invoice invoice)
        {
            return new InvoiceResponseDto
            {
                Id = invoice.Id,
                Numero = invoice.Numero,
                DataEmissao = invoice.DataEmissao,
                VendedorId = invoice.VendedorId,
                ClienteNome = invoice.ClienteNome,
                ClienteDocumento = invoice.ClienteDocumento,
                ValorTotal = invoice.ValorTotal,
                Status = invoice.Status,
                Observacoes = invoice.Observacoes
            };
        }


    }
}
