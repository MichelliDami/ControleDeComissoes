using Portal.Application.Invoices.Services;
using Portal.Application.Vendedores.Services;
using Portal.Domain.Interfaces;
using Portal.Infra.Data.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using Portal.Infra.Data.Repository.Context;
using FluentValidation;
using Portal.Domain.Models;
using Portal.Application.Invoices.Validation;
using Portal.Application.Vendedores.Validation;

var builder = WebApplication.CreateBuilder(args);

// services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IInvoiceAppService, InvoiceAppService>();
builder.Services.AddScoped<IVendedorAppService, VendedorAppService>();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IVendedorRepository, VendedorRepository>();
builder.Services.AddScoped<IComissaoRepository, ComissaoRepository>();

builder.Services.AddScoped<IValidator<Invoice>, CadastroInvoiceValidator>();
builder.Services.AddScoped<IValidator<Vendedor>, CadastroVendedorValidator>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<PortalDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure()
    )
);

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PortalDbContext>();
    db.Database.Migrate();
}

// pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();