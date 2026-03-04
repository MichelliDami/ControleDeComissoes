using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Base;
using Portal.Application.Vendedores.DTOs;
using Portal.Domain.Models;

namespace Portal.Application.Vendedores.Views
{
    public class VendedorView :ViewBase
    {
        public string Nome { get;  set; }
        public string Cpf { get;  set; }
        public string Email { get;  set; }
        public decimal PercentualComissao { get;  set; }
        public DateTime DataCadastro { get;  set; }
        public string? Telefone { get;  set; }
        public bool Ativo { get;  set; }

        public static VendedorView Map(Vendedor vendedor) 
        {
            return new VendedorView
            {
                Id = vendedor.Id,
                Nome = vendedor.Nome,
                Cpf = vendedor.Cpf,
                Email = vendedor.Email,
                Telefone = vendedor.Telefone,
                PercentualComissao = vendedor.PercentualComissao,
                Ativo = vendedor.Ativo,
                DataCadastro =vendedor.DataCadastro
            };
        }
    }


}
