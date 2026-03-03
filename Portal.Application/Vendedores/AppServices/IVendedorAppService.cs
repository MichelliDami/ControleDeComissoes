using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Vendedores.DTOs;
using Portal.Domain.Validation;

namespace Portal.Application.Vendedores.Services
{
    public interface IVendedorAppService
    {
        Task<ValidationResult> CadastrarAsync(CadastrarVendedorDto dto);
    }
}
