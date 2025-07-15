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
public class InventoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public InventoryController(AppDbContext context)
    {
        _context = context;
    }
    
    private InventoryDto MapInventoryRecursive(Inventory inventory)
    {
        return new InventoryDto
        {
            InventoryId = inventory.InventoryId,
            Name = inventory.Name,
            CreatedAt = inventory.CreatedAt,
            UpdatedAt = inventory.UpdatedAt,
            UserId = inventory.UserId,
            ParentInventoryId = inventory.ParentInventoryId,

            Products = inventory.Products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Stock = p.Stock,
                InventoryId = p.InventoryId,
                InventoryName = inventory.Name
            }).ToList(),

            ChildInventories = _context.Inventories
                .Where(c => c.ParentInventoryId == inventory.InventoryId)
                .Include(c => c.Products)
                .ToList()
                .Select(MapInventoryRecursive)
                .ToList()
        };
    }
    
    // Kullanıcının Envanterlerini Listeleme
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllInventories()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { Message = "Kullanıcı yetkilendirilmemiş." });

        // Sadece en üst seviyedekileri alıyoruz
        var rootInventories = await _context.Inventories
            .Where(i => i.UserId == userId && i.ParentInventoryId == null)
            .Include(i => i.Products)
            .ToListAsync();

        var result = rootInventories.Select(MapInventoryRecursive).ToList();
    
        return Ok(result);
    }

    // Yeni Envanter Oluşturma
    [HttpPost]
    public async Task<IActionResult> CreateInventory(InventoryCreateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        if (dto.ParentInventoryId.HasValue)
        {
            var parent = await _context.Inventories
                .FirstOrDefaultAsync(i => i.InventoryId == dto.ParentInventoryId.Value && i.UserId == userId);

            if (parent == null)
                return BadRequest(new { Message = "Geçersiz üst envanter." });
        }

        var inventory = new Inventory
        {
            Name = dto.Name,
            UserId = userId,
            ParentInventoryId = dto.ParentInventoryId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();

        var inventoryDto = new InventoryDto
        {
            InventoryId = inventory.InventoryId,
            Name = inventory.Name,
            CreatedAt = inventory.CreatedAt,
            UpdatedAt = inventory.UpdatedAt,
            UserId = inventory.UserId,
            ParentInventoryId = inventory.ParentInventoryId,
            ChildInventories = new List<InventoryDto>(),
            Products = new List<ProductDto>()
        };

        return CreatedAtAction(nameof(GetInventoryById), new { id = inventoryDto.InventoryId }, inventoryDto);
    }

    // Id'ye göre Envanteri Getir
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInventoryById(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var inventory = await _context.Inventories
            .Include(i => i.Products)
            .Include(i => i.ChildInventories)
            .FirstOrDefaultAsync(i => i.InventoryId == id && i.UserId == userId);

        if (inventory == null) return NotFound();

        InventoryDto ToDto(Inventory inv)
        {
            return new InventoryDto
            {
                InventoryId = inv.InventoryId,
                Name = inv.Name,
                CreatedAt = inv.CreatedAt,
                UpdatedAt = inv.UpdatedAt,
                UserId = inv.UserId,
                ParentInventoryId = inv.ParentInventoryId,
                Products = inv.Products.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Stock = p.Stock
                }).ToList(),
                ChildInventories = inv.ChildInventories.Select(ci => ToDto(ci)).ToList()
            };
        }

        var result = ToDto(inventory);
        return Ok(result);
    }

    // Envanteri Güncelle
    [HttpPut("{id}/update")]
    public async Task<IActionResult> UpdateInventory(Guid id, InventoryUpdateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { Message = "Kullanıcı yetkilendirilmemiş." });

        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { Message = "Envanter ismi boş olamaz." });

        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.InventoryId == id && i.UserId == userId);

        if (inventory == null)
            return NotFound(new { Message = "Güncellenecek envanter bulunamadı." });

        inventory.Name = dto.Name;
        inventory.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var inventoryDto = new InventoryDto
        {
            InventoryId = inventory.InventoryId,
            Name = inventory.Name,
            CreatedAt = inventory.CreatedAt,
            UpdatedAt = inventory.UpdatedAt,
            UserId = inventory.UserId,
            Products = new List<ProductDto>() // Ürünleri ayrıca getirebilirsin
        };

        return Ok(inventoryDto);
    }

    // Envanter Sil
    [HttpDelete("{id}/Delete")]
    public async Task<IActionResult> DeleteInventory(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { Message = "Kullanıcı yetkilendirilmemiş." });

        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.InventoryId == id && i.UserId == userId);

        if (inventory == null)
            return NotFound(new { Message = "Silinecek envanter bulunamadı." });

        _context.Inventories.Remove(inventory);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Depo Başarıyla Silindi." });
    }
}