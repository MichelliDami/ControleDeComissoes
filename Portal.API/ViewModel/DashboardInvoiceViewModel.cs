namespace Portal.API.ViewModel
{
    public class DashboardInvoiceViewModel
    {
        public int TotalInvoicesCriadas { get; set; }
        public int TotalPendentes { get; set; }
        public int TotalAprovadas { get; set; }
        public int TotalCanceladas { get; set; }
        public decimal ValorTotalInvoicesAprovadas { get; set; }
        public int InvoicesUltimos30Dias { get; set; }
    }
}
