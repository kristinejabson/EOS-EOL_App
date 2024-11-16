using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeBasedPreventiveMeasures.Models.Data
{
    public class Custodian
    {
        [Key]
        public int CustodianId { get; set; }

        [Column(TypeName = "NVARCHAR(250)")]
        [DisplayName("First Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string FirstName { get; set; }

        [Column(TypeName = "NVARCHAR(250)")]
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string LastName { get; set; }

        [Column(TypeName = "NVARCHAR(250)")]
        [DisplayName("Role")]
        [Required(ErrorMessage = "This field is required.")]
        public string CustodianRole { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        [DisplayName("Email")]
        [Required(ErrorMessage = "This field is required.")]
        public string Email { get; set; }

        public IEnumerable<Product>? Products { get; set; }
    }
}
