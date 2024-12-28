



using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Models;
public class ApplicationUser : IdentityUser
{
    public string Avatar { get; set; } = default!;
    public int Status { get; set; }
    public string FullName { get; set; } = default!;
}