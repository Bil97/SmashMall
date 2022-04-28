using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmashMall.Server.Data;
using SmashMall.Shared.Models.Files;
using SmashMall.Shared.Services;
using ThingLing.ImageResizer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmashStores.Controllers.Files
{
    [Route("api/files/goods")]
    [ApiController]
    public class GoodsImagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GoodsImagesController(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET: api/files/goods/images/good-id
        [HttpGet("images/{id}")]
        public async Task<ActionResult<IEnumerable<GoodImage>>> GetImages(string id) => await _context.GoodImage.Where(x => x.GoodId == id).ToListAsync();

        // GET: api/files/goods/image-name
        [HttpGet("{id}")]
        public ActionResult GetImage(string id)
        {
            var file = $"{BaseApi.ItemsPhotosDirectory}/{id}";
            if (System.IO.File.Exists(file))
            {
                return Ok(new FileStream(file, FileMode.Open));
            }
            return NotFound();
        }

        // POST api/files/goods
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] string goodId)
        {
            try
            {
                var image = new GoodImage
                {
                    Id = Guid.NewGuid().ToString(),
                    GoodId = goodId
                };
                if (Request.Form.Files.Any())
                {
                    var file = Request.Form.Files[0];
                    var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);
                    var path = Path.Combine(BaseApi.ItemsPhotosDirectory, fileName);

                    //using var stream = new FileStream(path, FileMode.Create);
                    //await file.CopyToAsync(stream);
                    image.Name = fileName;

                    // resize the images to be uniform
                    using var stream = file.OpenReadStream();
                    Resizer.ResizeImageFromStream(stream, path, BaseApi.ItemsPhotosWidth, BaseApi.ItemsPhotosHeight);
                }
                _context.GoodImage.Add(image);
                await _context.SaveChangesAsync();

                return Ok("Image added");
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

        // DELETE api/files/goods/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var goodImage = await _context.GoodImage.FindAsync(id);
            if (goodImage == null)
            {
                return NotFound("File not found");
            }
            try
            {
                var file = $"{BaseApi.ItemsPhotosDirectory}/{goodImage.Name}";
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }

                _context.GoodImage.Remove(goodImage);
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
