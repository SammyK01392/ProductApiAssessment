using Application.DTOs;
using Application.DTOs.Item;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductApiAssessment.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        // GET api/v1/items
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ItemDto>>> GetAll()
        {
            var result = await _itemService.GetAllAsync();
            return Ok(result);
        }

        // GET api/v1/items/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ItemDto>> GetById(int id)
        {
            var result = await _itemService.GetByIdAsync(id);
            return Ok(result);
        }

        // GET api/v1/items/by-product/3
        [HttpGet("by-product/{productId:int}")]
        public async Task<ActionResult<IReadOnlyList<ItemDto>>> GetByProductId(int productId)
        {
            var result = await _itemService.GetByProductIdAsync(productId);
            return Ok(result);
        }

        // POST api/v1/items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> Create([FromBody] CreateItemDto dto)
        {
            var result = await _itemService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id, version = "1.0" }, result);
        }

        // PUT api/v1/items/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateItemDto dto)
        {
            await _itemService.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE api/v1/items/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _itemService.DeleteAsync(id);
            return NoContent();
        }
    }
}
