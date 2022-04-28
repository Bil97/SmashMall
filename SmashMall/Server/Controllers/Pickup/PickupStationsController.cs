using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmashMall.Server.Data;
using SmashMall.Shared.Models.Pickup;
using System;
using System.Threading.Tasks;

namespace SmashMall.Server.Controllers.Delivery
{
    [Route("api/pickup/stations")]
    [ApiController]
   // [Authorize(Roles = "Admin,Manager")]
    public class PickupStationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PickupStationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get() => Ok(await _context.PickupStations.Include(x => x.Town).ToListAsync());

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PickupStation delivery)
        {
            try
            {
                delivery.Id = Guid.NewGuid().ToString();
                _context.PickupStations.Add(delivery);
                await _context.SaveChangesAsync();

                return Ok("Success");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] PickupStation delivery)
        {
            try
            {
                _context.Entry(delivery).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok("Success");
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
            var delivery = await _context.PickupStations.FindAsync(id);
            if (delivery == null)
            {
                return NotFound("Error deleting");
            }
            try
            {
                _context.PickupStations.Remove(delivery);
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
