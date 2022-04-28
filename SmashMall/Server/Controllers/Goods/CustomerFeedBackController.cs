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
    [Route("api/goods/feedback")]
    [ApiController]
    [Authorize]
    public class CustomerFeedBackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerFeedBackController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpPost("{id}")]
        public async Task<ActionResult> Post(string id, [FromBody] CustomerFeedback content)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.UserName != id)
            {
                return NotFound();
            }
            try
            {
                content.Id = Guid.NewGuid().ToString();
                content.UserId = user?.Id;
                content.DatePosted = DateTime.UtcNow;

                _context.CustomersFeedback.Add(content);
                await _context.SaveChangesAsync();

                return Ok("Success");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] CustomerFeedback content)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.Id != content.UserId)
            {
                return NotFound();
            }

            try
            {
                _context.Entry(content).State = EntityState.Modified;

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
            var content = await _context.CustomersFeedback.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user.Id != content.UserId)
            {
                return NotFound();
            }

            try
            {
                _context.CustomersFeedback.Remove(content);
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
