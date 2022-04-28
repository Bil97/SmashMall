using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmashMall.Server.Data;
using SmashMall.Shared.Models.Goods;
using SmashMall.Shared.Models.Items;
using SmashMall.Shared.Models.Orders;
using SmashMall.Shared.Models.UserAccount;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmashStores.Controllers.Carts
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET api/Cart/userId
        [HttpGet("{id}")]
        public async Task<ActionResult<GoodCart>> Get(string id)
        {
            var cart = await _context.GoodsCart.Include(x => x.Good).ThenInclude(x => x.Images).Where(x => x.UserId == id.ToString()).ToListAsync();
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        // GET api/Cart/count/userId
        [HttpGet("count/{id}")]
        public async Task<ActionResult<GoodCart>> GetCount(string id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null && id.ToString() != user?.Id)
                {
                    var items = await _context.GoodsCart.Where(x => x.UserId == id.ToString()).ToListAsync();
                    if (items?.Count > 0)
                    {
                        foreach (var item in items)
                        {
                            item.UserId = user?.Id;
                            _context.Entry(item).State = EntityState.Modified;
                        }
                        await _context.SaveChangesAsync();
                    }
                }

                var goods = await _context.GoodsCart.Where(x => x.UserId == id.ToString()).ToListAsync();
                if (goods == null)
                {
                    return NotFound();
                }
                var count = goods.Sum(x => x.Quantity);
                var weight = goods.Sum(x => x.TotalWeight);
                var totalPrice = goods.Sum(x => x.TotalPrice);
                return Ok(new CartModel { UserId = id.ToString(), ItemsCount = count, Weight = weight, TotalPrice = totalPrice });
            }
            catch (DbUpdateException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET api/Cart/count/userId
        [HttpGet("weight/{id}")]
        public async Task<ActionResult<GoodCart>> GetWeight(string id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null && id.ToString() != user?.Id)
                {
                    var items = await _context.GoodsCart.Where(x => x.UserId == id.ToString()).ToListAsync();
                    if (items?.Count > 0)
                    {
                        foreach (var item in items)
                        {
                            item.UserId = user?.Id;
                            _context.Entry(item).State = EntityState.Modified;
                        }
                        await _context.SaveChangesAsync();
                    }
                }

                var goods = await _context.GoodsCart.Where(x => x.UserId == id.ToString()).ToListAsync();
                if (goods == null)
                {
                    return NotFound();
                }
                var count = goods.Sum(x => x.Quantity);
                var weight = goods.Sum(x => x.TotalWeight);
                var totalPrice = goods.Sum(x => x.TotalPrice);
                return Ok(new CartModel { UserId = id.ToString(), ItemsCount = count, Weight = weight, TotalPrice = totalPrice });
            }
            catch (DbUpdateException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST api/Cart
        [HttpPost]
        public async Task<ActionResult<GoodCart>> Post([FromBody] Good good)
        {
            var user = await _userManager.GetUserAsync(User);
            string userId;
            if (user == null) userId = Guid.NewGuid().ToString().ToString();
            else userId = user.Id;
            var cart = new GoodCart
            {
                Id = Guid.NewGuid().ToString(),
                GoodId = good.Id,
                UserId = userId,
                TotalPrice = good.Price,
                Quantity = 1,
                TotalWeight = good.Weight

            };
            _context.GoodsCart.Add(cart);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new CartModel { UserId = userId, ItemsCount = 1, Weight = good.Weight, TotalPrice = good.Price });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // POST api/Cart/user-id
        [HttpPost("{id}")]
        public async Task<ActionResult> Post(string id, [FromBody] Good good)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null && id != user?.Id)
                {
                    var items = await _context.GoodsCart.Where(x => x.UserId == id).ToListAsync();
                    if (items?.Count > 0)
                    {
                        foreach (var item in items)
                        {
                            item.UserId = user?.Id;
                            _context.Update(item);
                        }
                        await _context.SaveChangesAsync();
                    }
                    id = user.Id;
                }

                var cart = await _context.GoodsCart.Include(x => x.Good).Where(x => x.UserId == id).FirstOrDefaultAsync(x => x.GoodId == good.Id);
                if (cart == null)
                {
                    cart = new GoodCart
                    {
                        Id = Guid.NewGuid().ToString(),
                        GoodId = good.Id,
                        UserId = id,
                        TotalPrice = good.Price,
                        Quantity = 1,
                        TotalWeight = good.Weight
                    };
                    _context.GoodsCart.Add(cart);
                }
                else
                {
                    cart.GoodId = good.Id;
                    cart.UserId = id;
                    cart.Quantity += 1;
                    cart.TotalPrice = cart.Quantity * good.Price;
                    cart.TotalWeight = cart.Quantity * good.Weight;
                    _context.Entry(cart).State = EntityState.Modified;
                };

                await _context.SaveChangesAsync();

                var goods = await _context.GoodsCart.Where(x => x.UserId == id).ToListAsync();
                var count = goods.Sum(x => x.Quantity);
                var weight = goods.Sum(x => x.TotalWeight);
                var totalPrice = goods.Sum(x => x.TotalPrice);
                return Ok(new CartModel { UserId = id, ItemsCount = count, Weight = weight, TotalPrice = totalPrice });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/Cart/userId
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] GoodCartModel goodCart)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null && id != user?.Id)
                {
                    var items = await _context.GoodsCart.Where(x => x.UserId == id).ToListAsync();
                    if (items?.Count > 0)
                    {
                        foreach (var item in items)
                        {
                            item.UserId = user?.Id;
                            _context.Entry(item).State = EntityState.Modified;
                        }
                        await _context.SaveChangesAsync();
                    }
                    id = user.Id;
                }

                var cart = await _context.GoodsCart.Include(x => x.Good).FirstOrDefaultAsync(x => x.Id == goodCart.CartId);
                if (cart != null)
                {
                    var good = await _context.Good.FirstOrDefaultAsync(x => x.Id == cart.GoodId);
                    cart.UserId = id;
                    cart.Quantity = goodCart.Quantity;
                    cart.TotalPrice = good.Price * cart.Quantity;
                    cart.TotalWeight = cart.Quantity * good.Weight;
                    _context.Entry(cart).State = EntityState.Modified;
                };

                await _context.SaveChangesAsync();

                var goods = await _context.GoodsCart.Where(x => x.UserId == id).ToListAsync();
                var count = goods.Sum(x => x.Quantity);
                var weight = goods.Sum(x => x.TotalWeight);
                var totalPrice = goods.Sum(x => x.TotalPrice);
                return Ok(new CartModel { UserId = id, ItemsCount = count, Weight = weight, TotalPrice = totalPrice });
            }
            catch (DbUpdateException ex)
            {
                return NotFound(ex.Message);
            }

        }

        // DELETE api/Cart/cartId
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var cart = await _context.GoodsCart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            try
            {
                string userId = cart.UserId;
                _context.GoodsCart.Remove(cart);
                await _context.SaveChangesAsync();

                var user = await _userManager.GetUserAsync(User);

                if (user != null && cart.UserId != user?.Id)
                {
                    var items = await _context.GoodsCart.Where(x => x.UserId == cart.UserId).ToListAsync();
                    if (items?.Count > 0)
                    {
                        foreach (var item in items)
                        {
                            item.UserId = user?.Id;
                            _context.Entry(item).State = EntityState.Modified;
                        }
                        await _context.SaveChangesAsync();
                    }
                    userId = user.Id;
                }

                var goods = await _context.GoodsCart.Where(x => x.UserId == userId).ToListAsync();
                var count = goods.Sum(x => x.Quantity);
                var weight = goods.Sum(x => x.TotalWeight);
                var totalPrice = goods.Sum(x => x.TotalPrice);
                return Ok(new CartModel { UserId = userId, ItemsCount = count, Weight = weight, TotalPrice = totalPrice });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/Cart/clear/userId
        [HttpDelete("clear/{id}")]
        public async Task<ActionResult> DeleteCart(string id)
        {
            var carts = _context.GoodsCart.Where(x => x.UserId == id);
            if (carts == null)
            {
                return NotFound();
            }

            try
            {
                foreach (var cart in carts)
                {
                    _context.GoodsCart.Remove(cart);
                }
                await _context.SaveChangesAsync();

                return Ok(new CartModel { UserId = id, ItemsCount = 0, Weight = 0, TotalPrice = 0 });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
