namespace FamilySpend.App.GetCurrentUserCommand;

public class GetCurrentUserResponse
{
    public string Email { get; set; }
    public bool IsPrimaryAccount { get; set; }
    public string?[] SubAccountEmails { get; set; }
}