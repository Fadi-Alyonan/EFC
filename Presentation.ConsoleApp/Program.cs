using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.ConsoleApp.Services;
using Shared.Contexts;
using Shared.Repositories;
using Shared.Services;


var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\skola\VisualStudio\EFC\Shared\Data\localhost.mdf;Integrated Security=True;Connect Timeout=30"));
    services.AddScoped<AddressRepository>();
    services.AddScoped<PhoneNumberRepository>();
    services.AddScoped<ProfileRepository>();
    services.AddScoped<RoleRepository>();
    services.AddScoped<UserRepository>();
    services.AddScoped<UserService>();

}).Build();

using (var scope = builder.Services.CreateScope())
{
   
    var services = scope.ServiceProvider;
    var userService = services.GetRequiredService<UserService>();

    

    while (true)
    {
        
        Console.WriteLine("1. Add a user");
        Console.WriteLine("2. Update a user");
        Console.WriteLine("3. Show all users");
        Console.WriteLine("4. Delete a user by email");
        Console.WriteLine("0. Exit");

        Console.Write("Choose an action (0-4): ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                await ProgramService.AddUser(userService);
                break;

            case "2":
                await ProgramService.UpdateUser(userService);
                break;

            case "3":
                await ProgramService.ShowAllUsers(userService);
                break;

            case "4":
                await ProgramService.DeleteUserFromdb(userService);
                break;

            case "0":
                Environment.Exit(0);
                break;

            default:
                Console.WriteLine("Invalid choice. Try again.");
                break;

                
        }
        
    }
}