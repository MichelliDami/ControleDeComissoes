using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portal.API.Controllers;
using Portal.Application.Vendedores.DTOs;
using Portal.Application.Vendedores.Services;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarVendedorDto dto)
        {
            return await Executar(async () => await _aplic.AtualizarAsync(id, dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            return await Executar(async () => await _aplic.ExcluirAsync(id));
        }


    }
}