namespace JustNomConsoleApp;

public class Topping : FoodItem
{
    private string _name;
    private decimal _price;

    public Topping(string name, decimal price) : base(name, price)
    {
        _name = name;
        _price = price;

    }
}