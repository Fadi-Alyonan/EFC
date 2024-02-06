using Shared.Dtos;
using Shared.Entities;
using Shared.Repositories;
using System.Diagnostics;

namespace Shared.Services;

public class ProductService(ProductRepository productRepository, ProductionInformationRepository productionInformationRepository, PriceRepository priceRepository, ManufacturerRepository manufacturerRepository, CategoryRepository categoryRepository)
{
    private readonly ProductRepository _productRepository = productRepository;
    private readonly ProductionInformationRepository _productionInformationRepository = productionInformationRepository;
    private readonly PriceRepository _priceRepository = priceRepository;
    private readonly ManufacturerRepository _manufacturerRepository = manufacturerRepository;
    private readonly CategoryRepository _categoryRepository = categoryRepository;

    public async Task<bool> CreateProduct(ProductDto products)
    {
        try
        {
            if (!await CheckIfProductExistsAsync(products.ProductName))
            {
                var category = new Category
                {
                    CategoryName = products.CategoryName
                };
                await _categoryRepository.Create(category);
                var manufacturer = new Manufacturer
                {
                    ManufacturerName = products.ManufacturerName
                };
                await _manufacturerRepository.Create(manufacturer);
                var price = new Price
                {
                    ProductPrice = products.ProductPrice
                };
                await _priceRepository.Create(price);
                var productionInformation = new ProductionInformation
                {
                    ProductionDate = products.ProductionDate
                };
                await _productionInformationRepository.Create(productionInformation);
                var product = new Product
                {
                    CategoryId = category.CategoryId,
                    ManufacturerId = manufacturer.ManufacturerId,
                    PriceId = price.PriceId,
                    ProductionId = productionInformation.ProductionId,
                    ProductName = products.ProductName,
                    ProductDescription = products.ProductDescription,
                    QuantityInStock = products.QuantityInStock,
                };
                await _productRepository.Create(product);
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR::" + ex.Message); }
        return false;

    }
    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        var products = new List<ProductDto>();


        try
        {
            var result = await _productRepository.GetAll();

            foreach (var item in result)
            {
                
                products.Add(new ProductDto
                {
                    ProductName = item.ProductName,
                    ProductDescription = item.ProductDescription,
                    QuantityInStock = item.QuantityInStock,
                    CategoryName = item.Category.CategoryName,
                    ManufacturerName = item.Manufacturer.ManufacturerName,
                    ProductPrice = item.Price.ProductPrice
                });
                
            }
            return products;
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
            return null!;

    }
    public async Task<ProductDto> GetOneProduct(Product product)
    {

        try
        {
            var result = await _productRepository.GetOne(x => x.ProductName == product.ProductName);

            if (result != null)
            {
                var productDto = new ProductDto
                {
                    ProductName = result.ProductName,
                    ProductDescription = result.ProductDescription,
                    QuantityInStock = result.QuantityInStock,
                    CategoryName = result.Category.CategoryName,
                    ManufacturerName = result.Manufacturer.ManufacturerName,
                    ProductPrice = result.Price.ProductPrice,
                };

                return productDto;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error :: " + ex.Message);
        }

        return null!;
    }
    public async Task<bool> UpdateProduct(ProductDto products)
    {

        try
        {
            var productEntityUpdate = await _productRepository.GetOne(x => x.ProductId == products.ProductId);
            if (productEntityUpdate != null)
            {
                
                var category = new Category
                {
                    CategoryId = (int)productEntityUpdate.CategoryId!,
                    CategoryName = products.CategoryName,
                };
                await _categoryRepository.Update(x => x.CategoryId == productEntityUpdate.CategoryId, category);
                var manufacturer = new Manufacturer
                {
                    ManufacturerId = (int)productEntityUpdate.ManufacturerId!,
                    ManufacturerName = products.ManufacturerName
                };
                await _manufacturerRepository.Update(x => x.ManufacturerId == productEntityUpdate.ManufacturerId, manufacturer);
                var price = new Price
                {
                    PriceId = (int)productEntityUpdate.PriceId!,
                    ProductPrice = products.ProductPrice
                };
                await _priceRepository.Update(x => x.PriceId == productEntityUpdate.PriceId, price);
                var productionInformation = new ProductionInformation
                {
                    ProductionId = (int)productEntityUpdate.ProductionId!,
                    ProductionDate = products.ProductionDate
                };
                await _productionInformationRepository.Update(x => x.ProductionId == productEntityUpdate.ProductionId, productionInformation);
                var product = new Product
                {
                    CategoryId = (int)productEntityUpdate.CategoryId,
                    ManufacturerId = (int)productEntityUpdate.ManufacturerId,
                    PriceId = (int)productEntityUpdate.PriceId,
                    ProductionId = (int)productEntityUpdate.ProductionId,
                    ProductName = products.ProductName,
                    ProductDescription = products.ProductDescription,
                    QuantityInStock = products.QuantityInStock,
                };
                await _productRepository.Update(x => x.ProductId == productEntityUpdate.ProductId, product);
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR::" + ex.Message); }
        return false;
    }
    public async Task<bool> DeleteProduct(ProductDto product)
    {
        try
        {
            var productEntity = await _productRepository.GetOne(x => x.ProductName == product.ProductName);
            if (productEntity != null)
            {
                await _categoryRepository.Delete(x => x.CategoryId == productEntity.CategoryId);
                await _manufacturerRepository.Delete(x => x.ManufacturerId == productEntity.ManufacturerId);
                await _priceRepository.Delete(x => x.PriceId == productEntity.PriceId);
                await _productionInformationRepository.Delete(x => x.ProductionId == productEntity.ProductionId);
                await _productRepository.Delete(x => x.PriceId == productEntity.PriceId);

                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("ERROR::" + ex.Message);
        }
        return false;
    }
    public async Task<bool> CheckIfProductExistsAsync(string productName)
    {
        if (await _productRepository.Exists(x => x.ProductName == productName))
        {
            Debug.WriteLine($"Product with name:{productName} already exists.");
            return true;
        }

        return false;
    }
}
