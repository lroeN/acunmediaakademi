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
   
   // Kullanıcının Envanterlerini Listeleme
   [HttpGet("Kullanıcının Envanterini Listeleme")]
   public async Task<IActionResult> GetAllInventories()
   {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (string.IsNullOrEmpty(userId)) 
         return Unauthorized(new { Message = "Kullanıcı yetkilendirilmemiş." });
      
      var inventories = await _context.Inventories
         .Where(i => i.UserId == userId)
         .Select(i => new InventoryDto
         {
            InventoryId = i.InventoryId,
            Name = i.Name,
            CreatedAt = i.CreatedAt,
            UpdatedAt = i.UpdatedAt,
            UserId = i.UserId,
            Products = i.Products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Stock = p.Stock
            }).ToList()
         })
         .ToListAsync();
      
      return Ok(inventories);
   }
   
   // Yeni Envanter Oluşturma
   [HttpPost("Envanter Oluşturma")]
   public async Task<IActionResult> CreateInventory(InventoryCreateDto dto)
   {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (string.IsNullOrEmpty(userId)) 
         return Unauthorized(new { Message = "Kullanıcı yetkilendirilmemiş." });

      if (string.IsNullOrWhiteSpace(dto.Name))
         return BadRequest(new { Message = "Envanter ismi boş olamaz." });

      var inventory = new Inventory
      {
         Name = dto.Name,
         UserId = userId,
         CreatedAt = DateTime.UtcNow,
         UpdatedAt = DateTime.UtcNow
      };

      _context.Inventories.Add(inventory);
      await _context.SaveChangesAsync();

      var createdInventory = await _context.Inventories
         .Include(i => i.Products)
         .FirstOrDefaultAsync(i => i.InventoryId == inventory.InventoryId);

      if (createdInventory == null)
         return NotFound(new { Message = "Envanter oluşturulurken hata oluştu." });

      var inventoryDto = new InventoryDto
      {
         InventoryId = createdInventory.InventoryId,
         Name = createdInventory.Name,
         CreatedAt = createdInventory.CreatedAt,
         UpdatedAt = createdInventory.UpdatedAt,
         UserId = createdInventory.UserId,
         Products = createdInventory.Products.Select(p => new ProductDto
         {
            ProductId = p.ProductId,
            Name = p.Name,
            Stock = p.Stock
         }).ToList()
      };

      return CreatedAtAction(nameof(GetInventoryById), new { id = inventoryDto.InventoryId }, inventoryDto);
   }

   // Id'ye göre Envanteri Getir
   [HttpGet("{id} 'ye Göre Envanteri Getirme")]
   public async Task<IActionResult> GetInventoryById(Guid id)
   {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (string.IsNullOrEmpty(userId)) 
         return Unauthorized(new { Message = "Kullanıcı yetkilendirilmemiş." });

      var inventory = await _context.Inventories
         .Include(i => i.Products)
         .FirstOrDefaultAsync(i => i.InventoryId == id && i.UserId == userId);

      if (inventory == null)
         return NotFound(new { Message = "Envanter bulunamadı." });

      var inventoryDto = new InventoryDto
      {
         InventoryId = inventory.InventoryId,
         Name = inventory.Name,
         CreatedAt = inventory.CreatedAt,
         UpdatedAt = inventory.UpdatedAt,
         UserId = inventory.UserId,
         Products = inventory.Products.Select(p => new ProductDto
         {
            ProductId = p.ProductId,
            Name = p.Name,
            Stock = p.Stock
         }).ToList()
      };

      return Ok(inventoryDto);
   }

   // Envanteri Güncelle
   [HttpPut("{id} Envanteri Güncelleme")]
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
         Products = new List<ProductDto>() // Güncelleme sonrası ürünler istenirse ayrıca getirilebilir
      };

      return Ok(inventoryDto);
   }

   // Envanter Sil
   [HttpDelete("{id} Kullanarak Envanteri Silme")]
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

      return Ok(new{ Message = "Depo Başıylar Silindi."});
   }
}
