using Domain.Validators;
using FluentValidation;

namespace Domain.Entities
{
    public class Fundo
    {
        public string Codigo { get; private set; } = string.Empty;
        public string Nome { get; private set; } = string.Empty;
        public string Cnpj { get; private set; } = string.Empty;
        public int CodigoTipo { get; private set; }
        public double? Patrimonio { get; private set; }

        protected Fundo() { }

        public Fundo(string codigo, string nome, string cnpj, int codigoTipo)
        {
            Codigo = codigo;
            Nome = nome;
            Cnpj = cnpj;
            CodigoTipo = codigoTipo;

            Validar(new FundoValidator(), this);
        }

        protected static void Validar<FundoValidator, Fundo>(FundoValidator validator, Fundo entity) where FundoValidator : AbstractValidator<Fundo>
        {
            var validationResult = validator.Validate(entity);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.ToString(", "));
            }
        }

        public void Atualizar(string nome, string cnpj, int codigoTipo)
        {
            Nome = nome;
            Cnpj = cnpj;
            CodigoTipo = codigoTipo;
            Validar(new FundoValidator(), this);
        }
    }
}
