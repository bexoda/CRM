using CRM.Data;
using CRM.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(ApplicationDbContext context, ILogger<AppointmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all appointments");
            var applicationDbContext = _context.Appointments.Include(a => a.Client).Include(a => a.Staff);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Appointment ID was null in Details");
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Staff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                _logger.LogWarning($"Appointment not found with ID {id}");
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Company");
            ViewData["StaffId"] = new SelectList(_context.Employees, "Id", "Email");
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            appointment.CreatedDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(appointment);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Appointment created successfully";
                    _logger.LogInformation("Appointment created successfully");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating appointment");
                    TempData["error"] = "Error occurred while creating appointment";
                }
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Company", appointment.ClientId);
            ViewData["StaffId"] = new SelectList(_context.Employees, "Id", "Email", appointment.StaffId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Appointment ID was null in Edit");
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                _logger.LogWarning($"Appointment not found with ID {id}");
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Company", appointment.ClientId);
            ViewData["StaffId"] = new SelectList(_context.Employees, "Id", "Email", appointment.StaffId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                _logger.LogWarning($"Appointment ID mismatch in Edit: {id} != {appointment.Id}");
                return NotFound();
            }
            appointment.UpdatedDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Appointment updated successfully";
                    _logger.LogInformation("Appointment updated successfully");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        _logger.LogWarning($"Appointment not found with ID {appointment.Id} during update");
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Concurrency exception occurred while updating appointment");
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating appointment");
                    TempData["error"] = "Error occurred while updating appointment";
                }
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Company", appointment.ClientId);
            ViewData["StaffId"] = new SelectList(_context.Employees, "Id", "Email", appointment.StaffId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Appointment ID was null in Delete");
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Staff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                _logger.LogWarning($"Appointment not found with ID {id}");
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment != null)
                {
                    _context.Appointments.Remove(appointment);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Appointment deleted successfully";
                    _logger.LogInformation("Appointment deleted successfully");
                }
                else
                {
                    _logger.LogWarning($"Appointment not found with ID {id} during deletion");
                    TempData["error"] = "Appointment not found";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting appointment");
                TempData["error"] = "Error occurred while deleting appointment";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
