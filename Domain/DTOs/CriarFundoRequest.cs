namespace Domain.DTOs
{
    public record CriarFundoRequest(string Codigo, string Nome, string Cnpj, int CodigoTipo);
}
