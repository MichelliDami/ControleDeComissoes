using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Invoices.DTOs;
using FluentValidation.Results;
using Portal.Application.Base;

namespace Portal.Application.Invoices.Services
{
    public interface IInvoiceAppService : IAplicBase
    {
        Task<ServiceResult> CriarAsync(CriarInvoiceDto dto);
        Task<ServiceResult> AtualizarAsync(Guid id, AtualizarInvoiceDto dto);
        Task<ServiceResult> ExcluirAsync(Guid id);
  
    }
}
