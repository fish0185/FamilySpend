namespace FamilySpend.Infra.Entities;

public class UserOrderCategory : BaseEntity
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int OrderCategoryId { get; set; }
}