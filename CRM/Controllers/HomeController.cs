using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CRM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        // Constructor for HomeController, initializes the logger and database context
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        // Action for the home page
        public IActionResult Index()
        {
            // Log the Index action call
            _logger.LogInformation("Index action called");

            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("User is not authenticated, redirecting to login page");
                return Redirect("~/identity/account/login"); // Redirect to login page if not authenticated
            }

            // Count the number of visitors, employees, appointments, and clients
            var visitorCount = _context.Visitors.Count();
            var employeeCount = _context.Employees.Count();
            var appointmentCount = _context.Appointments.Count();
            var clientsCount = _context.Clients.Count();

            // Store the counts in ViewData
            // Store the counts in ViewBag
            ViewBag.VisitorCount = visitorCount;
            ViewBag.EmployeeCount = employeeCount;
            ViewBag.AppointmentCount = appointmentCount;
            ViewBag.ClientsCount = clientsCount;

            // Log the counts
            _logger.LogInformation("Visitor Count: {Count}", visitorCount);
            _logger.LogInformation("Employee Count: {Count}", employeeCount);
            _logger.LogInformation("Appointment Count: {Count}", appointmentCount);
            _logger.LogInformation("Client Count: {Count}", clientsCount);

            return View(); // Return the home view
        }

        // Action for the privacy page
        public IActionResult Privacy()
        {
            _logger.LogInformation("Privacy action called");
            return View(); // Return the privacy view
        }

        // Action for handling errors
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("Error action called");

            // Return the error view with the request ID
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
