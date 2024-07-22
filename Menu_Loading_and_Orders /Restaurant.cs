namespace JustNomConsoleApp;

public class Restaurant 
{
   public string Name { get; set; }
   public List<FoodItem> MenuItems { get; set; }


    public Restaurant(string name, List<FoodItem> menuItems)
    {
        Name = name;
        MenuItems = menuItems;

        
    }
}