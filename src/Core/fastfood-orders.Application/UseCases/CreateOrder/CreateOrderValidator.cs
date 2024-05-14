using FluentValidation;

namespace fastfood_orders.Application.UseCases.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(dto => dto.OrderedItems)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("OBE005")
            .ForEach(item => item.SetValidator(new OrderItensValidator()));

        RuleFor(dto => dto.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("OBE008")
            .Must(BeValidCpf)
            .WithMessage("OBE009");
    }

    private bool BeValidCpf(string cpf)
    {
        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf;
        string digito;
        int soma;
        int resto;
        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");
        if (cpf.Length != 11)
            return false;
        tempCpf = cpf.Substring(0, 9);
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito = digito + resto.ToString();

        return cpf.EndsWith(digito);
    }
}

public class OrderItensValidator : AbstractValidator<OrderItens>
{
    public OrderItensValidator()
    {
        RuleFor(dto => dto.ProductId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("OBE006");

        RuleFor(dto => dto.Quantity)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("OBE007");
    }
}