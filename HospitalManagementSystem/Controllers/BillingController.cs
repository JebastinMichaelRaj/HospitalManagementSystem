using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

namespace HospitalManagementSystem.Controllers
{
    [Authorize]
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Billing/Index — Admin sees all invoices
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var invoices = await _context.Billings
                .Include(b => b.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(b => b.Appointment)
                    .ThenInclude(a => a.Doctor)
                .OrderByDescending(b => b.BillingDate)
                .ToListAsync();

            return View(invoices);
        }

        // GET: /Billing/MyBills — Patient sees their own invoices
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> MyBills()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var invoices = await _context.Billings
                .Include(b => b.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(b => b.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Where(b => b.Appointment != null && b.Appointment.Patient != null &&
                            b.Appointment.Patient.UserID == userId)
                .OrderByDescending(b => b.BillingDate)
                .ToListAsync();

            return View("MyBills", invoices);
        }

        // GET: /Billing/Details/5 — Shows the full invoice receipt
        [Authorize(Roles = "Admin, Patient")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var billing = await _context.Billings
                .Include(b => b.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(b => b.Appointment)
                    .ThenInclude(a => a.Doctor)
                .FirstOrDefaultAsync(b => b.BillingID == id);

            if (billing == null) return NotFound();

            return View(billing);
        }

        // GET: /Billing/Edit/5 — Returns partial view for modal
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var billing = await _context.Billings.FindAsync(id);
            if (billing == null) return NotFound();

            return PartialView("_Edit", billing);
        }

        // POST: /Billing/Edit/5 — Processes the modal submission
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Billing billing)
        {
            if (id != billing.BillingID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillingExists(billing.BillingID))
                        return NotFound();
                    else
                        throw;
                }
                return Ok(new { message = "Invoice updated successfully!" });
            }

            return PartialView("_Edit", billing);
        }

        // POST: /Billing/MarkAsPaid/5 — Secure POST action (CSRF protected)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(string id)
        {
            if (id == null) return NotFound();

            var billing = await _context.Billings.FindAsync(id);
            if (billing == null) return NotFound();

            if (billing.PaymentStatus == "Paid")
            {
                TempData["Warning"] = "This invoice is already marked as Paid.";
                return RedirectToAction(nameof(Index));
            }

            billing.PaymentStatus = "Paid";
            _context.Update(billing);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Invoice has been marked as Paid successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool BillingExists(string id)
        {
            return _context.Billings.Any(e => e.BillingID == id);
        }
    }
}