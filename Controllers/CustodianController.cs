#nullable disable
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeBasedPreventiveMeasures.Data;
using TimeBasedPreventiveMeasures.Models.Data;

namespace TimeBasedPreventiveMeasures.Controllers
{
    public class CustodianController : Controller
    {
        private readonly LifecycleManagementDB _context;

        public CustodianController(LifecycleManagementDB context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {

            IQueryable<Custodian> custodiansQuery = _context.Custodians;
   
            if (!string.IsNullOrEmpty(searchString))
            {
                
                if (searchString.Contains(" "))
                {
                    // Full name search
                    string firstName = searchString.Substring(0, searchString.IndexOf(" ")).Trim();
                    string lastName = searchString.Substring(searchString.IndexOf(" ") + 1).Trim();

                    custodiansQuery = custodiansQuery.Where(c =>
                        EF.Functions.Like(c.FirstName.ToUpper(), $"%{firstName.ToUpper()}%") &&
                        EF.Functions.Like(c.LastName.ToUpper(), $"%{lastName.ToUpper()}%")
                    );
                }
                else
                {
             
                    string[] searchTerms = searchString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            
                    custodiansQuery = custodiansQuery.Where(c =>
                        searchTerms.Any(term =>
                            EF.Functions.Like(c.FirstName.ToUpper(), $"%{term.ToUpper()}%") ||
                            EF.Functions.Like(c.LastName.ToUpper(), $"%{term.ToUpper()}%") ||
                            EF.Functions.Like(c.CustodianRole.ToUpper(), $"%{term.ToUpper()}%")
                        )
                    );
                }
            }

            List<Custodian> custodians = await custodiansQuery.ToListAsync();

            return View(custodians);
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {

                return View(new Custodian());
            }
            else
            {
                var custodian = _context.Custodians.Find(id);
                if (custodian == null)
                {
                    return NotFound();
                }
                return View(custodian);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("CustodianId,FirstName,LastName,CustodianRole,Email")] Custodian custodian)
        {
            if (id != custodian.CustodianId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (custodian.CustodianId == 0)
                    {
                        _context.Add(custodian);
                    }
                    else
                    {
                        _context.Update(custodian);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustodianExists(custodian.CustodianId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(custodian);
        }

        private bool CustodianExists(int id)
        {
            return _context.Custodians.Any(e => e.CustodianId == id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var custodian = await _context.Custodians.FindAsync(id);
            if (custodian == null)
            {
                return NotFound();
            }
            _context.Custodians.Remove(custodian);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetAllCustodians()
        {
            var custodians = await _context.Custodians.ToListAsync();
            return Json(custodians);
        }
    }
}
