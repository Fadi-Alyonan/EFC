using Shared.Dtos;
using Shared.Entities;
using Shared.Services;
using System.Diagnostics;

namespace Presentation.ConsoleApp.Services;

internal class ProgramUserService
{
    public static async Task AddUser(UserService userService)
    {
        Console.Clear();
        Console.Write("Enter first name: ");
        var firstName = Console.ReadLine();

        Console.Write("Enter last name: ");
        var lastName = Console.ReadLine();

        Console.Write("Enter street name: ");
        var streetName = Console.ReadLine();

        Console.Write("Enter postal code: ");
        var postalCode = Console.ReadLine();

        Console.Write("Enter city: ");
        var city = Console.ReadLine();

        Console.Write("Enter phone number: ");
        var phoneNumber = Console.ReadLine();

        Console.Write("Enter role name: ");
        var roleName = Console.ReadLine();

        Console.Write("Enter email: ");
        var email = Console.ReadLine();

        Console.Write("Enter password: ");
        var password = Console.ReadLine();

        var userDto = new UserDto
        {
            FirstName = firstName!,
            LastName = lastName!,
            StreetName = streetName,
            PostalCode = postalCode,
            City = city,
            PhoneNumber = phoneNumber,
            RoleName = roleName!,
            Email = email!,
            Password = password!
        };

        if (await userService.CreateUser(userDto))
        {
            Console.WriteLine("User added successfully!");
        }
        else
        {
            Console.WriteLine("Error adding user.");
        }

    }

    public static async Task UpdateUser(UserService userService)
    {
        Console.Clear();
        Console.Write("Enter email to find user for update: ");
        var email = Console.ReadLine();

        var userToUpdate = new UserDto() { Email = email };


        if (await userService.CheckIfUserExistsAsync(email))
        {
            Console.WriteLine($"User found! Enter new details:");

            Console.Write("Enter first name: ");
            userToUpdate.FirstName = Console.ReadLine();

            Console.Write("Enter last name: ");
            userToUpdate.LastName = Console.ReadLine();

            Console.Write("Enter street name: ");
            userToUpdate.StreetName = Console.ReadLine();

            Console.Write("Enter postal code: ");
            userToUpdate.PostalCode = Console.ReadLine();

            Console.Write("Enter city: ");
            userToUpdate.City = Console.ReadLine();

            Console.Write("Enter phone number: ");
            userToUpdate.PhoneNumber = Console.ReadLine();

            Console.Write("Enter new password: ");
            userToUpdate.Password = Console.ReadLine();

            Console.Write("Enter new role: ");
            userToUpdate.RoleName = Console.ReadLine();



            if (await userService.UpdateUser(userToUpdate))
            {
                Console.WriteLine("User updated successfully!");
            }
            else
            {
                Console.WriteLine("Error updating user.");
            }
        }
        else
        {
            Console.WriteLine($"User with email {email} not found.");
        }

    }

    public static async Task ShowAllUsers(UserService userService)
    {
        Console.Clear();
        var users = await userService.GetAllUsers();

        if (users != null)
        {
            foreach (var user in users)
            {
                Console.WriteLine($"Name: {user.FirstName} {user.LastName}, Email: {user.Email}");
            }
        }
        else
        {
            Console.WriteLine("Error fetching users.");
        }
    }

    public static async Task ShowOneUser(UserService userService)
    {
        Console.Clear();
        try
        {
            Console.Write("Enter email to show user information: ");

            var email = Console.ReadLine();


            if (await userService.CheckIfUserExistsAsync(email))
            {

                var userDto = await userService.GetOneUser(new UserEntity { Email = email });

                // Display user information in the console
                Console.WriteLine($"User Information:\n" +
                                  $"-----------------\n" +
                                  $"First Name: {userDto.FirstName}\n" +
                                  $"Last Name: {userDto.LastName}\n" +
                                  $"Phone Number: {userDto.PhoneNumber}\n" +
                                  $"Role Name: {userDto.RoleName}\n" +
                                  $"Email: {userDto.Email}\n" +
                                  $"Street Name: {userDto.StreetName}\n" +
                                  $"Postal Code: {userDto.PostalCode}\n" +
                                  $"City: {userDto.City}\n");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
        }
    }

    public static async Task DeleteUserFromdb(UserService userService)
    {
        Console.Clear();
        Console.Write("Enter email to delete: ");

        var email = Console.ReadLine();

        var userDto = new UserDto { Email = email };

        if (await userService.DeleteUser(userDto))
        {
            Console.WriteLine($"User with email {email} deleted successfully!");
        }
        else
        {
            Console.WriteLine($"Error deleting user with email {email}.");
        }
    }
}
