using Portal.Domain.Validation;

namespace Portal.Domain.Models
{
    public class Vendedor : Entity
    {


        public string Nome { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }
        public decimal PercentualComissao { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string? Telefone { get; private set; }
        public bool Ativo { get; private set; }
        private Vendedor() { }


        public Vendedor(string nome, string cpf, string email,string? telefone, decimal percentualComissao)
        {
            Nome = nome;
            Cpf = cpf;
            Email = email;
            Telefone = telefone;
            PercentualComissao = percentualComissao;
            DataCadastro = DateTime.Now;
            Ativo = true;

        }

        public void AtualizarDados(
        string nome,
        string cpf,
        string email,
        string? telefone,
        decimal percentualComissao,
        bool ativo)

        {
            Nome = nome;
            Cpf = cpf;
            Email = email;
            Telefone = telefone;
            PercentualComissao = percentualComissao;
            Ativo = ativo;
        }
    }

    }
