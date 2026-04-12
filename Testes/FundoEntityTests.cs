using Domain.Entities;
using FluentAssertions;

namespace Testes
{
    public class FundoEntityTests
    {
        [Fact]
        public void Criar_ComDadosValidos_DeveInstanciarCorretamente()
        {
            var fundo = new Fundo("ITAURF123", "ITAU RF", "86727045000188", 1);

            fundo.Codigo.Should().Be("ITAURF123");
            fundo.Nome.Should().Be("ITAU RF");
            fundo.CodigoTipo.Should().Be(1);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Criar_ComNomeVazio_DeveLancarException(string nome)
        {
            var act = () => new Fundo("ITAURF123", nome, "86727045000188", 1);
            act.Should().Throw<ArgumentException>().WithMessage("*Nome*");
        }

        [Fact]
        public void Criar_ComCnpjInvalido_DeveLancarException()
        {
            var act = () => new Fundo("ITAURF123", "ITAU RF", "123", 1);
            act.Should().Throw<ArgumentException>().WithMessage("*CNPJ*");
        }

        [Fact]
        public void Criar_ComCodigoTipoZero_DeveLancarException()
        {
            var act = () => new Fundo("ITAURF123", "ITAU RF", "86727045000188", 0);
            act.Should().Throw<ArgumentException>().WithMessage("*CodigoTipo*");
        }

        [Fact]
        public void Criar_ComCodigoMaiorQue20Caracteres_DeveLancarException()
        {
            var act = () => new Fundo("CODIGOMUITOLONGOQUENAODEVERIA", "ITAU RF", "86727045000188", 1);
            act.Should().Throw<ArgumentException>().WithMessage("*Codigo*");
        }

        [Fact]
        public void Atualizar_ComDadosValidos_DeveAtualizarCampos()
        {
            var fundo = new Fundo("ITAURF123", "ITAU RF", "86727045000188", 1);
            fundo.Atualizar("ITAU RF PLUS", "86727045000188", 2);

            fundo.Nome.Should().Be("ITAU RF PLUS");
            fundo.CodigoTipo.Should().Be(2);
        }

        [Fact]
        public void Atualizar_ComNomeVazio_DeveLancarException()
        {
            var fundo = new Fundo("ITAURF123", "ITAU RF", "86727045000188", 1);
            var act = () => fundo.Atualizar("", "86727045000188", 1);
            act.Should().Throw<ArgumentException>().WithMessage("*Nome*");
        }
    }
}
