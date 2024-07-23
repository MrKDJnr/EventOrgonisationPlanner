using EventOrgonisationPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using EventOrgonisationPlanner.Data;
using System.Data.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventOrgonisationPlanner.Controllers.API
{

	[Route("api/[controller]")]
	[ApiController]
	public class RegistrationAPI : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public RegistrationAPI(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/<RegistrationAPI>
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<RegistrationAPI>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}
		// POST: api/Registrations
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Registration registration)
		{
			if (ModelState.IsValid)
			{
				// Check if user is already registered for the event
				if (_context.Registrations.Any(r => r.EventId == registration.EventId && r.UserId == registration.UserId))
				{
					return BadRequest("User is already registered for this event.");
				}

				// Check if the event has available seats
				var @event = await _context.Events.FindAsync(registration.EventId);
				if (@event == null || @event.AvailableSeats == 0)
				{
					return BadRequest("The event is full or does not exist.");
				}

				// Generate a unique reference number
				registration.ReferenceNumber = Guid.NewGuid().ToString();

				// Add the registration and save changes
				_context.Registrations.Add(registration);
				await _context.SaveChangesAsync();

				// Update the available seats for the event
				@event.AvailableSeats -= 1;
				await _context.SaveChangesAsync();

				// Return a 201 Created status code with the location of the new resource
				return CreatedAtAction(nameof(GetRegistration), new { id = registration.Id }, registration);
			}

			return BadRequest(ModelState);
		}

		[HttpGet]
		[Route("api/registrations/{id}")]
		public async Task<IActionResult> GetRegistration(int id)
		{
			if (_context.Registrations == null)
			{
				return NotFound();
			}

			var registration = await _context.Registrations.FindAsync(id);
			if (registration == null)
			{
				return NotFound();
			}

			return Ok(registration);
		}

		// DELETE api/<RegistrationAPI>/5
		[HttpDelete]
		[Route("api/registrations/{id}")]
		public async Task<IActionResult> DeleteRegistration(int id)
		{
			if (_context.Registrations == null)
			{
				return NotFound();
			}

			var registration = await _context.Registrations.FirstOrDefaultAsync(m => m.Id == id);
			if (registration == null)
			{
				return NotFound();
			}

			// Remove the registration from the context
			_context.Registrations.Remove(registration);

			// Save the changes to the database
			await _context.SaveChangesAsync();

			// Return a 204 No Content status code to indicate the deletion was successful
			return NoContent();
		}
	}
}
