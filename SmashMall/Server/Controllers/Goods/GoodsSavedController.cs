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
    [Route("api/Goods")]
    [ApiController]
    [Authorize]
    public class GoodsSavedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GoodsSavedController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET api/Goods/sold
        [HttpGet("saved")]
        public async Task<ActionResult> GetGoodSaved()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null)
            {
                return NotFound("No goods saved");
            }

            var goodsSaved = await _context.GoodsSaved.Include(x => x.Good).ThenInclude(x => x.Images).Include(c => c.Good.Brand).Include(c => c.Good.Images).Include(c => c.Good.Brand.Subcategory).ToListAsync();
            foreach (var goodSaved in goodsSaved)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(c => c.Id == goodSaved.UserId);
                var userDetails = new UserDetails
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    Phonenumber = user.PhoneNumber,
                    PhonenumberConfirmed = user.PhoneNumberConfirmed,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    NationalIdNumber = user.NationalIdNumber,
                    DateCreated = user.DateCreated,
                    Roles = await _userManager.GetRolesAsync(user)
                };
                goodSaved.Good.UserDetails = userDetails;
                goodSaved.Good.IsSaved = true;
            }

            return Ok(goodsSaved);
        }

        // POST api/Goods/saved
        [HttpPost("saved")]
        public async Task<ActionResult> SaveGood(Good good)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest("Access denied");
            }

            try
            {
                var goodSaved = await _context.GoodsSaved.FirstOrDefaultAsync(x => x.GoodId == good.Id);
                if (goodSaved == null)
                {
                    goodSaved = new GoodSaved
                    {
                        Id = Guid.NewGuid().ToString(),
                        GoodId = good.Id,
                        UserId = user.Id
                    };

                    _context.GoodsSaved.Add(goodSaved);
                    await _context.SaveChangesAsync();
                }
                return Ok("Good added");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/Goods/goodId
        [HttpDelete("saved/{id}")]
        public async Task<ActionResult> DeleteSavedGood(string id)
        {
            var Good = await _context.GoodsSaved.FirstOrDefaultAsync(x => x.GoodId == id);
            if (Good == null)
            {
                return NotFound();
            }
            try
            {
                _context.GoodsSaved.Remove(Good);
                await _context.SaveChangesAsync();
                return Ok("Delete successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
