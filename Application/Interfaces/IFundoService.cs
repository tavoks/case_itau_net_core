using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFundoService
    {
        Task<IEnumerable<FundoResponse>> BuscarTodosAsync();
        Task<FundoResponse?> BuscarPorCodigoAsync(string codigo);
        Task<FundoResponse> CriarFundoAsync(CriarFundoRequest criarFundoRequest);
        Task AtualizarFundoAsync(string codigo, AtualizarFundoRequest atualizarFundoRequest);
        Task DeletarFundoAsync(string codigo);
        Task MovimentarPatrimonioAsync(string codigo, decimal valor);
    }
}
