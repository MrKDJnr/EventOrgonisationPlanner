using EventOrgonisationPlanner.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventOrgonisationPlanner.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{


		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<Event> Events { get; set; }
		public DbSet<Registration> Registrations { get; set; }
	}
}