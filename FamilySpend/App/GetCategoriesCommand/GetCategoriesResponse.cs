namespace FamilySpend.App.GetCategoriesCommand;

public class GetCategoriesResponse
{
    public IEnumerable<Category> Categories { get; set; }
}

public class Category
{
    public string CategoryName { get; set; }
    public int Id { get; set; }
}