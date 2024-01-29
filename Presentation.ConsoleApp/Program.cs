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
    services.AddDbContext<ProductDataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\skola\VisualStudio\EFC\Shared\Data\ProductCatalog.mdf;Integrated Security=True"));
    services.AddScoped<AddressRepository>();
    services.AddScoped<PhoneNumberRepository>();
    services.AddScoped<ProfileRepository>();
    services.AddScoped<RoleRepository>();
    services.AddScoped<UserRepository>();
    services.AddScoped<CategoryRepository>();
    services.AddScoped<ManufacturerRepository>();
    services.AddScoped<PriceRepository>();
    services.AddScoped<ProductionInformationRepository>();
    services.AddScoped<ProductRepository>();
    services.AddSingleton<UserService>();
    services.AddSingleton<ProductService>();

}).Build();

using (var scope = builder.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    while (true)
    {
        Console.WriteLine("Choose which service to manage:");
        Console.WriteLine("1. User Service");
        Console.WriteLine("2. Product Service");
        Console.WriteLine("0. Exit");

        Console.Write("Choose an action (0-2): ");
        var serviceChoice = Console.ReadLine();

        switch (serviceChoice)
        {
            case "1":
                await ManageUserService(services);
                break;

            case "2":
                await ManageProductService(services);
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

async Task ManageUserService(IServiceProvider services)
{
    var userService = services.GetRequiredService<UserService>();

    while (true)
    {
        Console.WriteLine("User Service Menu:");
        Console.WriteLine("1. Add a user");
        Console.WriteLine("2. Update a user");
        Console.WriteLine("3. Show all users");
        Console.WriteLine("4. Show user information");
        Console.WriteLine("5. Delete a user by email");
        Console.WriteLine("0. Go back to menu");

        Console.Write("Choose an action (0-5): ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                await ProgramUserService.AddUser(userService);
                break;

            case "2":
                await ProgramUserService.UpdateUser(userService);
                break;

            case "3":
                await ProgramUserService.ShowAllUsers(userService);
                break;

            case "4":
                await ProgramUserService.ShowOneUser(userService);
                break;

            case "5":
                await ProgramUserService.DeleteUserFromdb(userService);
                break;

            case "0":
                return; 

            default:
                Console.WriteLine("Invalid choice. Try again.");
                break;
        }
    }
}

async Task ManageProductService(IServiceProvider services)
{
    var productService = services.GetRequiredService<ProductService>();

    while (true)
    {
        Console.WriteLine("Product Service Menu:");
        Console.WriteLine("1. Add a product");
        Console.WriteLine("2. Update a product");
        Console.WriteLine("3. Show all products");
        Console.WriteLine("4. Show product information");
        Console.WriteLine("5. Delete a product by name");
        Console.WriteLine("0. Go back to menu");

        Console.Write("Choose an action (0-5): ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                await ProgramProductService.AddProduct(productService);
                break;

            case "2":
                await ProgramProductService.UpdateProduct(productService);
                break;

            case "3":
                await ProgramProductService.ShowAllProducts(productService);
                break;

            case "4":
                await ProgramProductService.ShowOneProduct(productService);
                break;

            case "5":
                await ProgramProductService.DeleteProductFromDb(productService);
                break;

            case "0":
                return; 

            default:
                Console.WriteLine("Invalid choice. Try again.");
                break;
        }
    }
}