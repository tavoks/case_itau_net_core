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
                .NotEmpty().WithMessage("Codigo não pode estar vazio.");

            RuleFor(p => p.Nome)
                .NotNull().WithMessage("Nome não pode ser nulo.")
                .NotEmpty().WithMessage("Nome não pode estar vazio.");

            RuleFor(p => p.Cnpj)
                .NotNull().WithMessage("Cnpj não pode ser nulo.")
                .NotEmpty().WithMessage("Cnpj não pode estar vazio.");

            RuleFor(p => p.CodigoTipo)
                .NotNull().WithMessage("CodigoTipo não pode ser nulo.")
                .GreaterThan(0).WithMessage("CodigoTipo deve ser maior que 0.");
        }
    }
}
