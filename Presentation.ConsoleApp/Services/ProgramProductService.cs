using Shared.Dtos;
using Shared.Entities;
using Shared.Services;
using System.Diagnostics;

namespace Presentation.ConsoleApp.Services;

internal class ProgramProductService
{
    public static async Task AddProduct(ProductService productService)
    {
        Console.Clear();
        Console.Write("Enter product name: ");
        var productName = Console.ReadLine();

        Console.Write("Enter product description: ");
        var productDescription = Console.ReadLine();

        Console.Write("Enter product quantity in stock: ");
        if (!int.TryParse(Console.ReadLine(), out var quantityInStock))
        {
            Console.WriteLine("Invalid quantity input. Please enter a valid integer.");
            return;
        }

        Console.Write("Enter product production date (YYYY-MM-DD): ");
        if (!DateOnly.TryParse(Console.ReadLine(), out var productionDate))
        {
            Console.WriteLine("Invalid date input. Please enter a valid date in YYYYMMDD format.");
            return;
        }

        Console.Write("Enter product category name: ");
        var categoryName = Console.ReadLine();

        Console.Write("Enter product manufacturer name: ");
        var manufacturerName = Console.ReadLine();

        Console.Write("Enter product price: ");
        if (!decimal.TryParse(Console.ReadLine(), out var productPrice))
        {
            Console.WriteLine("Invalid price input. Please enter a valid decimal number.");
            return;
        }

        var productDto = new ProductDto
        {
            ProductName = productName!,
            ProductDescription = productDescription!,
            QuantityInStock = quantityInStock,
            ProductionDate = productionDate,
            CategoryName = categoryName!,
            ManufacturerName = manufacturerName!,
            ProductPrice = productPrice
        };
        
        if (await productService.CreateProduct(productDto))
        {
            Console.Clear();
            Console.WriteLine("Product added successfully!");
        }
        else
        {
            Console.Clear();
            Console.WriteLine("The product already exists.");
        }

    }
    public static async Task UpdateProduct(ProductService productService)
    {
        Console.Clear();
        Console.Write("Enter product name to find product for update: ");
        var productName = Console.ReadLine();

        var productToUpdate = new ProductDto() { ProductName = productName! };

        if (await productService.CheckIfProductExistsAsync(productName!))
        {
            Console.WriteLine($"Product found! Enter new details:");

            Console.Write("Enter new product name: ");
            productToUpdate.ProductName = Console.ReadLine()!;

            Console.Write("Enter new product description: ");
            productToUpdate.ProductDescription = Console.ReadLine()!;

            Console.Write("Enter new product quantity in stock: ");
            if (!int.TryParse(Console.ReadLine(), out var quantityInStock))
            {
                Console.WriteLine("Invalid quantity input. Please enter a valid integer.");
                return;
            }
            productToUpdate.QuantityInStock = quantityInStock;

            Console.Write("Enter new product production date (YYYY-MM-DD): ");
            if (!DateOnly.TryParse(Console.ReadLine(), out var productionDate))
            {
                Console.WriteLine("Invalid date input. Please enter a valid date in YYYY-MM-DD format.");
                return;
            }
            productToUpdate.ProductionDate = productionDate;

            Console.Write("Enter new product category name: ");
            productToUpdate.CategoryName = Console.ReadLine()!;

            Console.Write("Enter new product manufacturer name: ");
            productToUpdate.ManufacturerName = Console.ReadLine()!;

            Console.Write("Enter new product price: ");
            if (!decimal.TryParse(Console.ReadLine(), out var productPrice))
            {
                Console.WriteLine("Invalid price input. Please enter a valid decimal number.");
                return;
            }
            productToUpdate.ProductPrice = productPrice;

            if (await productService.UpdateProduct(productToUpdate))
            {
                Console.WriteLine("Product updated successfully!");
            }
            else
            {
                Console.WriteLine("Error updating product.");
            }
        }
        else
        {
            Console.WriteLine($"Product with name {productName} not found.");
        }
    }
    public static async Task ShowAllProducts(ProductService productService)
    {
        
        var products = await productService.GetAllProducts();

        if (products != null && products.Count() != 0)
        {
            Console.Clear();
            foreach (var product in products)
            {
                Console.WriteLine($"Product Name: {product.ProductName}, Quantity: {product.QuantityInStock},  Price: {product.ProductPrice}");
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Error fetching products.");
        }
    }
    public static async Task ShowOneProduct(ProductService productService)
    {
        
        try
        {
            Console.Clear();
            Console.Write("Enter product name to show product information: ");

            var productName = Console.ReadLine();

            if (await productService.CheckIfProductExistsAsync(productName!))
            {
                var productDto = await productService.GetOneProduct(new Product { ProductName = productName!});
                Console.Clear();
                // Display product information in the console
                Console.WriteLine($"Product Information:\n" +
                                  $"-----------------\n" +
                                  $"Product Name: {productDto.ProductName}\n" +
                                  $"Description: {productDto.ProductDescription}\n" +
                                  $"Quantity in Stock: {productDto.QuantityInStock}\n" +
                                  $"Production Date: {productDto.ProductionDate}\n" +
                                  $"Category: {productDto.CategoryName}\n" +
                                  $"Manufacturer: {productDto.ManufacturerName}\n" +
                                  $"Price: {productDto.ProductPrice}\n");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Product not found.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
        }
    }

    public static async Task DeleteProductFromDb(ProductService productService)
    {
        Console.Clear();
        Console.Write("Enter product name to delete: ");

        var productName = Console.ReadLine();

        var productDto = new ProductDto { ProductName = productName!};

        if (await productService.DeleteProduct(productDto))
        {
            Console.Clear();
            Console.WriteLine($"Product with name {productName} deleted successfully!");
        }
        else
        {
            Console.Clear();
            Console.WriteLine($"Error deleting product with name {productName}.");
        }
    }
    
}
