using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TimeBasedPreventiveMeasures.Models.Data;

namespace TimeBasedPreventiveMeasures.Models.Data
{
    public class ActionPlan
    {
        [Key]
        public int ActionPlanId { get; set; }

        [DisplayName("Product ID")]
        public int ProductId { get; set; }

        [DisplayName("Title")]
        public string? Title { get; set; }
        [DisplayName("Description")]
        public string? Description { get; set; }

        [DisplayName("Assigned ID:")]
        [ForeignKey("AssignedToCustodian")]
        public int AssignedToCustodianId { get; set; }

        public Custodian? AssignedToCustodian { get; set; }

        [DisplayName("Start Date:")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date:")]
        public DateTime? EndDate { get; set; }
        [DisplayName("Status:")]
        public string? Status { get; set; }
        [DisplayName("Reminder:")]
        public string? Reminder { get; set; }
    }
}
