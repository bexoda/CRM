using CRM.Data;
using CRM.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CRM.Controllers
{
    public class VisitorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VisitorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Visitors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Visitors.Include(v => v.Host);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Visitors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitors
                .Include(v => v.Host)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visitor == null)
            {
                return NotFound();
            }

            return View(visitor);
        }

        // GET: Visitors/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID
            var employee = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.EmployeeId, u.Employee.Firstname, u.Employee.Surname })
                .FirstOrDefault();

            if (employee != null && _context.Employees.Any(e => e.Id == employee.EmployeeId))
            {
                ViewData["HostId"] = employee.EmployeeId;
                ViewData["HostName"] = $"{employee.Firstname} {employee.Surname}";
            }
            else
            {
                ViewData["HostId"] = null;
                ViewData["HostName"] = "No host assigned";
            }
            return View();
        }

        // POST: Visitors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Visitor visitor)
        {
            // Retrieve the logged-in user's Employee ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID
            var employee = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.EmployeeId, u.Employee.Firstname, u.Employee.Surname })
                .FirstOrDefault();

            if (employee != null && _context.Employees.Any(e => e.Id == employee.EmployeeId))
            {
                visitor.HostId = employee.EmployeeId;
                ViewData["HostId"] = employee.EmployeeId;
                ViewData["HostName"] = $"{employee.Firstname} {employee.Surname}";
            }
            else
            {
                ModelState.AddModelError("", "The logged-in user does not have a valid Employee ID.");
                ViewData["HostId"] = null;
                ViewData["HostName"] = "No host assigned";
            }

            if (ModelState.IsValid)
            {
                visitor.CreatedDate = DateTime.UtcNow;
                _context.Add(visitor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(visitor);
        }

        // GET: Visitors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor == null)
            {
                return NotFound();
            }

            var host = await _context.Employees.FindAsync(visitor.HostId);
            if (host != null)
            {
                ViewData["HostName"] = $"{host.Firstname} {host.Surname}";
                ViewData["HostId"] = host.Id;
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID
                var employee = _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => new { u.EmployeeId, u.Employee.Firstname, u.Employee.Surname })
                    .FirstOrDefault();

                if (employee != null && _context.Employees.Any(e => e.Id == employee.EmployeeId))
                {
                    ViewData["HostId"] = employee.EmployeeId;
                    ViewData["HostName"] = $"{employee.Firstname} {employee.Surname}";
                }
                else
                {
                    ViewData["HostId"] = null;
                    ViewData["HostName"] = "No host assigned";
                }
            }

            return View(visitor);
        }

        // POST: Visitors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Visitor visitor)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID
            var employee = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.EmployeeId, u.Employee.Firstname, u.Employee.Surname })
                .FirstOrDefault();

            if (employee != null && _context.Employees.Any(e => e.Id == employee.EmployeeId))
            {
                //visitor.HostId = employee.EmployeeId;
                ViewData["HostId"] = employee.EmployeeId;
                ViewData["HostName"] = $"{employee.Firstname} {employee.Surname}";
            }
            else
            {
                ViewData["HostId"] = null;
                ViewData["HostName"] = "No host assigned";
            }

            if (id != visitor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    visitor.UpdatedDate = DateTime.UtcNow;
                    _context.Update(visitor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitorExists(visitor.Id))
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

            var host = await _context.Employees.FindAsync(visitor.HostId);
            if (host != null)
            {
                ViewData["HostName"] = $"{host.Firstname} {host.Surname}";
            }
            else
            {
                ViewData["HostName"] = "No host assigned";
            }

            return View(visitor);
        }


        // GET: Visitors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitors
                .Include(v => v.Host)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visitor == null)
            {
                return NotFound();
            }

            return View(visitor);
        }

        // POST: Visitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor != null)
            {
                _context.Visitors.Remove(visitor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitorExists(int id)
        {
            return _context.Visitors.Any(e => e.Id == id);
        }
    }
}
