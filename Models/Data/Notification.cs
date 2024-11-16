using Microsoft.AspNetCore.Mvc;

namespace TimeBasedPreventiveMeasures.Models
{
	public class Notification
	{
		public int NotificationId { get; set; }
		public string? Subject { get; set; }
		public string? Message { get; set; }
		public int? ITCustodianId { get; set; }
		public string? PurchaseOrderNo { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsRead { get; set; }
	}
}

