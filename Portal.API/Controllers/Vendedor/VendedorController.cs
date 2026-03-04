using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portal.API.Controllers;
using Portal.Application.Vendedores.DTOs;
using Portal.Application.Vendedores.Services;
using Portal.Domain.Notifications.Portal.Domain.Notifications;
using System.Collections.Generic;
using System.Net;

namespace Portal.API.Controllers.Vendedor
{
    [Route("api/vendedor")]
    public class VendedoresController : ControllerAplicacaoBase<IVendedorAppService>
    {
        public VendedoresController(IVendedorAppService aplic) : base(aplic)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CadastrarVendedorDto dto)
        {
            return await Executar(async () => await _aplic.CadastrarAsync(dto));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarVendedorDto dto)
        {
            return await Executar(async () => await _aplic.AtualizarAsync(id, dto));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            return await Executar(async () => await _aplic.ExcluirAsync(id));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Obter(Guid id)
        {
            return await Executar(async () => await _aplic.ObterAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return await Executar(async () => await _aplic.ListarAsync());
        }
    }
}