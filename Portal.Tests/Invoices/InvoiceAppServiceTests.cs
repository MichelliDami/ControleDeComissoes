using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Portal.Application.Invoices.DTOs;
using Portal.Application.Invoices.Services;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using Xunit;

namespace Portal.Tests.Invoices
{
    public class InvoiceAppServiceTests
    {
        private readonly Mock<IInvoiceRepository> _invoiceRepo = new();
        private readonly Mock<IVendedorRepository> _vendedorRepo = new();
        private readonly Mock<IComissaoRepository> _comissaoRepo = new();
        private readonly Mock<IValidator<Invoice>> _validator = new();

        private InvoiceAppService CreateSut()
         => new InvoiceAppService(
         _validator.Object,
         _invoiceRepo.Object,
         _vendedorRepo.Object,
         _comissaoRepo.Object
         );

        private static ValidationResult Valid() => new ValidationResult();
        private static ValidationResult Invalid(string msg) => new ValidationResult(new[] { new ValidationFailure("X", msg) });

        [Fact]
        public async Task CriarAsync_Deve_falhar_quando_vendedor_nao_encontrado()
        {
            // Arrange
            _vendedorRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                         .ReturnsAsync((Vendedor?)null);

            var sut = CreateSut();

            var dto = new CriarInvoiceDto
            {
                VendedorId = Guid.NewGuid(),
                DataEmissao = DateTime.Today,
                ClienteNome = "Cliente A",
                ClienteDocumento = "12345678901",
                ValorTotal = 100m,
                Observacoes = "teste"
            };

            // Act
            var result = await sut.CriarAsync(dto);

            // Assert (ajuste nomes se seu ServiceResult for diferente)
            result.Sucesso.Should().BeFalse();
            result.Erros.Should().Contain(e => e.Contains("Vendedor não encontrado", StringComparison.OrdinalIgnoreCase));

            _invoiceRepo.Verify(r => r.AddAsync(It.IsAny<Invoice>()), Times.Never);
            _comissaoRepo.Verify(r => r.AddAsync(It.IsAny<Comissao>()), Times.Never);
        }

        [Fact]
        public async Task CriarAsync_Deve_falhar_quando_vendedor_inativo()
        {
            // Arrange
            var vendedor = CriarVendedorFake(ativo: false);

            _vendedorRepo.Setup(r => r.GetByIdAsync(vendedor.Id))
                         .ReturnsAsync(vendedor);

            var sut = CreateSut();

            var dto = new CriarInvoiceDto
            {
                VendedorId = vendedor.Id,
                DataEmissao = DateTime.Today,
                ClienteNome = "Cliente A",
                ClienteDocumento = "12345678901",
                ValorTotal = 100m,
                Observacoes = "teste"
            };

            // Act
            var result = await sut.CriarAsync(dto);

            // Assert
            result.Sucesso.Should().BeFalse();
            result.Erros.Should().Contain(e => e.Contains("inativo", StringComparison.OrdinalIgnoreCase));

            _invoiceRepo.Verify(r => r.AddAsync(It.IsAny<Invoice>()), Times.Never);
            _comissaoRepo.Verify(r => r.AddAsync(It.IsAny<Comissao>()), Times.Never);
        }

