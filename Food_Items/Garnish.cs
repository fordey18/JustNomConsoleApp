namespace JustNomConsoleApp;

public class Garnish : Ingredient
{
    private string _name;
    private int _price;
    public Garnish(string name, int price): base(name, price)
    {
        _name = name;
        _price = price;
    }
}