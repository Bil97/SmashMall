using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmashMall.Server.Data;
using SmashMall.Shared.Models.Goods;
using SmashMall.Shared.Models.UserAccount;
using System;
using System.Threading.Tasks;

namespace SmashMall.Server.Controllers.Goods
{
    [Route("api/goods/key-features")]
    [ApiController]
    [Authorize]
    public class GoodsKeyFeaturesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GoodsKeyFeaturesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GoodKeyFeature keyFeature)
        {
            var good = await _context.Good.FindAsync(keyFeature.GoodId);
            if (good == null) return NotFound();
            var user = await _userManager.GetUserAsync(User);
            if (user.Id != good.UserId)
            {
                return NotFound();
            }
            try
            {
                keyFeature.Id = Guid.NewGuid().ToString();

                _context.GoodsKeyFeatures.Add(keyFeature);
                await _context.SaveChangesAsync();

                return Ok("Success");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] GoodKeyFeature keyFeature)
        {
            var good = await _context.Good.FindAsync(keyFeature.GoodId);
            if (good == null) return NotFound();
            var user = await _userManager.GetUserAsync(User);
            if (user.Id != good.UserId)
            {
                return NotFound();
            }

            try
            {
                _context.Entry(keyFeature).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok("Update successful");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var feature = await _context.GoodsKeyFeatures.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user.Id != feature.Good?.UserId)
            {
                return NotFound();
            }

            try
            {
                _context.GoodsKeyFeatures.Remove(feature);
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
