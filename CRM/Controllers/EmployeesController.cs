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
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeesController> _logger;

        // Single constructor accepting both ApplicationDbContext and ILogger<EmployeesController>
        public EmployeesController(ApplicationDbContext context, ILogger<EmployeesController> logger)
        {
            _context = context; // Initialize the database context
            _logger = logger;   // Initialize the logger
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all employees");
            var applicationDbContext = _context.Employees.Include(e => e.Department); // Include Department data
            return View(await applicationDbContext.ToListAsync()); // Return the list of employees
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Employee ID was null in Details");
                return NotFound(); // Return 404 if ID is null
            }

            var employee = await _context.Employees
                .Include(e => e.Department) // Include Department data
                .FirstOrDefaultAsync(m => m.Id == id); // Find employee by ID
            if (employee == null)
            {
                _logger.LogWarning($"Employee not found with ID {id}");
                return NotFound(); // Return 404 if employee is not found
            }

            return View(employee); // Return the employee details view
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName"); // Populate Department dropdown
            return View(); // Return the create view
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            employee.CreatedDate = DateTime.UtcNow; // Set the created date
            ModelState.Remove("Department"); // Remove Department validation

            if (ModelState.IsValid) // Check if model state is valid
            {
                try
                {
                    _context.Add(employee); // Add employee to context
                    await _context.SaveChangesAsync(); // Save changes to database
                    TempData["success"] = "Employee created successfully"; // Set success message
                    _logger.LogInformation("Employee created successfully");
                    return RedirectToAction(nameof(Index)); // Redirect to index
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating employee");
                    TempData["error"] = "Error occurred while creating employee"; // Set error message
                }
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", employee.DepartmentId); // Populate Department dropdown
            return View(employee); // Return the create view with employee data
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Employee ID was null in Edit");
                return NotFound(); // Return 404 if ID is null
            }

            var employee = await _context.Employees.FindAsync(id); // Find employee by ID
            if (employee == null)
            {
                _logger.LogWarning($"Employee not found with ID {id}");
                return NotFound(); // Return 404 if employee is not found
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", employee.DepartmentId); // Populate Department dropdown
            return View(employee); // Return the edit view
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                _logger.LogWarning($"Employee ID mismatch in Edit: {id} != {employee.Id}");
                return NotFound(); // Return 404 if ID mismatch
            }
            employee.UpdatedDate = DateTime.UtcNow; // Set the updated date
            ModelState.Remove("Department"); // Remove Department validation

            if (ModelState.IsValid) // Check if model state is valid
            {
                try
                {
                    _context.Update(employee); // Update employee in context
                    await _context.SaveChangesAsync(); // Save changes to database
                    TempData["success"] = "Employee updated successfully"; // Set success message
                    _logger.LogInformation("Employee updated successfully");
                    return RedirectToAction(nameof(Index)); // Redirect to index
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        _logger.LogWarning($"Employee not found with ID {employee.Id} during update");
                        return NotFound(); // Return 404 if employee does not exist
                    }
                    else
                    {
                        _logger.LogError("Concurrency exception occurred while updating employee");
                        throw; // Throw exception if concurrency error occurs
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating employee");
                    TempData["error"] = "Error occurred while updating employee"; // Set error message
                }
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", employee.DepartmentId); // Populate Department dropdown
            return View(employee); // Return the edit view with employee data
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Employee ID was null in Delete");
                return NotFound(); // Return 404 if ID is null
            }

            var employee = await _context.Employees
                .Include(e => e.Department) // Include Department data
                .FirstOrDefaultAsync(m => m.Id == id); // Find employee by ID
            if (employee == null)
            {
                _logger.LogWarning($"Employee not found with ID {id}");
                return NotFound(); // Return 404 if employee is not found
            }

            return View(employee); // Return the delete view
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id); // Find employee by ID
                if (employee != null)
                {
                    _context.Employees.Remove(employee); // Remove employee from context
                    await _context.SaveChangesAsync(); // Save changes to database
                    TempData["success"] = "Employee deleted successfully"; // Set success message
                    _logger.LogInformation("Employee deleted successfully");
                }
                else
                {
                    _logger.LogWarning($"Employee not found with ID {id} during deletion");
                    TempData["error"] = "Employee not found"; // Set error message
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting employee");
                TempData["error"] = "Error occurred while deleting employee"; // Set error message
            }

            return RedirectToAction(nameof(Index)); // Redirect to index
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id); // Check if employee exists in context
        }
    }
}
