namespace Portal.API.ViewModel
{
    public class VendedorViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = default!;
        public string Cpf { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Telefone { get; set; }
        public decimal PercentualComissao { get; set; }
        public bool Status { get; set; }
    }
}
