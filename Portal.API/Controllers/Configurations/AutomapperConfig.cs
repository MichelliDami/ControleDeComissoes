using AutoMapper;
using Portal.API.ViewModel;
using Portal.Application.Invoices.DTOs;
using Portal.Application.Vendedores.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Portal.API.Controllers.Configurations
{
   
        public class AutoMapperConfig : Profile
        {
            public AutoMapperConfig()
            {
            CreateMap<VendedorResponseDto, VendedorViewModel>();
            CreateMap<VendedorViewModel, AtualizarVendedorDto>();
            CreateMap<VendedorViewModel, CadastrarVendedorDto>();


            CreateMap<InvoiceViewModel, CriarInvoiceDto>();
            CreateMap<InvoiceViewModel, AtualizarInvoiceDto>();
            CreateMap<InvoiceResponseDto, InvoiceViewModel>();
            CreateMap<DashboardFiltroViewModel, DashboardFiltroDto>();
            CreateMap<InvoiceIndicadoresDto, DashboardInvoiceViewModel>();
        }
        }
    
}
