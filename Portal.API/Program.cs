using Portal.Application.Invoices.Services;
using Portal.Application.Vendedores.Services;
using Portal.Domain.Interfaces;
using Portal.Domain.Notifications;
using Portal.Domain.Notifications.Portal.Domain.Notifications;
using Portal.Infra.Data.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using Portal.Infra.Data.Repository.Context;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IInvoiceAppService, InvoiceAppService>();
builder.Services.AddScoped<IVendedorAppService, VendedorAppService>();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IVendedorRepository, VendedorRepository>();
builder.Services.AddScoped<IComissaoRepository, ComissaoRepository>();

builder.Services.AddScoped<INotificador, Notificador>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
