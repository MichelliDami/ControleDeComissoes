using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Vendedores.DTOs;
using FluentValidation.Results;

namespace Portal.Application.Vendedores.Services
{
        public interface IVendedorAppService
        {
            Task<(ValidationResult Validation, VendedorResponseDto? Data)> CadastrarAsync(CadastrarVendedorDto dto);

            Task<(ValidationResult Validation, VendedorResponseDto? Data)> AtualizarAsync(Guid id, AtualizarVendedorDto dto);

            Task<ValidationResult> ExcluirAsync(Guid id);

            Task<VendedorResponseDto?> ObterAsync(Guid id);

            Task<List<VendedorResponseDto>> ListarAsync();
        }
    
}
