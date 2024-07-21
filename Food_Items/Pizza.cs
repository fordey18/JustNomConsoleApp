namespace JustNomConsoleApp;
public class Pizza : FoodItem
{
    public List<Topping> Toppings { get; set; }

    public Pizza(string name, decimal price) : base(name, price)
    {
        Toppings = new List<Topping>();
    }
}
