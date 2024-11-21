namespace CleanArchitectureDDD.Domain.Entities.Shared;

public record Currency(decimal price, CurrencyType currencyType)
{
    public static Currency operator +(Currency c1, Currency c2)
    {
        if (c1.currencyType != c2.currencyType)
        {
            throw new InvalidOperationException("Cannot add two different currencies");
        }

        return new Currency(c1.price + c2.price, c1.currencyType);
    }
    
    public static Currency Zero() => new(0, CurrencyType.None);
    public static Currency Zero(CurrencyType currencyType) => new(0, currencyType);
    public bool IsZero() => this == Zero(currencyType);
}