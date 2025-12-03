namespace TIWIKOM.WebApp.Models;

public class UserViewModel
{
    public string Id { get; set; } = "";
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public List<string> Roles { get; set; } = new();
}

public class ManageRolesViewModel
{
    public string UserId { get; set; } = "";
    public string UserEmail { get; set; } = "";
    public List<RoleSelection> UserRoles { get; set; } = new();
}

public class RoleSelection
{
    public string RoleName { get; set; } = "";
    public bool IsSelected { get; set; }
}
