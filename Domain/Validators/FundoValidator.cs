using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class FundoValidator :AbstractValidator<Fundo>
    {
        public FundoValidator()
        {
            RuleFor(p => p.Codigo)
                .NotNull().WithMessage("Codigo não pode ser nulo.")
                .NotEmpty().WithMessage("Codigo não pode estar vazio.")
                .MaximumLength(20).WithMessage("Codigo deve ter no máximo 20 caracteres.");

            RuleFor(p => p.Nome)
                .NotNull().WithMessage("Nome não pode ser nulo.")
                .NotEmpty().WithMessage("Nome não pode estar vazio.")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");

            RuleFor(p => p.Cnpj)
                .NotNull().WithMessage("Cnpj não pode ser nulo.")
                .NotEmpty().WithMessage("Cnpj não pode estar vazio.")
                .Length(14).WithMessage("CNPJ deve ter exatamente 14 dígitos.");

            RuleFor(p => p.CodigoTipo)
                .NotNull().WithMessage("CodigoTipo não pode ser nulo.")
                .GreaterThan(0).WithMessage("CodigoTipo deve ser maior que 0.");
        }
    }
}
