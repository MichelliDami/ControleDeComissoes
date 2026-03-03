using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application.Vendedores.DTOs
{
    public class CadastrarVendedorDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefone { get; set; } = string.Empty;
        public decimal PercentualComissao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
