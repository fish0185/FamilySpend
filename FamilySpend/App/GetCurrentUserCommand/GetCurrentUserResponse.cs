namespace FamilySpend.App.GetCurrentUserCommand;

public class GetCurrentUserResponse
{
    public string Email { get; set; }
    public decimal Balance { get; set; }
    public bool IsPrimaryAccount { get; set; }
    public IEnumerable<SubAccountResponse> SubAccounts { get; set; }
}

public class SubAccountResponse
{
    public string Email { get; set; }
    public decimal Balance { get; set; }
}