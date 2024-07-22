namespace JustNomConsoleApp;

public class Garnish : FoodItem
{
    private string _name;
    private decimal _price;
    public Garnish(string name, decimal price): base(name, price)
    {
        _name = name;
        _price = price;
    }
}