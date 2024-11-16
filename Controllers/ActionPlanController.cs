#nullable disable
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeBasedPreventiveMeasures.Data;
using TimeBasedPreventiveMeasures.Models;
using TimeBasedPreventiveMeasures.Models.Data;
using TimeBasedPreventiveMeasures.Models.ViewModel;

namespace TimeBasedPreventiveMeasures.Controllers
{
    public class ActionPlanController : Controller
    {
        private readonly LifecycleManagementDB _context;

        public ActionPlanController(LifecycleManagementDB context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ActionPlans.ToListAsync());
        }


        public IActionResult AddOrEdit(int id = 0, int productId = 0)
        {
            ActionPlan actionPlan;

            if (id == 0)
            {

                actionPlan = new ActionPlan { ProductId = productId };
            }
            else
            {
                actionPlan = _context.ActionPlans.Include(ap => ap.AssignedToCustodian).FirstOrDefault(ap => ap.ActionPlanId == id);
                if (actionPlan == null)
                {
                    return NotFound();
                }
            }

            // Populate ViewBag.Custodians with the list of custodians
            ViewBag.Custodians = _context.Custodians
                .Select(c => new SelectListItem
                {
                    Value = c.CustodianId.ToString(),
                    Text = $"{c.FirstName} {c.LastName}"
                })
                .ToList();

            return View(actionPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("ActionPlanId,ProductId,Title,Description,AssignedToCustodianId,StartDate,EndDate,Status,Reminder")] ActionPlan actionPlan)
        {
            if (id != actionPlan.ActionPlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (actionPlan.ActionPlanId == 0)
                    {
                        _context.Add(actionPlan);
                    }
                    else
                    {
                        _context.Update(actionPlan);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("ProductDetails", "Product", new { id = actionPlan.ProductId });

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActionPlanExists(actionPlan.ActionPlanId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Custodians = _context.Custodians
                .Select(c => new SelectListItem
                {
                    Value = c.CustodianId.ToString(),
                    Text = $"{c.FirstName} {c.LastName}"
                })
                .ToList();

            return View(actionPlan);
        }

        private bool ActionPlanExists(int id)
        {
            return _context.ActionPlans.Any(e => e.ActionPlanId == id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var action = await _context.ActionPlans.FindAsync(id);
            if (action == null)
            {
                return NotFound();
            }
            _context.ActionPlans.Remove(action);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
