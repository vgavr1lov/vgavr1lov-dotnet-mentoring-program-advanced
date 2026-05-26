namespace ECommerce.Cart.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private const int DefaultCurrencyDecimals = 2;

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative!");

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required!");

        Amount = decimal.Round(amount, DefaultCurrencyDecimals);
        Currency = currency.ToUpper();
    }
}
