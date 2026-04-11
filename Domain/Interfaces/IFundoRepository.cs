using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFundoRepository
    {
        Task<IEnumerable<Fundo>> BuscarTodosAsync();
        Task<Fundo?> BuscarPorCodigoAsync(string codigo);
        Task CriarFundoAsync(Fundo fundo);
        Task AtualizarFundoAsync(Fundo fundo);
        Task DeletarFundoAsync(string codigo);
        Task MovimentarPatrimonioAsync(string codigo, decimal valor);
    }
}
