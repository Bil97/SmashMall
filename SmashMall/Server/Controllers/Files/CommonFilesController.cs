using System.IO;
using Microsoft.AspNetCore.Mvc;
using SmashMall.Shared.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmashMall.Controllers.Files
{
    [Route("api/files")]
    [ApiController]
    public class CommonFilesController : ControllerBase
    {

        public CommonFilesController()
        {

        }
        // GET api/files/default/image
        [HttpGet("default/image")]
        public ActionResult Get()
        {
            try
            {
                var file = new FileStream(BaseApi.DefaultImage, FileMode.Open);
                return Ok(file);
            }
            catch (FileNotFoundException)
            {
                return NotFound("File not found");
            }
        }

        // GET api/files/item/image
        [HttpGet("item/{id}")]
        public ActionResult Get(string id)
        {
            try
            {
                var file = new FileStream($"{BaseApi.ItemsPhotosDirectory}/{id}", FileMode.Open);
                return Ok(file);
            }
            catch (FileNotFoundException)
            {
                return NotFound("File not found");
            }
        }

    }
}
