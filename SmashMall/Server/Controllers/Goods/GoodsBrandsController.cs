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
    [Authorize]
    public class GoodsBrandsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public GoodsBrandsController(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult> Get() => Ok(await _context.GoodBrands.Include(c => c.Subcategory).Include(c => c.Goods).ThenInclude(x => x.Images).Include(c => c.Image).ToListAsync());

        // GET api/Brands/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var brand = await _context.GoodBrands.Include(c => c.Subcategory).Include(c => c.Goods).ThenInclude(x => x.Images).Include(c => c.Image).FirstOrDefaultAsync(c => c.Id == id);
            if (brand == null)
            {
                return NotFound("Item not found");
            }
            return Ok(brand);
        }

        // GET api/Brands/good_brands
        [HttpGet("{good_brands}")]
        public async Task<ActionResult> GetBrands()
        {
            var brands = await _context.GoodBrands.Include(c => c.Goods).ToListAsync();
            var brandGoods = new List<GoodBrand>();
            if (brands != null)
            {
                foreach (var brand in brands)
                {
                    if (brand.Goods?.Count > 0)
                    {
                        brandGoods.Add(brand);
                    }
                }
            }
            return Ok(brandGoods);
        }

        // POST api/Brands
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Post([FromForm] string name, [FromForm] string subcategoryId)
        {
            try
            {
                var brand = new GoodBrand
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    SubcategoryId = subcategoryId
                };

                if (Request.Form.Files.Any())
                {
                    var file = Request.Form.Files[0];
                    var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);
                    var path = Path.Combine(BaseApi.ItemsPhotosDirectory, fileName);
                    // resize the images to be uniform
                    using var stream = file.OpenReadStream();
                    Resizer.ResizeImageFromStream(stream, path, BaseApi.ItemsPhotosWidth, BaseApi.ItemsPhotosHeight);

                    var image = new GoodBrandImage
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = fileName,
                    };
                    brand.Image = image;
                    brand.ImageId = image.Id;
                }

                _context.GoodBrands.Add(brand);
                await _context.SaveChangesAsync();

                return Ok("Brand added");
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

        // PUT api/Brands/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Put(string id, [FromForm] string subcategoryId, [FromForm] string name, [FromForm] string imageId, [FromForm] string imagePath)
        {
            try
            {
                var brand = new GoodBrand
                {
                    Id = id,
                    Name = name,
                    SubcategoryId = subcategoryId,
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
                    Resizer.ResizeImageFromStream(stream, path, BaseApi.ItemsPhotosWidth, BaseApi.ItemsPhotosHeight);

                    var image = new GoodBrandImage
                    {
                        Id = imageId,
                        Name = fileName
                    };
                    _context.Entry(image).State = EntityState.Modified;
                }

                _context.Entry(brand).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok("Update successful");
            }
            catch (DbUpdateException)
            {
                return BadRequest($"{name} already exists");
            }

        }

        // DELETE api/Brands/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var brand = await _context.GoodBrands.Include(c => c.Image).FirstOrDefaultAsync(i => i.Id == id);
            if (brand == null)
            {
                return NotFound("Item not found");
            }
            try
            {
                _context.GoodBrands.Remove(brand);
                var file = $"{BaseApi.ItemsPhotosDirectory}/{brand.Image.Name}";
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
