using CRM.Data;
using CRM.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(ApplicationDbContext context, ILogger<ClientsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all clients");
            return View(await _context.Clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Client ID was null in Details");
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                _logger.LogWarning($"Client not found with ID {id}");
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            client.CreatedDate = DateTime.UtcNow;
            client.DateRegistered = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(client);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Client created successfully";
                    _logger.LogInformation("Client created successfully");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating client");
                    TempData["error"] = "Error occurred while creating client";
                }
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Client ID was null in Edit");
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                _logger.LogWarning($"Client not found with ID {id}");
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id)
            {
                _logger.LogWarning($"Client ID mismatch in Edit: {id} != {client.Id}");
                return NotFound();
            }

            client.UpdatedDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Client updated successfully";
                    _logger.LogInformation("Client updated successfully");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        _logger.LogWarning($"Client not found with ID {client.Id} during update");
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Concurrency exception occurred while updating client");
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating client");
                    TempData["error"] = "Error occurred while updating client";
                }
            }

            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Client ID was null in Delete");
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                _logger.LogWarning($"Client not found with ID {id}");
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var client = await _context.Clients.FindAsync(id);
                if (client != null)
                {
                    _context.Clients.Remove(client);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Client deleted successfully";
                    _logger.LogInformation("Client deleted successfully");
                }
                else
                {
                    _logger.LogWarning($"Client not found with ID {id} during deletion");
                    TempData["error"] = "Client not found";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting client");
                TempData["error"] = "Error occurred while deleting client";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
