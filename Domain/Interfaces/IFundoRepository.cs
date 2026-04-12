using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFundoRepository
    {
        Task<IEnumerable<FundoResponse>> BuscarTodosAsync();
        Task<FundoResponse?> BuscarPorCodigoAsync(string codigo);
        Task CriarFundoAsync(Fundo fundo);
        Task AtualizarFundoAsync(Fundo fundo);
        Task DeletarFundoAsync(string codigo);
        Task MovimentarPatrimonioAsync(string codigo, decimal valor);
    }
}
