using System.ComponentModel.DataAnnotations;

namespace EventOrgonisationPlanner.Models
{
	public class Event
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[Range(1, int.MaxValue)]
		public int AvailableSeats { get; set; }
	}
}
