using FluentValidation;
using PriceWatch.Domain.Validators;

namespace PriceWatch.Domain.Products;

public record DisplayName
{
  public const int MaximumLength = byte.MaxValue;

  public string Value { get; }

  public DisplayName(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  public override string ToString() => Value;

  private class Validator : AbstractValidator<DisplayName>
  {
    public Validator()
    {
      RuleFor(x => x.Value).DisplayName();
    }
  }
}
