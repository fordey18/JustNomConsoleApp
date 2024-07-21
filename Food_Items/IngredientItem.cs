namespace JustNomConsoleApp;
public class IngredientItem
{
    public List<Ingredient> Ingredients { get; private set; }

    public IngredientItem()
    {
        Ingredients = new List<Ingredient>();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        Ingredients.Add(ingredient);
    }

}
