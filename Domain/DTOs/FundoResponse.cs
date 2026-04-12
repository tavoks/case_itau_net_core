namespace Domain.DTOs
{
    public class FundoResponse
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public int CodigoTipo { get; set; }
        public string? NomeTipo { get; set; }
        public decimal? Patrimonio { get; set; }
    }
}