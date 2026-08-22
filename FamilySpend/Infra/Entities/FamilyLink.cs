using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace FamilySpend.Infra.Entities;

public class FamilyLink : BaseEntity
{
    public string UserId { get; set; }
    public string FamilyUserId { get; set; }
}