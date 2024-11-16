using Azure.Core;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;
using TimeBasedPreventiveMeasures.Data;
using TimeBasedPreventiveMeasures.Models.Data;
using TimeBasedPreventiveMeasures.Models.ViewModel;
using TimeBasedPreventiveMeasures.Services;

namespace TimeBasedPreventiveMeasures.Controllers
{
	public class ProductController : Controller
	{
		private readonly LifecycleManagementDB _context;
		private readonly INotificationService _notificationService;
		private IEnumerable<Product> _products = new List<Product>();
		private IEnumerable<Custodian> _custodians = new List<Custodian>();

		public ProductController(LifecycleManagementDB lifecycleManagementDB, INotificationService notificationService)
		{
			_context = lifecycleManagementDB;
			_notificationService = notificationService;
		}

		#region ViewResults
		public async Task<IActionResult> Index(string searchString)
		{
			IEnumerable<Product> _products = 
				await _context.Products
					.Include(c => c.AssigendItCustodian)
					.ToListAsync();

			// Search functionality
			if (!string.IsNullOrEmpty(searchString))
			{
				_products = _products.Where(e => e.ProductName.Contains(searchString) || e.ProductCode.Contains(searchString));
			}

			return View(_products);
		}

        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _context.Products
                .Include(p => p.ActionPlans)
                .ThenInclude(ap => ap.AssignedToCustodian)
                .FirstOrDefaultAsync(m => m.ProductId == id);

			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		[HttpGet]
        public IActionResult AddProduct()
		{
			var viewModel = new ProductViewModel();
			viewModel.IsEditMode = false;

            var custodians = _context.Custodians.ToList();
            ViewBag.Custodians = new SelectList(custodians, "CustodianId", "CustodianRole");

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddProduct(ProductViewModel viewModel)
		{
			var test = viewModel;
			if (ModelState.IsValid)
			{
				var product = ProductViewModel.ToDataModel(viewModel);
				_context.Add(product);
				await _context.SaveChangesAsync();
				// Check and send notifications if necessary
				var currentDate = DateTime.UtcNow.Date;
				await _notificationService.GenerateEOSNotificationForProductAsync(product, currentDate);
				await _notificationService.GenerateEOLNotificationForProductAsync(product, currentDate);

				return RedirectToAction("Index");
			}

            var custodians = _context.Custodians.ToList();
            ViewBag.Custodians = new SelectList(custodians, "CustodianId", "CustodianRole");

			return View("Index");
		}

		[HttpGet]
        public async Task<IActionResult> EditProduct(int id)
		{
			var productData = await _context.Products
				.Include(c => c.AssigendItCustodian)
				.FirstOrDefaultAsync(p => p.ProductId == id) ?? new Product { };


			_custodians = _context.Custodians.ToList();

			List<CustodianViewModel> custodians = new List<CustodianViewModel>();
            foreach (var item in _custodians)
            {
                custodians.Add(CustodianViewModel.ToViewModel(item));
            };

			ProductViewModel viewModel = ProductViewModel.ToViewModel(_context.Products.FirstOrDefault(p => p.ProductId == id) ?? new());
            viewModel.AssignedCustodian = CustodianViewModel.ToViewModel(productData.AssigendItCustodian ?? new());

			viewModel.IsEditMode = true;

			return View("AddProduct", viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditProduct(int id, ProductViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var productToUpdate = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
				
				if (productToUpdate != null)
				{
					productToUpdate.ProductName = viewModel.ProductName;
					productToUpdate.ProductVersion = viewModel.ProductVersion;
					productToUpdate.ProductCategory = viewModel.ProductCategory;
					productToUpdate.Status = viewModel.Status;
					productToUpdate.Manufacturer = viewModel.Manufacturer;
					productToUpdate.VendorName = viewModel.VendorName;
					productToUpdate.PurchaseOrderNo = viewModel.PurchaseOrderNo;
					productToUpdate.AssignedITCustodianId = viewModel.AssignedITCustodianId;
					productToUpdate.EOSDate = viewModel.EOSDate;
					productToUpdate.EOLDate = viewModel.EOLDate;
					productToUpdate.EOESDate = viewModel.EOESDate;
					productToUpdate.SupportDocumentationURL = viewModel.SupportDocumentationURL;
					productToUpdate.SupportEmail = viewModel.SupportEmail;

                    _context.Products.Update(productToUpdate);
					_context.SaveChanges();
                }

                _products = await _context.Products.ToListAsync();
                var product = await _context.Products
					.Include(p => p.ActionPlans)
					.ThenInclude(ap => ap.AssignedToCustodian)
					.FirstOrDefaultAsync(m => m.ProductId == id);

                return View("ProductDetails", product);
            }

			return NotFound();	
		}

		public IActionResult HardwareHistoryPartial()
		{
			return PartialView("_HardwareMaintenanceHistoryPartial");
		}
		#endregion

		#region RemoteValidation
		/// <summary>
		///     An action for remote validation if the product code is already taken, only if the
		///     form is not in Product Mode. Otherwise the validation will not validate.
		/// </summary>
		/// <param name="productCode">The product code to be checked if already existing</param>
		/// <param name="isEditMode">Determines if the form is in Edit Mode</param>
		/// <returns>
		///     Returns the Json format string for handling validation message if 
		///     the product code already exists and true if the validation is successful
		/// </returns>
		[AcceptVerbs("GET", "POST")]
		public async Task<IActionResult> ValidateProductCodeIfTaken(
			string productCode, bool isEditMode)
		{
            if (!isEditMode)
			{
				bool isTaken = await _context.Products.AnyAsync(p => p.ProductCode == productCode);

				if (isTaken)
				{
					return Json($"Product code {productCode} is already taken");
				}
			}

			return Json(true);
		}
        #endregion

        #region JsonResults
        [HttpGet]
        public JsonResult SearchCustodian(string term)
        {
            string _term = term == null ? "" : term.Trim();

            var custodians = _context.Custodians.ToList()
                .Where(c =>
                    $"{c.FirstName} {c.LastName}".Contains(
                        _term, StringComparison.InvariantCultureIgnoreCase) ||
                    c.CustodianRole.Contains(
                        _term, StringComparison.InvariantCultureIgnoreCase) ||

                    c.Email.Contains(
                        _term, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            return Json(custodians);
        }
        #endregion
    }

}
