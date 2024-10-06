using FluentValidation;

namespace PriceWatch.Domain.Validators;

public static class FluentValidationExtensions
{
  public static IRuleBuilderOptions<T, string> DisplayName<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Products.DisplayName.MaximumLength);
  }
}
