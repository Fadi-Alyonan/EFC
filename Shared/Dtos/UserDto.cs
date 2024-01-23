namespace Shared.Dtos;

public class UserDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? StreetName { get; set; } 
    public string? PostalCode { get; set; }
    public string? City { get; set; } 
}
