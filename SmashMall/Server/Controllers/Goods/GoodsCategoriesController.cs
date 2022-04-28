using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmashMall.Server.Data;
using SmashMall.Shared.Models.Goods;

namespace SmashStores.Controllers.Goods
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GoodsCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/GoodsCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GoodCategory>>> Get()
        {
            return await _context.GoodCategory.Include(x => x.Subcategories).ThenInclude(x => x.Brands).ThenInclude(x => x.Goods).ToListAsync();
        }

        // GET api/GoodsCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GoodCategory>> Get(string id)
        {
            var Category = await _context.GoodCategory.Include(x => x.Subcategories).ThenInclude(x => x.Brands).ThenInclude(x => x.Goods).FirstOrDefaultAsync(c => c.Id == id);
            if (Category == null)
            {
                return NotFound();
            }
            return Category;
        }

        // GET api/GoodsCategories/category-goods
        [HttpGet("category-goods")]
        public async Task<ActionResult<IEnumerable<GoodCategory>>> GetCategoryGoods()
        {
            var categories = await _context.GoodCategory.Include(x => x.Subcategories).ThenInclude(x => x.Brands).ThenInclude(x => x.Goods).ToListAsync();
            var categoryGoods = new List<GoodCategory>();
            if (categories != null)
            {
                foreach (var category in categories)
                {
                    foreach (var subcategory in category.Subcategories)
                    {
                        if (subcategory.Brands?.Count > 0)
                        {
                            foreach (var brand in subcategory.Brands)
                            {
                                if (brand.Goods?.Count > 0)
                                {
                                    category.Subcategories = null;
                                    categoryGoods.Add(category);
                                }
                            }
                        }
                    }
                }
            }
            return Ok(categoryGoods.Distinct());
        }

        // POST api/GoodsCategories
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Post([FromBody] GoodCategory category)
        {
            try
            {
                category.Id = Guid.NewGuid().ToString();
                _context.GoodCategory.Add(category);
                await _context.SaveChangesAsync();
                return Ok("Category added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/GoodsCategories/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Put(string id, [FromBody] GoodCategory category)
        {
            if (id != category.Id)
            {
                return NotFound("Item not found");
            }
            try
            {
                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Update successful");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE api/GoodsCategories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var Category = await _context.GoodCategory.FindAsync(id);
            if (Category == null)
            {
                return NotFound("Item not found");
            }
            try
            {
                _context.GoodCategory.Remove(Category);
                await _context.SaveChangesAsync();
                return Ok("Delete successful");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
