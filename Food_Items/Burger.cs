namespace JustNomConsoleApp;

public class Burger : FoodItem
{
    public List<Garnish> Garnishes { get; set; }

    public Burger(string name, decimal price) : base(name, price)
    {
        Garnishes = new List<Garnish>();
    }
}
