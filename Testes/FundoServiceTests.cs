using Application.Interfaces;
using Application.Services;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Testes
{
    public class FundoServiceTests
    {
        private readonly Mock<IFundoRepository> _repoMock;
        private readonly IMemoryCache _cache;
        private readonly IFundoService _service;

        public FundoServiceTests()
        {
            _repoMock = new Mock<IFundoRepository>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _service = new FundoService(_repoMock.Object, _cache);
        }

        [Fact]
        public async Task Buscar_Todos_DeveRetornarLista()
        {
            var fundos = new List<FundoResponse>
            {
                new FundoResponse { Codigo = "ITAURF123", Nome = "ITAU RF", Cnpj = "86727045000188", CodigoTipo = 1 },
                new FundoResponse { Codigo = "ITAUMM999", Nome = "ITAU MM", Cnpj = "11222333444455", CodigoTipo = 2 }
            };

            _repoMock.Setup(r => r.BuscarTodosAsync()).ReturnsAsync(fundos);

            var resultado = await _service.BuscarTodosAsync();
            resultado.Should().HaveCount(2);
        }

        [Fact]
        public async Task BuscarTodos_SegundaChamada_DeveUsarCache()
        {
            var fundos = new List<FundoResponse>
            {
                new FundoResponse { Codigo = "ITAURF123", Nome = "ITAU RF", Cnpj = "86727045000188", CodigoTipo = 1 }
            };
            _repoMock.Setup(r => r.BuscarTodosAsync()).ReturnsAsync(fundos);

            await _service.BuscarTodosAsync();
            await _service.BuscarTodosAsync();

            _repoMock.Verify(r => r.BuscarTodosAsync(), Times.Once());
        }

        [Fact]
        public async Task BuscarPorCodigo_QuandoNaoExiste_DeveRetornarNull()
        {
            _repoMock.Setup(r => r.BuscarPorCodigoAsync(It.IsAny<string>()))
                .ReturnsAsync((FundoResponse?)null);

            var resultado = await _service.BuscarPorCodigoAsync("INEXISTENTE");

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task Criar_ComDadosValidos_DeveSalvar()
        {
            _repoMock.Setup(r => r.CriarFundoAsync(It.IsAny<Fundo>()))
                .Returns(Task.CompletedTask);

            var resultado = await _service.CriarFundoAsync(
               new CriarFundoRequest("ITAURF123", "ITAU RF", "86727045000188", 1));

            resultado.Codigo.Should().Be("ITAURF123");
            _repoMock.Verify(r => r.CriarFundoAsync(It.IsAny<Fundo>()), Times.Once);
        }

        [Fact]
        public async Task Deletar_DeveChamarRepositorioEInvalidarCache()
        {
            _repoMock.Setup(r => r.DeletarFundoAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            await _service.DeletarFundoAsync("ITAURF123");

            _repoMock.Verify(R => R.DeletarFundoAsync("ITAURF123"), Times.Once);
        }

        [Fact]
        public async Task MovimentarPatrimonio_DeveChamarRepositorio()
        {
            _repoMock.Setup(r => r.MovimentarPatrimonioAsync(It.IsAny<string>(), It.IsAny<decimal>()))
                .Returns(Task.CompletedTask);

            await _service.MovimentarPatrimonioAsync("ITAURF123", 1000);

            _repoMock.Verify(r => r.MovimentarPatrimonioAsync("ITAURF123", 1000), Times.Once);
        }
    }
}
