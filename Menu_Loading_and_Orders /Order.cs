namespace JustNomConsoleApp;

public class Order 
{
    public List<FoodItem> Items { get; private set;}
    
    public string CustomerName { get; set; }
    public string DeliveryAddress { get; set;}
    

    public Order() 
    {
        Items = new List<FoodItem>();
    }

    public void AddItem(FoodItem item) 
    {
        Items.Add(item);
    }

     public void RemoveItem(FoodItem item)
    {
        Items.Remove(item);
    }

    public decimal Total 
    {
        get 
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.Price;
            }
            return total;
        }
    }

    public void SaveOrderToFile(string filePath)
    {
        using (StreamWriter sw = File.CreateText(filePath))
        {
            sw.WriteLine("Order Details:");
            foreach (var item in Items)
            {
                sw.WriteLine($"{item.Name} - £{item.Price:F2}");  
            }
            sw.WriteLine($"Total: £{Total:F2}");
            sw.WriteLine($"Name for the Order is : {CustomerName}");
            sw.WriteLine($"Delivery Order, Address is: {DeliveryAddress}");
            
        }
    }
}