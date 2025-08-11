using Microsoft.AspNetCore.Identity;

namespace Domain.Users;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole() : base() { }
    public ApplicationRole(string roleName) : base(roleName) { }

    // Can add custom properties later if needed
}