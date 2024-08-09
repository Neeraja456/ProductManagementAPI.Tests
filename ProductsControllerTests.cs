using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.Controllers;
using ProductManagementAPI.Data;
using ProductManagementAPI.Model;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProductManagementAPI.Tests
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly ProductContext _context;

        public ProductsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ProductContext(options);

            // Seed test data
            _context.Products.Add(new Product { Id = 1, Name = "Test Product", Description = "Test Description", Price = 10, Stock = 100 });
            _context.SaveChanges();

            _controller = new ProductsController(_context);
        }

        [Fact]
        public async Task AddToStock_ValidRequest_ReturnsOk()
        {
            // Act
            var result = await _controller.AddToStock(1, 5);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            var product = await _context.Products.FindAsync(1);
            Assert.NotNull(product);
            Assert.Equal(105, product.Stock); 
        }
    }

}
