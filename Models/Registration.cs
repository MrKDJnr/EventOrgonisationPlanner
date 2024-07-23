using System.ComponentModel.DataAnnotations;

namespace EventOrgonisationPlanner.Models
{
	public class Registration
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int EventId { get; set; }

		[Required]
		[MaxLength(50)]
		public string UserId { get; set; }

		[Required]
		[MaxLength(100)]
		public string ReferenceNumber { get; set; }
	}
}
