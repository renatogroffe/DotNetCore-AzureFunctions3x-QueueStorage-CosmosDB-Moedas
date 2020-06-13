using FluentValidation;
using ProcessamentoCotacoes.Models;

namespace ProcessamentoCotacoes.Validators
{
    public class CotacaoValidator : AbstractValidator<Cotacao>
    {
        public CotacaoValidator()
        {
            RuleFor(c => c.Codigo).NotEmpty().WithMessage("Preencha o campo 'Codigo'");

            RuleFor(c => c.Valor).NotEmpty().WithMessage("Preencha o campo 'Valor'")
                .GreaterThan(0).WithMessage("O campo 'Valor' deve ser maior do 0");
        }
    }
}