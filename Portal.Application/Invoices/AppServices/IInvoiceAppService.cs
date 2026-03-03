using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Invoices.DTOs;
using FluentValidation.Results;

namespace Portal.Application.Invoices.Services
{
    public interface IInvoiceAppService
    {
        Task<(ValidationResult Validation, InvoiceResponseDto? Data)> CriarAsync(CriarInvoiceDto dto);
        Task<(ValidationResult Validation, InvoiceResponseDto? Data)> AtualizarAsync(Guid id, AtualizarInvoiceDto dto);
        Task<ValidationResult> ExcluirAsync(Guid id);
        Task<InvoiceResponseDto?> ObterAsync(Guid id);
        Task<List<InvoiceResponseDto>> ListarAsync();
    }
}
