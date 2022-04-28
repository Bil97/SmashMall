using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmashMall.Server.Data;
using SmashMall.Shared.Models.Files;
using SmashMall.Shared.Models.Items;
using SmashMall.Shared.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThingLing.ImageResizer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmashMall.Server.Controllers.TopDeals
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopDealsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TopDealsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<TopDealsController>
        [HttpGet]
        public async Task<ActionResult> Get() => Ok(await _context.TopDeals.Include(x => x.Image).ToListAsync());

        // GET api/<TopDealsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id) => Ok(await _context.TopDeals.Include(x => x.Image).FirstOrDefaultAsync(x => x.Id == id));

        // POST api/<TopDealsController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Post([FromForm] string name, [FromForm] string pageUrl)
        {
            try
            {
                var image = new TopDealImage
                {
                    Id = Guid.NewGuid().ToString(),
                };
                if (Request.Form.Files.Any())
                {
                    // var file = Request.Form.Files[0];
                    // var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);
                    // var path = Path.Combine(BaseApi.ItemsPhotosDirectory, fileName);
                    // // resize the images to be uniform
                    // using var stream = file.OpenReadStream();
                    // var resizer = new Resizer();
                    // resizer.ResizeImageFromStream(stream, path, 200, 150);
                    // image.Name = fileName;
                }
                var deal = new TopDeal
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    PageUrl = pageUrl,
                    ImageId = image.Id,
                    Image = image,
                };
                _context.TopDeals.Add(deal);
                await _context.SaveChangesAsync();

                return Ok("Success");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<TopDealsController>/dealId
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Put(string id, [FromForm] string name, [FromForm] string pageUrl, [FromForm] string imageId, string imagePath)
        {
            try
            {
                var deal = new TopDeal
                {
                    Id = id,
                    Name = name,
                    PageUrl = pageUrl,
                    ImageId = imageId,
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
                    Resizer.ResizeImageFromStream(stream, path, 600, 450);

                    var image = new TopDealImage
                    {
                        Id = imageId,
                        Name = fileName
                    };
                    _context.Entry(image).State = EntityState.Modified;

                }
                _context.Entry(deal).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok("Update successful");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<TopDealsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var content = await _context.TopDeals.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            try
            {
                _context.TopDeals.Remove(content);
                await _context.SaveChangesAsync();

                if (System.IO.File.Exists(content.Image.Name))
                {
                    System.IO.File.Delete(content.Image.Name);
                }

                return Ok("Delete successful");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
