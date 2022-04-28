using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmashMall.Server.Data;
using SmashMall.Shared.Models.Items;
using SmashMall.Shared.Models.Orders;
using SmashMall.Shared.Models.UserAccount;
using SmashMall.Shared.Services;

namespace SmashStores.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: api/Orders
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Get() => Ok(await _context.GoodOrder.Include(x => x.Good).ThenInclude(x => x.Images).Include(x => x.PickupStation).ToListAsync());

        // GET: api/Orders/userOrderId
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var userId = _userManager.GetUserId(User);
            return Ok(await _context.GoodOrder.Include(x => x.Good).ThenInclude(x => x.Images).Include(x => x.PickupStation).Where(x => x.UserOrderId == id).Where(x => x.UserId == userId).ToListAsync());
        }

        // GET: api/Orders/user
        [HttpGet("user")]
        public async Task<ActionResult> GetUserOrders()
        {
            var userId = _userManager.GetUserId(User);
            return Ok(await _context.GoodOrder.Include(x => x.Good).ThenInclude(x => x.Images).Where(x => x.UserId == userId).Include(x => x.PickupStation).ToListAsync());
        }

        // GET: api/Orders/seller
        [HttpGet("seller")]
        public async Task<ActionResult> GetSellerGoods()
        {
            var userId = _userManager.GetUserId(User);

            var orders = await _context.GoodOrder.Include(x => x.Good).ThenInclude(x => x.Images).Where(x => x.SellerId == userId).Include(x => x.PickupStation).ToListAsync();
            return Ok(orders);
        }

        // POST api/Orders
        [HttpPost]
        public async Task<ActionResult> Post(GoodOrderModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var cart = await _context.GoodsCart.Where(x => x.UserId == user.Id).Include(x => x.Good).ToListAsync();
                if (cart == null)
                {
                    return NotFound("Invalid operation");
                }
                var userOrderId = Guid.NewGuid().ToString();
                foreach (var good in cart)
                {
                    var order = new GoodOrder
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserOrderId = userOrderId,
                        GoodId = good.Good?.Id,
                        IsConfirmed = false,
                        IsApproved = false,
                        TotalPrice = good.TotalPrice,
                        TotalWeight = good.TotalWeight,
                        Quantity = good.Quantity,
                        UserId = user.Id,
                        SellerId = good.Good?.UserId,
                        PickupStationId = model.PickupStationId,
                        LocalDelivery = model.LocalDelivery
                    };
                    _context.GoodOrder.Add(order);
                }
                _context.GoodsCart.RemoveRange(cart);
                await _context.SaveChangesAsync();
                //var callbackUrl = BaseApi.Url;
                //await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                //    $"Your order with id <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{userOrderId}</a> has been placed and is waiting confirmation.");

                return Ok(new CartModel { UserId = user.Id, ItemsCount = 0, Weight = 0 });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/Orders
        [HttpPut]
        public async Task<ActionResult> Put(GoodOrder order)
        {
            try
            {
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Update successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
