using InventoryManagement.Models.Entity;
using InventoryManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using InventoryManagement.DTOs;

namespace InventoryManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    // Belirli bir envanterin ürünlerini getir
    [HttpGet("{inventoryId}/ProductsAll")]
    public async Task<IActionResult> GetProductsByInventoryId(Guid inventoryId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(new { Message = "Giriş yapmanız gerekmektedir." });

        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.InventoryId == inventoryId && i.UserId == userId);

        if (inventory == null)
            return NotFound(new { Message = "Bu envantere erişim yetkiniz yok veya envanter bulunamadı." });

        var products = await _context.Products
            .Where(p => p.InventoryId == inventoryId)
            .Select(p => new SimpleProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Stock = p.Stock
            })
            .ToListAsync();

        return Ok(new
        {
            Message = "Ürünler başarıyla getirildi.",
            Data = products
        });
    }

    [HttpPost("NewProduct")]
    public async Task<IActionResult> AddProduct(ProductCreateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(new { Message = "Giriş yapmanız gerekmektedir." });

        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.InventoryId == dto.InventoryId && i.UserId == userId);

        if (inventory == null)
            return BadRequest(new { Message = "Geçersiz envanter bilgisi." });

        var product = new Product
        {
            Name = dto.Name,
            Stock = dto.Stock,
            InventoryId = dto.InventoryId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var addedProduct = await _context.Products
            .Include(p => p.Inventory)
            .FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

        var result = new ProductDto
        {
            ProductId = addedProduct!.ProductId,
            Name = addedProduct.Name,
            Stock = addedProduct.Stock,
            InventoryId = addedProduct.InventoryId,
            InventoryName = addedProduct.Inventory?.Name ?? "(Envanter bilgisi yok)"
        };

        return Ok(new
        {
            Message = "Ürün başarıyla eklendi.",
            Data = result
        });
    }

    // Ürün ID ile getir
    [HttpGet("{id}/Products")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(new { Message = "Giriş yapmanız gerekmektedir." });

        var product = await _context.Products
            .Include(p => p.Inventory)
            .FirstOrDefaultAsync(p => p.ProductId == id && p.Inventory.UserId == userId);

        if (product == null)
            return NotFound(new { Message = "Ürün bulunamadı veya yetkiniz yok." });

        var dto = new ProductDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Stock = product.Stock,
            InventoryId = product.InventoryId,
            InventoryName = product.Inventory?.Name ?? ""
        };

        return Ok(new
        {
            Message = "Ürün başarıyla getirildi.",
            Data = dto
        });
    }

    // Ürün güncelle
    [HttpPut("{id}/ProductUpdate")]
    public async Task<IActionResult> UpdateProduct(Guid id, ProductUpdateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(new { Message = "Giriş yapmanız gerekmektedir." });

        var product = await _context.Products
            .Include(p => p.Inventory)
            .FirstOrDefaultAsync(p => p.ProductId == id && p.Inventory.UserId == userId);

        if (product == null)
            return NotFound(new { Message = "Ürün bulunamadı veya yetkiniz yok." });

        product.Name = dto.Name;
        product.Stock = dto.Stock;
        product.InventoryId = dto.InventoryId;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var result = new ProductDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Stock = product.Stock,
            InventoryId = product.InventoryId,
            InventoryName = product.Inventory?.Name ?? ""
        };

        return Ok(new
        {
            Message = "Ürün başarıyla güncellendi.",
            Data = result
        });
    }

    // Ürün sil
    [HttpDelete("{id}/ProductDelete")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(new { Message = "Giriş yapmanız gerekmektedir." });

        var product = await _context.Products
            .Include(p => p.Inventory)
            .FirstOrDefaultAsync(p => p.ProductId == id && p.Inventory.UserId == userId);

        if (product == null)
            return NotFound(new { Message = "Silinecek ürün bulunamadı veya yetkiniz yok." });

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Ürün başarıyla silindi." });
    }
}
