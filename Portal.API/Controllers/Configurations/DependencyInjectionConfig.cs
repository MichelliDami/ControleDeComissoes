using Portal.Application.Invoices.Services;
using Portal.Application.Vendedores.Services;
using Portal.Domain.Interfaces;
using Portal.Infra.Data.Repository.Repository;

namespace Portal.WebApi.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IVendedorRepository, VendedorRepository>();
            services.AddScoped<IComissaoRepository, ComissaoRepository>();

          
            services.AddScoped<IInvoiceAppService, InvoiceAppService>();
            services.AddScoped<IVendedorAppService, VendedorAppService>();

            return services;
        }
    }
}