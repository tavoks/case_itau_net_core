using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Data.Sqlite;

namespace Infra.Repositories
{
    public class FundoRepository : IFundoRepository
    {
        private readonly string _con;
        public FundoRepository(string connectionString)
        {
            _con = connectionString;
        }

        private SqliteConnection CriarConexao() => new SqliteConnection(_con);
        public async Task AtualizarFundoAsync(Fundo fundo)
        {
            using var conn = CriarConexao();
            await conn.ExecuteAsync(
                "UPDATE FUNDO SET NOME = @Nome, CNPJ = @Cnpj, CODIGO_TIPO = @CodigoTipo " +
                "WHERE CODIGO = @Codigo", fundo);
        }

        public async Task<Fundo?> BuscarPorCodigoAsync(string codigo)
        {
            using var conn = CriarConexao();
            return await conn.QueryFirstOrDefaultAsync<Fundo>(
                "SELECT F.*, T.NOME AS NOME_TIPO FROM FUNDO F " +
                "LEFT JOIN TIPO_FUNDO T ON T.CODIGO = F.CODIGO_TIPO " +
                "WHERE F.CODIGO = @Codigo", new { Codigo = codigo });
        }

        public async Task<IEnumerable<Fundo>> BuscarTodosAsync()
        {
            using var conn = CriarConexao();
            return await conn.QueryAsync<Fundo>("SELECT F.*, T.NOME AS NOME_TIPO FROM FUNDO F " +
                "LEFT JOIN TIPO_FUNDO T ON T.CODIGO = F.CODIGO_TIPO");
        }

        public async Task CriarFundoAsync(Fundo fundo)
        {
            using var conn = CriarConexao();
            await conn.ExecuteAsync(
                "INSERT INTO FUNDO (CODIGO, NOME, CNPJ, CODIGO_TIPO, PATRIMONIO) " +
                "VALUES (@Codigo, @Nome, @Cnpj, @CodigoTipo, NULL)", fundo);
        }

        public async Task DeletarFundoAsync(string codigo)
        {
            using var conn = CriarConexao();
            await conn.ExecuteAsync(
                "DELETE FROM FUNDO WHERE CODIGO = @Codigo", new { Codigo = codigo });
        }

        public async Task MovimentarPatrimonioAsync(string codigo, decimal valor)
        {
            using var conn = CriarConexao();
            await conn.ExecuteAsync(
                "UPDATE FUNDO SET PATRIMONIO = IFNULL(PATRIMONIO, 0) + @Valor " +
                "WHERE CODIGO = @Codigo", new { Codigo = codigo, Valor = valor });
        }
    }
}
