using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application.Vendedores.DTOs
{
    public class AtualizarVendedorDto
    {
        public string Nome { get; set; } = default!;
        public string Cpf { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Telefone { get; set; } = default!;
        public decimal PercentualComissao { get; set; }
        public bool Ativo { get; set; }
    }
}
