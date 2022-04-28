using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmashMall.Server.Data;
using SmashMall.Shared.Models.Goods;
using SmashMall.Shared.Models.UserAccount;
using SmashMall.Shared.Services;

namespace SmashStores.Controllers.Goods
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GoodsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Goods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Good>>> Get()
        {
            ApplicationUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name ?? "");
            var goods = await _context.Good.Include(c => c.Brand).Include(c => c.Images).Include(c => c.Brand.Subcategory).ThenInclude(x => x.GoodCategory).Include(x => x.KeyFeatures).Where(x => x.IsApprovedForSale == true).ToListAsync();
            foreach (var good in goods)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(c => c.Id == good.UserId);
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
                good.UserDetails = userDetails;
                if (currentUser == null)
                    good.IsSaved = false;
                else
                {
                    good.IsSaved = await _context.GoodsSaved.Where(x => x.UserId == currentUser.Id).AnyAsync(x => x.GoodId == good.Id);
                }
            }
            return goods;
        }

        // GET api/Goods/UserName
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Good>>> Get(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var user = await _userManager.Users.FirstOrDefaultAsync(c => c.UserName == id);
            if (user is null)
            {
                return BadRequest("Invalid request");
            }

            var goods = await _context.Good.Include(c => c.Brand).Include(c => c.Images).Include(c => c.Brand.Subcategory).ThenInclude(x => x.GoodCategory).Where(c => c.UserId == user.Id).Include(x => x.Documents).Where(x => x.IsApprovedForSale == true).ToListAsync();
            foreach (var good in goods)
            {
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
                good.UserDetails = userDetails;
                if (currentUser == null)
                    good.IsSaved = false;
                else
                {
                    good.IsSaved = await _context.GoodsSaved.Where(x => x.UserId == user.Id).AnyAsync(x => x.GoodId == good.Id);
                }
            }
            return goods;
        }

        // GET api/Goods/good/goodId
        [HttpGet("good/{id}")]
        public async Task<ActionResult<Good>> GetGood(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var good = await _context.Good.Include(c => c.Brand).Include(c => c.Brand.Subcategory).ThenInclude(x => x.GoodCategory).Include(x => x.Documents).Include(x => x.Images).Include(x => x.KeyFeatures).Include(x => x.Feedback).FirstOrDefaultAsync(c => c.Id == id);
            var user = await _userManager.Users.FirstOrDefaultAsync(c => c.Id == good.UserId);
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
            good.UserDetails = userDetails;
            if (currentUser == null)
                good.IsSaved = false;
            else
            {
                good.IsSaved = await _context.GoodsSaved.Where(x => x.UserId == user.Id).AnyAsync(x => x.GoodId == good.Id);
            }
            return good;
        }

        // GET api/Goods/pending-approval
        [HttpGet("pending-approval")]
        [Authorize]
        public async Task<ActionResult> GetPendingGoods()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var goods = await _context.Good.Include(c => c.Brand).Include(c => c.Images).Include(c => c.Brand.Subcategory).ThenInclude(x => x.GoodCategory).Where(x => x.IsApprovedForSale == false).ToListAsync();
            if (!User.IsInRole("Admin")) goods = goods.Where(x => x.UserId == currentUser.Id).ToList();

            foreach (var good in goods)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(c => c.Id == good.UserId);
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
                good.UserDetails = userDetails;
                if (currentUser == null)
                    good.IsSaved = false;
                else
                {
                    good.IsSaved = await _context.GoodsSaved.Where(x => x.UserId == currentUser.Id).AnyAsync(x => x.GoodId == good.Id);
                }
            }
            return Ok(goods);
        }

        // GET api/Goods/rejected
        [HttpGet("rejected")]
        [Authorize]
        public async Task<ActionResult> GetRejectedGoods()
        {
            var goods = await _context.RejectedGoods.Include(x => x.Good).ToListAsync();
            return Ok(goods);
        }

        // GET api/Goods/search/GoodName
        [HttpGet("search/{id}")]
        public async Task<ActionResult<IEnumerable<Good>>> SearchGood(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var goods = await _context.Good.Include(c => c.Brand).Include(c => c.Brand.Subcategory).ThenInclude(x => x.GoodCategory).Where(c => c.Name.Contains(id.ToString())).Where(x => x.IsApprovedForSale == true).ToListAsync();
            foreach (var good in goods)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(c => c.Id == good.UserId);
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
                good.UserDetails = userDetails;
                if (currentUser == null)
                    good.IsSaved = false;
                else
                {
                    good.IsSaved = await _context.GoodsSaved.Where(x => x.UserId == user.Id).AnyAsync(x => x.GoodId == good.Id);
                }
            }
            return goods;
        }

        // POST api/Goods
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] Good good)
        {
            try
            {
                good.Id = Guid.NewGuid().ToString();
                good.DateCreated = DateTime.UtcNow;
                good.UserId = (await _userManager.GetUserAsync(User)).Id;
                good.IsApprovedForSale = false;

                if (User.IsInRole("Admin")) good.IsApprovedForSale = true;

                _context.Good.Add(good);
                await _context.SaveChangesAsync();

                return Ok("Good added");
            }
            catch (DbUpdateException)
            {
                return BadRequest($"Item with code '{good.Code}' already exists");
            }

        }

        // PUT api/Goods/5
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] Good good)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            if (!User.IsInRole("Admin") && good.IsApprovedForSale == true)
            {
                good.IsApprovedForSale = false;
            }

            if (User.IsInRole("Admin")) good.IsApprovedForSale = true;

            if (good.UserId == applicationUser.Id || User.IsInRole("Admin"))
            {
                try
                {
                    _context.Entry(good).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok("Update successful");
                }
                catch (DbUpdateException)
                {
                    return BadRequest($"Item with code '{good.Code}' already exists");
                }
            }
            return NotFound();
        }

        // PUT api/Goods/quantity/goodId
        [HttpPut("quantity/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangeQuantity(int id, [FromBody] Good good)
        {
            var user = await _userManager.GetUserAsync(User);

            if (good.UserId != user.Id) { return BadRequest(); }

            try
            {
                good.Quantity = id;

                _context.Entry(good).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Update successful");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/Goods/reject
        [HttpPost("reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject([FromBody] RejectedGood rejected)
        {
            try
            {
                var good = await _context.Good.FindAsync(rejected.Id);
                if (good == null) return NotFound();

                good.IsApprovedForSale = null;
                _context.Good.Update(good);
                _context.RejectedGoods.Add(rejected);
                await _context.SaveChangesAsync();
                return Ok("Update successful");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("reject/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReject(string id)
        {
            var good = await _context.RejectedGoods.FindAsync(id);
            if (good == null)
            {
                return NotFound();
            }
            try
            {
                _context.RejectedGoods.Remove(good);
                foreach (var image in good.Good?.Images)
                {
                    var file = $"{BaseApi.ItemsPhotosDirectory}/{image.Name}";
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                    }
                }
                await _context.SaveChangesAsync();
                return Ok("Delete successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/Goods/approve-rejected
        [HttpPut("approve-rejected")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveRejected([FromBody] RejectedGood rejected)
        {
            try
            {
                var good = await _context.Good.FindAsync(rejected.Id);
                if (good == null) return NotFound();

                good.IsApprovedForSale = true;
                _context.Entry(good).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Update successful");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/Goods/goodId
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            var Good = await _context.Good.Include(x => x.Images).Include(x => x.Documents).FirstOrDefaultAsync(x => x.Id == id);
            if (Good == null)
            {
                return NotFound();
            }
            try
            {
                if (Good.Images != null)
                {
                    foreach (var image in Good.Images)
                    {
                        var file = $"{BaseApi.ItemsPhotosDirectory}/{image.Name}";
                        if (System.IO.File.Exists(file))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                }
                if (Good.Documents != null)
                {
                    foreach (var document in Good.Documents)
                    {
                        var file = $"{BaseApi.ItemsDocumentsDirectory}/{document.Name}";
                        if (System.IO.File.Exists(file))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                }

                _context.Good.Remove(Good);
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
