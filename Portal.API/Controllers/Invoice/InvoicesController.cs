using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portal.Application.Dashboard.DTOs;
using Portal.Application.Invoices.DTOs;
using Portal.Application.Invoices.Services;
using System.Net;

namespace Portal.API.Controllers.Invoice
{
    [Route("api/invoices")]
    public class InvoicesController : ControllerAplicacaoBase<IInvoiceAppService>
    {
        public InvoicesController(IInvoiceAppService aplic) : base(aplic)
        {
        }


        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarInvoiceDto invoiceDto)
        {
            return await Executar(async () => await _aplic.CriarAsync(invoiceDto));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarInvoiceDto invoiceDto)
        {
            return await Executar(async () => await _aplic.AtualizarAsync(id, invoiceDto));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPedido(Guid id)
        {
            return await Executar(async () => await _aplic.ExcluirAsync(id));
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard([FromQuery] DashboardInvoiceParametrosDto filtros)
        {
            return await Executar(async () => await _aplic.ObterDashboardAsync(filtros));
        }

        [HttpGet("dashboard/comissoes")]
        public async Task<IActionResult> DashboardComissoes([FromQuery] DashboardInvoiceParametrosDto filtros)
        {
            return await Executar(async () => await _aplic.ObterDashboardComissoesAsync(filtros));
        }


    }
}