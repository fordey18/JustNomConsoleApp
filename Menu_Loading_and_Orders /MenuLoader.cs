namespace JustNomConsoleApp;

public class MenuLoader
{
    private readonly string _directoryPath;

    public MenuLoader(string directoryPath)
    {
        _directoryPath = directoryPath;
    }

    public List<Restaurant> LoadRestaurants()
    {
        List<Restaurant> restaurants = new List<Restaurant>();

        string[] files = Directory.GetFiles(_directoryPath);
        foreach (var file in files)
        {
            Restaurant restaurant = ParseRestaurantData(file);

            if (restaurant != null)
            {
                restaurants.Add(restaurant);
            }
        }

        return restaurants;
    }

    private Restaurant ParseRestaurantData(string filePath)
    {
        try
        {
            var lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
            {
                Console.WriteLine($"File '{filePath}' is empty or does not contain valid data.");
                return null;
            }

            string[] nameParts = lines[0].Split(':');
            if (nameParts.Length < 2)
            {
                Console.WriteLine($"Invalid restaurant name format in file '{filePath}'.");
                return null;
            }
        

            string name = nameParts[1];
            if (!HasAlphanumeric(name))
            {
                Console.WriteLine($"Restaurant '{name}' does not have a valid name and was not loaded.");
                return null;
            }

            if (!lines[1].StartsWith("Toppings:"))
            {
                Console.WriteLine($"Restaurant '{name}' does not have a valid structure and was not loaded.");
                return null;
            }

            if (!lines[2].StartsWith("Garnishes:"))
            {
                Console.WriteLine($"Restaurant '{name}' does not have a valid structure and was not loaded.");
                return null;
            }

            


            var foodItems = new List<FoodItem>();
            var toppings = new List<Topping>();
            var garnishes = new List<Garnish>();
            
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (line.StartsWith("Toppings:"))
                {
                    var toppingsList = ParseToppingData(line.Substring("Toppings:".Length).Trim());
                    foreach (var topping in toppingsList)
                    {
                        foodItems.Add(topping);  
                    }
                }

                else if (line.StartsWith("Garnishes:"))
                {
                    var GarnishList = ParseGarnishData(line.Substring("Garnishes:".Length).Trim());
                    foreach (var garnish in GarnishList)
                    {
                        foodItems.Add(garnish);  
                    }
                }

                else if (line.StartsWith("Pizza:"))
                {
                    var pizza = ParsePizzaData(line.Substring("Pizza:".Length).Trim());
                    if (pizza != null)
                    {
                        foodItems.Add(pizza);
                    }
                }
                else if (line.StartsWith("Burger:"))
                {
                    var burger = ParseBurgerData(line.Substring("Burger:".Length).Trim());
                    if (burger != null)
                    {
                        foodItems.Add(burger);
                    }
                }
                
            }

            return new Restaurant(name, foodItems);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading restaurant from file '{filePath}': {ex.Message}");
            return null;
        }
    }
    

    private Pizza ParsePizzaData(string data)
    {
        try
        {
            // Remove the enclosing angle brackets
            if (data.StartsWith("<") && data.EndsWith(">"))
            {
                data = data.Substring(1, data.Length - 2);
            }
            else
            {
                throw new FormatException($"Invalid data format: {data}");
            }

            // Find the index positions for Name, Toppings, and Price.
            int nameIndex = data.IndexOf("Name:");
            int toppingsIndex = data.IndexOf("Toppings:");
            int priceIndex = data.IndexOf("Price:");

            if (nameIndex == -1 || toppingsIndex == -1 || priceIndex == -1)
            {
                throw new FormatException($"Invalid data format: {data}");
            }

            // Extract the substrings for each part of the pizza data.
            string namePart = data.Substring(nameIndex + 5, toppingsIndex - nameIndex - 6).Trim();
            string toppingsPart = data.Substring(toppingsIndex + 9, priceIndex - toppingsIndex - 10).Trim(new char[] { '[', ']', ' ' });
            string pricePart = data.Substring(priceIndex + 6).Trim();

            // Parsing the toppings.
            List<string> toppings = new List<string>(toppingsPart.Split(new string[] { ", " }, StringSplitOptions.None));

            // Parsing the price.
            if (!decimal.TryParse(pricePart, out decimal price))
            {
                throw new FormatException($"Invalid price format: {pricePart}");
            }

            return new Pizza(namePart, price / 100m);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing pizza data: {data}");
            Console.WriteLine($"Exception: {ex.Message}");
            throw;
        }
    }

    private Burger ParseBurgerData(string data)
    {
        try
        {
            // Remove the enclosing angle brackets.
            if (data.StartsWith("<") && data.EndsWith(">"))
            {
                data = data.Substring(1, data.Length - 2);
            }
            else
            {
                throw new FormatException($"Invalid data format: {data}");
            }

            // Find the index positions for Name, Garnishes, and Price.
            int nameIndex = data.IndexOf("Name:");
            int garnishesIndex = data.IndexOf("Garnishes:");
            int priceIndex = data.IndexOf("Price:");

            if (nameIndex == -1 || garnishesIndex == -1 || priceIndex == -1)
            {
                throw new FormatException($"Invalid data format: {data}");
            }

            // Extract the substrings for each part of the burger data. 
            string namePart = data.Substring(nameIndex + 5, garnishesIndex - nameIndex - 6).Trim();
            string garnishesPart = data.Substring(garnishesIndex + 10, priceIndex - garnishesIndex - 11).Trim(new char[] { '[', ']', ' ' });
            string pricePart = data.Substring(priceIndex + 6).Trim();

            // Parsing the garnishes.
            List<string> garnishes = new List<string>(garnishesPart.Split(new string[] { ", " }, StringSplitOptions.None));

            // Parsing the price.
            if (!decimal.TryParse(pricePart, out decimal price))
            {
                throw new FormatException($"Invalid price format: {pricePart}");
            }

            return new Burger(namePart, price / 100m );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing pizza data: {data}");
            Console.WriteLine($"Exception: {ex.Message}");
            throw;
        }
    }


    private List<Topping> ParseToppingData(string data)
    {
        try
        {
            data = data.Trim(new char[] { '[', ']' });  
            var toppings = new List<Topping>();

            
            var entries = data.Split(new[] { ">,<" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(e => e.Trim('<', '>')) 
            .ToList();

            foreach (var entry in entries)
            {
                var parts = entry.Split(',');
                if (parts.Length == 2)
                {
                    string name = parts[0].Trim();
                    if (decimal.TryParse(parts[1].Trim(), out decimal price))
                    {
                        toppings.Add(new Topping(name, price / 100m ));  
                    }
                }
            }
            return toppings;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing the toppings data: {ex.Message}");
            throw;
        }
    }
    private List<Garnish> ParseGarnishData(string data)
    {
        try
        {
            data = data.Trim(new char[] { '[', ']' });  
            var garnishes = new List<Garnish>();

            
            var entries = data.Split(new[] { ">,<" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(e => e.Trim('<', '>')) 
            .ToList();

            foreach (var entry in entries)
            {
                var parts = entry.Split(',');
                if (parts.Length == 2)
                {
                    string name = parts[0].Trim();
                    if (decimal.TryParse(parts[1].Trim(), out decimal price))
                    {
                        garnishes.Add(new Garnish(name, price / 100m ));  
                    }
                }
            }
            return garnishes;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing the garnish data: {ex.Message}");
            throw;
        }
    }
    public static bool HasAlphanumeric(string input)
        {
            return input.Any(char.IsLetterOrDigit);
        }
}

