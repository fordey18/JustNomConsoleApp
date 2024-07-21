namespace JustNomConsoleApp;

public abstract class Ingredient
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    protected Ingredient(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}