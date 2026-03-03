using System.ComponentModel.DataAnnotations;

namespace Portal.API.ViewModel
{
    public class InvoiceViewModel
    {
        public Guid Id { get; set; }                 

        [Required]
        public Guid VendedorId { get; set; }         

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Valor { get; set; }           

        public DateTime DataEmissao { get; set; }   

        public string? Status { get; set; }         

        public decimal? ValorComissao { get; set; }

        public DateTime? CriadoEm { get; set; }      
        public DateTime? AtualizadoEm { get; set; }  
      
        public string? VendedorNome { get; set; }    
        public string? VendedorCpf { get; set; }    
    }
}
