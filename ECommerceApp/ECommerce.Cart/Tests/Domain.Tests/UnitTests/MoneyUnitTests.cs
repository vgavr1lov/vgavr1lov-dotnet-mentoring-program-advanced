using ECommerce.Cart.Domain.ValueObjects;

namespace ECommerce.Cart.Domain.Tests.UnitTests;

public class MoneyUnitTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenAmountIsNegative()
    {
        Assert.Throws<ArgumentException>(() => new Money(
            amount: -2m,
            currency: "PLN"));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenCurrencyIsBlank()
    {
        Assert.Throws<ArgumentException>(() => new Money(
            amount: 2m,
            currency: ""));
    }

    [Fact]
    public void Constructor_ShouldCreate_WhenValidAmountAndCurrencyAreProvided()
    {
        var amount = 10m;

        var money = new Money(
            amount: amount,
            currency: "PLN");

        Assert.Equal(amount, money.Amount);
    }
}
