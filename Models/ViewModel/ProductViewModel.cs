using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Principal;
using TimeBasedPreventiveMeasures.Data;
using TimeBasedPreventiveMeasures.Models.Data;

namespace TimeBasedPreventiveMeasures.Models.ViewModel
{
    public class ProductViewModel
    {
        public bool IsEditMode { get; set; }

        // Basic Product Informations
        public int? ProductId { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Product Name is Required")]
        public string? ProductName { get; set; }

        [Display(Name = "Product Code")]
        [Required(ErrorMessage = "Product Code is Required")]
        [Remote(action: "ValidateProductCodeIfTaken", controller: "Product", AdditionalFields = nameof(IsEditMode))]
        public string? ProductCode { get; set; }

        [Display(Name = "Product Version")]
        [Required(ErrorMessage = "Product Version is Required")]
        public string? ProductVersion { get; set; }

        [Display(Name = "Product Category")]
        [Required(ErrorMessage = "Product Category is Required")]
        public string? ProductCategory { get; set; }

        [Required(ErrorMessage = "Please select the product status")]
        public string? Status { get; set; }

        [Display(Name = "Vendor Name")]
        [Required(ErrorMessage = "Vendor Name is Required")]
        public string? VendorName { get; set; }

        [Required(ErrorMessage = "Manufacturer is Required")]
        public string? Manufacturer { get; set; }

        [Display(Name = "Purchase Order No.")]
        [Required(ErrorMessage = "Purchase Order is Required")]
        public string? PurchaseOrderNo { get; set; }

        [Display(Name = "Assigned Custodian")]
        public int? AssignedITCustodianId { get; set; }

        [Display(Name = "End Of Support date")]
        [Required(ErrorMessage = "End of support date is required")]
        public DateTime? EOSDate { get; set; }

        [Display(Name = "End Of Life Date")]
        public DateTime? EOLDate { get; set; }

        [Display(Name = "End Of Extended Support date")]
        public DateTime? EOESDate { get; set; }

        [Display(Name = "Support Email")]
        public string? SupportEmail { get; set; }

        [Display(Name = "Support Documentation URL")]
        public string? SupportDocumentationURL { get; set; }

        public CustodianViewModel AssignedCustodian { get; set; } = new CustodianViewModel();

        public IEnumerable<CustodianViewModel> Custodians { get; set; } = new List<CustodianViewModel>();

        public SelectList? SelectListItems { get; set; }

        public static Product ToDataModel(ProductViewModel viewModel)
        {
            return new Product
            {
                ProductId = viewModel.ProductId ?? 0,
                ProductName = viewModel.ProductName,
                ProductCode = viewModel.ProductCode,
                ProductVersion = viewModel.ProductVersion,
                ProductCategory = viewModel.ProductCategory,
                Status = viewModel.Status,
                Manufacturer = viewModel.Manufacturer,
                VendorName = viewModel.VendorName,
                PurchaseOrderNo = viewModel.PurchaseOrderNo,
                AssignedITCustodianId = viewModel.AssignedITCustodianId,
                EOSDate = viewModel.EOSDate,
                EOLDate = viewModel.EOLDate,
                EOESDate = viewModel.EOESDate,
                SupportDocumentationURL = viewModel.SupportDocumentationURL,
                SupportEmail = viewModel.SupportEmail
            };
        }

        public static ProductViewModel ToViewModel(Product dataModel)
        {
            return new ProductViewModel
            {
                ProductId = dataModel.ProductId,
                ProductName = dataModel.ProductName,
                ProductCode = dataModel.ProductCode,
                ProductVersion = dataModel.ProductVersion,
                ProductCategory = dataModel.ProductCategory,
                Status = dataModel.Status,
                Manufacturer = dataModel.Manufacturer,
                VendorName = dataModel.VendorName,
                PurchaseOrderNo = dataModel.PurchaseOrderNo,
                AssignedITCustodianId = dataModel.AssignedITCustodianId,
                EOSDate = dataModel.EOSDate,
                EOLDate = dataModel.EOLDate,
                EOESDate = dataModel.EOESDate,
                SupportDocumentationURL = dataModel.SupportDocumentationURL,
                SupportEmail = dataModel.SupportEmail
            };
        }
    }

    public class CustodianViewModel
    {
        public int CustodianId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }


        [Display(Name = "Assigned Custodian")]
        [Required(ErrorMessage = "Custodian is Required")]
        public string? FullName { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        public string? Role { get; set; }

        public static Custodian ToDataModel(CustodianViewModel viewModel)
        {
            return new Custodian
            {
                CustodianId = viewModel.CustodianId,
                Email = viewModel.Email ?? "",
                FirstName = viewModel.FirstName ?? "",
                LastName = viewModel.LastName ?? "",
                CustodianRole = viewModel.Role ?? "",
            };
        }

        public static CustodianViewModel ToViewModel(Custodian dataModel)
        {
            return new CustodianViewModel
            {
                CustodianId = dataModel.CustodianId,
                FirstName = dataModel.FirstName ?? "",
                LastName = dataModel.LastName ?? "",
                FullName = $"{dataModel.FirstName ?? ""} {dataModel.LastName ?? ""}",
                Email = dataModel.Email ?? "",
                Role = dataModel.CustodianRole ?? ""
            };
        }
    }
}
