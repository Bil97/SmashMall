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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmashStores.Controllers.Files
{
    [Route("api/files/goods/documents")]
    [ApiController]
    public class GoodDocuments : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GoodDocuments(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET: api/files/goods/documents/good-id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetDocuments(string id) => Ok(await _context.GoodDocuments.Where(x => x.GoodId == id).ToListAsync());

        // GET: api/files/goods/documents/document/documentId
        [HttpGet("document/{id}")]
        public ActionResult GetDocument(string id)
        {
            var file = $"{BaseApi.ItemsDocumentsDirectory}/{id}";
            if (System.IO.File.Exists(file))
            {
                return Ok(new FileStream(file, FileMode.Open));
            }
            return NotFound();
        }

        // POST api/files/goods/documents
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] string goodId)
        {
            try
            {
                if (Request.Form.Files.Any())
                {
                    foreach (var file in Request.Form.Files)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + "_" + file.FileName;
                        var path = Path.Combine(BaseApi.ItemsDocumentsDirectory, fileName);
                        using var stream = new FileStream(path, FileMode.Create);
                        await file.CopyToAsync(stream);

                        var document = new GoodDocument
                        {
                            Id = Guid.NewGuid().ToString(),
                            GoodId = goodId,
                            Name = fileName
                        };
                        _context.GoodDocuments.Add(document);
                    }
                }
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

        // DELETE api/files/goods/documents/documentId
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var goodDocument = await _context.GoodDocuments.FindAsync(id);
            if (goodDocument == null)
            {
                return NotFound("File not found");
            }
            try
            {
                var file = $"{BaseApi.ItemsDocumentsDirectory}/{goodDocument.Name}";
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }

                _context.GoodDocuments.Remove(goodDocument);
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
