using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Vendedores.DTOs;
using FluentValidation.Results;
using Portal.Application.Base;

namespace Portal.Application.Vendedores.Services
{

    public interface IVendedorAppService : IAplicBase
    {
        Task<ServiceResult> CadastrarAsync(CadastrarVendedorDto dto);

        Task<ServiceResult> AtualizarAsync(Guid id, AtualizarVendedorDto dto);

        Task<ServiceResult> ExcluirAsync(Guid id);
    }
}


