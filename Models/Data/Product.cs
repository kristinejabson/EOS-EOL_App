using System.ComponentModel.DataAnnotations;

namespace TimeBasedPreventiveMeasures.Models.Data
{
    public class Product
    {
        // Basic Product Details
        public int ProductId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? ProductVersion { get; set; }
        public string? ProductCategory { get; set; }
        public string? Status { get; set; }
        public string? Manufacturer { get; set; }
        public string? VendorName { get; set; }
        public string? PurchaseOrderNo { get; set; }
        public int? AssignedITCustodianId { get; set; }

		// Support Details
		public DateTime? EOSDate { get; set; }
        public DateTime? EOLDate { get; set; }
        public DateTime? EOESDate { get; set; }
        public string? SupportDocumentationURL { get; set; }
        public string? SupportEmail { get; set; }
		
        public Custodian? AssigendItCustodian { get; set; }

        public ICollection<ActionPlan> ActionPlans { get; set; }

    }
}