        [Fact]
        public async Task CriarAsync_Deve_criar_invoice_e_comissao_quando_dados_validos()
        {
            // Arrange
            var vendedor = CriarVendedorFake(ativo: true, percentual: 5.5m);

            _vendedorRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync(vendedor);

            _validator.Setup(v => v.ValidateAsync(It.IsAny<Invoice>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(Valid());

            Invoice? invoiceCriada = null;
            Comissao? comissaoCriada = null;

            _invoiceRepo.Setup(r => r.AddAsync(It.IsAny<Invoice>()))
                        .Callback<Invoice>(i => invoiceCriada = i)
                        .Returns(Task.CompletedTask);

            _comissaoRepo.Setup(r => r.AddAsync(It.IsAny<Comissao>()))
                         .Callback<Comissao>(c => comissaoCriada = c)
                         .Returns(Task.CompletedTask);

            var sut = CreateSut();

            var dto = new CriarInvoiceDto
            {
                VendedorId = vendedor.Id,
                DataEmissao = DateTime.Today,
                ClienteNome = "Cliente A",
                ClienteDocumento = "12345678901",
                ValorTotal = 500m,
                Observacoes = "teste"
            };

            // Act
            var result = await sut.CriarAsync(dto);

            // Assert
            result.Sucesso.Should().BeTrue();
            _invoiceRepo.Verify(r => r.AddAsync(It.IsAny<Invoice>()), Times.Once);
            _comissaoRepo.Verify(r => r.AddAsync(It.IsAny<Comissao>()), Times.Once);

            invoiceCriada.Should().NotBeNull();
            comissaoCriada.Should().NotBeNull();

            invoiceCriada!.Id.Should().NotBe(Guid.Empty);  
            comissaoCriada!.InvoiceId.Should().Be(invoiceCriada.Id);
            comissaoCriada.VendedorId.Should().Be(vendedor.Id);
            comissaoCriada.ValorBase.Should().Be(500m);
            comissaoCriada.PercentualAplicado.Should().Be(5.5m);
            comissaoCriada.ValorComissao.Should().Be(27.50m);
            comissaoCriada.Status.Should().Be(ComissaoStatus.Pendente);
        }

        [Fact]
        public async Task AtualizarAsync_Deve_falhar_se_invoice_aprovada_e_tentar_trocar_vendedor()
        {
            // Arrange
            var vendedorAtual = CriarVendedorFake(ativo: true);
            var vendedorNovo = CriarVendedorFake(ativo: true);

            var invoice = CriarInvoiceFake(vendedorAtual.Id);
            SetPrivate(invoice, "Status", InvoiceStatus.Aprovada); 

            _invoiceRepo.Setup(r => r.GetByIdAsync(invoice.Id)).ReturnsAsync(invoice);

            var sut = CreateSut();

            var dto = new AtualizarInvoiceDto
            {
                DataEmissao = DateTime.Today,
                VendedorId = vendedorNovo.Id, 
                ClienteNome = "Cliente X",
                ClienteDocumento = "12345678901",
                ValorTotal = 100m,
                Status = InvoiceStatus.Aprovada,
                Observacoes = "teste"
            };

            // Act
            var result = await sut.AtualizarAsync(invoice.Id, dto);

            // Assert
            result.Sucesso.Should().BeFalse();
            result.Erros.Should().Contain(e => e.Contains("alterar o vendedor", StringComparison.OrdinalIgnoreCase));

            _invoiceRepo.Verify(r => r.UpdateAsync(It.IsAny<Invoice>()), Times.Never);
            _comissaoRepo.Verify(r => r.UpdateAsync(It.IsAny<Comissao>()), Times.Never);
        }

        // ---------------- helpers ----------------

        private static Vendedor CriarVendedorFake(bool ativo, decimal percentual = 5m)
        {
            // Ajuste o construtor conforme o seu Vendedor real.
            // O importante é: Id != Guid.Empty, Ativo = ativo, PercentualComissao = percentual.
            var v = (Vendedor)Activator.CreateInstance(typeof(Vendedor), nonPublic: true)!;

            SetPrivate(v, "Id", Guid.NewGuid());
            SetPrivate(v, "Ativo", ativo);
            SetPrivate(v, "PercentualComissao", percentual);
            SetPrivate(v, "Nome", "Vendedor Teste");
            SetPrivate(v, "Cpf", Guid.NewGuid().ToString("N")[..11]);
            SetPrivate(v, "Email", $"{Guid.NewGuid():N}@teste.com");
            SetPrivate(v, "Telefone", "48999999999");
            SetPrivate(v, "DataCadastro", DateTime.Now);

            return v;
        }

        private static Invoice CriarInvoiceFake(Guid vendedorId)
        {
            // Ajuste o construtor conforme o seu Invoice real.
            // Eu vou criar via reflection para não depender do construtor público.
            var i = (Invoice)Activator.CreateInstance(typeof(Invoice), nonPublic: true)!;

            SetPrivate(i, "Id", Guid.NewGuid());
            SetPrivate(i, "VendedorId", vendedorId);
            SetPrivate(i, "DataEmissao", DateTime.Today);
            SetPrivate(i, "ClienteNome", "Cliente");
            SetPrivate(i, "ClienteDocumento", "12345678901");
            SetPrivate(i, "ValorTotal", 100m);
            SetPrivate(i, "Status", InvoiceStatus.Pendente);
            SetPrivate(i, "Observacoes", "teste");

            return i;
        }

        private static void SetPrivate(object obj, string propOrFieldName, object? value)
        {
            var t = obj.GetType();

            var prop = t.GetProperty(propOrFieldName);
            if (prop != null)
            {
                prop.SetValue(obj, value);
                return;
            }

            var field = t.GetField($"<{propOrFieldName}>k__BackingField",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (field != null)
            {
                field.SetValue(obj, value);
                return;
            }

            // fallback: procurar field direto
            field = t.GetField(propOrFieldName,
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);

            if (field != null)
            {
                field.SetValue(obj, value);
                return;
            }

            throw new InvalidOperationException($"Não achei prop/field '{propOrFieldName}' em {t.Name}.");
        }
    }
}