using Dapper;
using Domain.DTOs;
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

        public async Task<FundoResponse?> BuscarPorCodigoAsync(string codigo)
        {
            using var conn = CriarConexao();
            return await conn.QueryFirstOrDefaultAsync<FundoResponse>(
                "SELECT F.CODIGO, F.NOME, F.CNPJ, " +
                "F.CODIGO_TIPO AS CodigoTipo, " +
                "ROUND(CAST(F.PATRIMONIO AS REAL), 2) AS PATRIMONIO, " +
                "T.NOME AS NomeTipo " +
                "FROM FUNDO F " +
                "LEFT JOIN TIPO_FUNDO T ON T.CODIGO = F.CODIGO_TIPO " +
                "WHERE F.CODIGO = @Codigo", new { Codigo = codigo });
        }

        public async Task<IEnumerable<FundoResponse>> BuscarTodosAsync()
        {
            using var conn = CriarConexao();
            return await conn.QueryAsync<FundoResponse>(
                "SELECT F.CODIGO, F.NOME, F.CNPJ, F.CODIGO_TIPO AS CodigoTipo, " +
                "ROUND(CAST(F.PATRIMONIO AS REAL), 2) AS PATRIMONIO, " +
                "T.NOME AS NomeTipo " +
                "FROM FUNDO F " +
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
