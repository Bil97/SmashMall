using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmashMall.Server.Data;
using SmashMall.Shared.Models.Pickup;
using System;
using System.Threading.Tasks;

namespace SmashMall.Server.Controllers.Delivery
{
    [Route("api/pickup/towns")]
    [ApiController]
    //[Authorize(Roles = "Admin,Manager")]
    public class TownsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TownsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get() => Ok(await _context.Towns.Include(x => x.PickupStations).ToListAsync());

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Town deliveryTown)
        {
            try
            {
                deliveryTown.Id = Guid.NewGuid().ToString();
                _context.Towns.Add(deliveryTown);
                await _context.SaveChangesAsync();

                return Ok("Town added");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Town deliveryTown)
        {
            try
            {
                _context.Entry(deliveryTown).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok("Town updated");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var delivery = await _context.Towns.FindAsync(id);
            if (delivery == null)
            {
                return NotFound("Town not found");
            }
            try
            {
                _context.Towns.Remove(delivery);
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
