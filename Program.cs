namespace JustNomConsoleApp;

class Program
    {
        //Loads, checks the filepath where the restaurant data is located.
        static void Main(string[] args)
        {
            string DirectoryPath = @"/Users/Archie_Forde/Documents/JustNomConsoleApp/Restaurant Data";
            MenuLoader loader = new MenuLoader(DirectoryPath);
            List<Restaurant> restaurants = loader.LoadRestaurants();

            if (restaurants.Count == 0)
            {
                Console.WriteLine("No restaurants loaded. Exiting.");
                return;
            }

            // Displays the restaurants that are available and lets you select one.

            Console.WriteLine("Available Restaurants:");
            for (int i = 0; i < restaurants.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {restaurants[i].Name}");
            }

            Console.WriteLine("\nChoose a restaurant by number:");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= restaurants.Count)
            {
                Restaurant selectedRestaurant = restaurants[choice - 1];
                Console.WriteLine($"\nSelected Restaurant: {selectedRestaurant.Name}");
                Order order = new Order();

                // once selected a new order is created and the functions for the name and if it is delivery is called.
                // Then the DisplayMenu function is called on the selected restaurant to display the contents.

                AskForName(order);
                AskForDelivery(order);

                DisplayMenu(selectedRestaurant);
            
            while (true)
            {
                    
                string input = Console.ReadLine().ToLower();

                if (input == "done")
                    {
                        break;
                    }
            else if (input == "order")
                {
                    DisplayOrder(order);
                    continue;
                }    
            else if (input.ToLower() == "remove")
                {
                    RemoveItemFromOrder(order);
                    continue;       
                }
            else if (input.ToLower() == "address")
                {
                    AskForDelivery(order);       
                }
            else if (input.ToLower() == "name")
                {
                    AskForName(order);
                }
            else if (input.ToLower() == "exit")
                {
                    break;
                }
            else if (input.ToLower() == "menu")
                {
                    DisplayMenu(selectedRestaurant);
                }
            else if (int.TryParse(input, out int itemChoice) && itemChoice > 0 && itemChoice <= selectedRestaurant.MenuItems.Count)
                {
                    FoodItem selectedItem = selectedRestaurant.MenuItems[itemChoice - 1];
                    order.AddItem(selectedItem);
                    Console.WriteLine($"Added {selectedItem.Name} - £{selectedItem.Price:F2} to your order.");
                }
            else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }

            }
                
            DisplayOrder(order);
                   
        }
        else
        {
            Console.WriteLine("Invalid choice. Exiting the Application");
        }
    }
        

    static void AskForName(Order order) 
    {
        bool checkCustomerName = false;

        while (!checkCustomerName)
        {
            Console.WriteLine("\nPlease Enter the Name for the order.");
            string customerName = Console.ReadLine();

            checkCustomerName = true; 

            for (int i = 0; i < customerName.Length; i++)
                {
                    if (!((customerName[i] >= 'a' && customerName[i] <= 'z') || (customerName[i] >= 'A' && customerName[i] <= 'Z') || customerName[i] == ' '))
                    {
                        checkCustomerName = false;
                        break;
                    }
                }

            if (!checkCustomerName)
                {
                    Console.WriteLine("Enter a valid name containing only letters A-Z.");
                }
            else
                {
                    order.CustomerName = customerName; 
                }
        }
    }

        static void AskForDelivery(Order order)
        {
                Console.WriteLine("\nIs this a delivery Order: Yes/No");
                string DeliveryInput = Console.ReadLine().ToLower();
                if (DeliveryInput == "yes") 
                {
        
                    Console.WriteLine("\nPlease enter the delivery address:");
                    order.DeliveryAddress = Console.ReadLine();
                }
        }

        static void DisplayMenu(Restaurant restaurant )
        {
            Console.WriteLine($"Menu for {restaurant.Name}:\n");
    
            for (int i = 0; i < restaurant.MenuItems.Count; i++)
            {
                FoodItem item = restaurant.MenuItems[i];
                Console.WriteLine($"{i + 1}. {item.Name} - £{item.Price:F2}");
            }
            
                
        Console.WriteLine("\nChoose an item by number to add.\nType 'Done' to finish selecting your items.\nType 'Order' to view your current order.\nType 'Remove' to remove an item.\nType 'address' to change the delivery address.\nType 'Name' to change the name for the order.\nType 'Menu' to allow selection of items again.\nType 'Exit' to exit the application.\n");
    }

        // Remove item function holds the code to remove items from the current order.
        static void RemoveItemFromOrder(Order order)
        {
            Console.WriteLine("\nCurrent Order:\n");
            
            for (int i = 0; i < order.Items.Count; i++)
            {
                FoodItem item = order.Items[i];
                Console.WriteLine($"{i + 1}. {item.Name} - £{item.Price:F2}");
            }
            Console.WriteLine("\nChoose an item by number to remove.");

            if (int.TryParse(Console.ReadLine(), out int itemChoice) && itemChoice > 0 && itemChoice <= order.Items.Count)
            {
                FoodItem itemToRemove = order.Items[itemChoice - 1];
                order.RemoveItem(itemToRemove);
                Console.WriteLine($"Removed {itemToRemove.Name} - £{itemToRemove.Price:F2} from your order.");
            }
            else
            {
                Console.WriteLine("That choice was invalid. Please pick again.");
            }

        }

        // Display order function holds all the code to display whatever the current order is the user has selected and saved.

        static void DisplayOrder(Order order) 
        {
            string orderFilePath = "Orders.txt";

            Console.WriteLine("Would you like to save the order to a file? (yes/no)");
            string saveOrder = Console.ReadLine().ToLower();
            if (saveOrder == "yes")
            {
                order.SaveOrderToFile(orderFilePath);
                Console.WriteLine("Order saved successfully.");
            }

            Console.WriteLine("\nCurrent Order:\n");
            
            for (int i = 0; i < order.Items.Count; i++) 
            {
                FoodItem item = order.Items[i];
                Console.WriteLine($"{i + 1}. {item.Name} - £{item.Price:F2}");
                if (item is Pizza pizza) 
                {
                    foreach (var topping in pizza.Toppings) 
                    {
                        Console.WriteLine($" + {topping}");
                    }
                }
                else if (item is Burger burger) 
                {
                    foreach (var garnish in burger.Garnishes)
                    {
                        Console.WriteLine($" + {garnish}");
                    }
                }
                
            
            }
            // Check if delivery address is in a true state and apply delivery charge as long as it is less than £20.
            if (!string.IsNullOrEmpty(order.DeliveryAddress))
            {
                decimal deliveryCharge = 2.00m;
                decimal totalWithDelivery = order.Total + deliveryCharge;
                Console.WriteLine($"\nName for the order: {order.CustomerName}");
                Console.WriteLine($"\nDelivery Address: {order.DeliveryAddress}\n");

                if (order.Total <= 20.00m)
                {
                    Console.WriteLine($"Delivery Charge: £{deliveryCharge:F2}\n");
                    Console.WriteLine($"Total (including delivery): £{totalWithDelivery:F2}\n");
                }
                else
                {
                    Console.WriteLine("Since your total exceeds £20.00 you qualify for free delivery!.");
                    Console.WriteLine("Delivery Charge: £0.00");
                    Console.WriteLine($"Total: £{order.Total:F2}");
                }
                
            }
        }
    }
    

    