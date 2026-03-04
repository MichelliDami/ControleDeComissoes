namespace Portal.Application.Dashboard.DTOs
{
    public class DashboardInvoiceParametrosDto
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public Guid? VendedorId { get; set; }

        public InvoiceStatus? Status { get; set; }
    }
}
