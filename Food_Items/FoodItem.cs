namespace JustNomConsoleApp;

public abstract class FoodItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    protected FoodItem(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}