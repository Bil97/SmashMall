using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmashMall.Server.Data;
using SmashMall.Shared.Models.Files;
using SmashMall.Shared.Models.Goods;
using SmashMall.Shared.Services;
using ThingLing.ImageResizer;

namespace SmashStores.Controllers.Goods
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsSubcategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public GoodsSubcategoriesController(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET: api/Subcategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GoodSubcategory>>> Get()
        {
            return await _context.GoodSubcategory.Include(c => c.GoodCategory).Include(c => c.Brands).Include(c => c.Image).ToListAsync();
        }

        // GET api/Subcategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GoodSubcategory>> Get(string id)
        {
            var subcategory = await _context.GoodSubcategory.Include(c => c.GoodCategory).Include(c => c.Brands).Include(c => c.Image).FirstOrDefaultAsync(c => c.Id == id);
            if (subcategory == null)
            {
                return NotFound("Item not found");
            }
            return subcategory;
        }

        // GET api/Subcategories/good-subcategories
        [HttpGet("good-subcategories")]
        public async Task<ActionResult> GetGoods()
        {
            var subcategories = await _context.GoodSubcategory.Include(c => c.Brands).ThenInclude(c => c.Goods).ToListAsync();
            var goodSubcategories = new List<GoodSubcategory>();
            if (subcategories != null)
            {
                foreach (var subcategory in subcategories)
                {
                    foreach (var brand in subcategory.Brands)
                    {
                        if (brand.Goods?.Count > 0)
                        {
                            subcategory.Brands = null;
                            goodSubcategories.Add(subcategory);
                        }
                    }
                }
            }
            return Ok(goodSubcategories);
        }

        // POST api/Subcategories
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Post([FromForm] string name, [FromForm] string categoryId)
        {
            try
            {
                var subcategory = new GoodSubcategory
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    GoodCategoryId = categoryId
                };

                if (Request.Form.Files.Any())
                {
                    var file = Request.Form.Files[0];
                    var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);
                    var path = Path.Combine(BaseApi.ItemsPhotosDirectory, fileName);
                    // resize the images to be uniform
                    using var stream = file.OpenReadStream();
                    Resizer.ResizeImageFromStream(stream, path, BaseApi.ItemsPhotosWidth, BaseApi.ItemsPhotosHeight);

                    var image = new GoodSubcategoryImage
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = fileName
                    };
                    subcategory.Image = image;
                    subcategory.ImageId = image.Id;
                }

                _context.GoodSubcategory.Add(subcategory);
                await _context.SaveChangesAsync();

                return Ok("Subcategory added");
            }
            catch (DbUpdateException)
            {
                return BadRequest($"{name} already exists");
            }
            catch (UnauthorizedAccessException)
            {
                return BadRequest(" Access to the storage path is denied");
            }
        }

        // PUT api/Subcategories/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Put(string id, [FromForm] string subcategoryId, [FromForm] string name, [FromForm] string categoryId, [FromForm] string imageId, [FromForm] string imagePath)
        {
           if (id != subcategoryId)
            {
                return NotFound("Item not found");
            }
            try
            {
                var subcategory = new GoodSubcategory
                {
                    Id = subcategoryId,
                    Name = name,
                    GoodCategoryId = categoryId,
                    ImageId = imageId
                };

                if (Request.Form.Files.Any())
                {
                    var file1 = $"{BaseApi.ItemsPhotosDirectory}/{imagePath}";
                    if (System.IO.File.Exists(file1))
                    {
                        System.IO.File.Delete(file1);
                    }

                    var file = Request.Form.Files[0];
                    var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);
                    var path = Path.Combine(BaseApi.ItemsPhotosDirectory, fileName);
                    // resize the images to be uniform
                    using var stream = file.OpenReadStream();
                    Resizer.ResizeImageFromStream(stream, path, BaseApi.ItemsPhotosWidth,BaseApi.ItemsPhotosHeight);

                    var image = new GoodSubcategoryImage
                    {
                        Id = imageId,
                        Name = fileName
                    };

                    _context.Entry(image).State = EntityState.Modified;
                }

                _context.Entry(subcategory).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok("Update successful");
            }
            catch (DbUpdateException)
            {
                return BadRequest($"{name} already exists");
            }

        }

        // DELETE api/Subcategories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var subcategory = await _context.GoodSubcategory.Include(c => c.Image).FirstOrDefaultAsync(i => i.Id == id);
            if (subcategory == null)
            {
                return NotFound("Item not found");
            }
            try
            {
                _context.GoodSubcategory.Remove(subcategory);
                var file = $"{BaseApi.ItemsPhotosDirectory}/{subcategory.Image.Name}";
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }

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
