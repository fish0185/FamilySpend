namespace FamilySpend.Infra.Entities;

public class Order : BaseEntity
{
    public int Id { get; set; }
    public string ItemDescription { get; set; }
    public string MerchantName { get; set; }
}