using Portal.Application.Base;
using Portal.Application.Dashboard.DTOs;
using Portal.Application.Invoices.DTOs;

namespace Portal.Application.Invoices.Services
{
    public interface IInvoiceAppService : IAplicBase
    {
        Task<ServiceResult> CriarAsync(CriarInvoiceDto dto);
        Task<ServiceResult> AtualizarAsync(Guid id, AtualizarInvoiceDto dto);
        Task<ServiceResult> ExcluirAsync(Guid id);
        Task<ServiceResult> ObterDashboardAsync(DashboardInvoiceParametrosDto parametros);
        Task<ServiceResult> ObterDashboardComissoesAsync(DashboardInvoiceParametrosDto parametros);

    }
}
