using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gymbokning_2.Data;
using Gymbokning_2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Gymbokning_2.Models.ViewModels;

namespace Gymbokning_2.Controllers
{
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManger;


        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            userManger = userManager;
            
        }

        // GET: GymClasses
        public async Task<IActionResult> Index()
        {
            var userId = userManger.GetUserId(User);
            
           // return _context.GymClass != null ?

                var gymClasses= await _context.GymClasses
                .Include(g => g.AttendingMembers)
                .Select(g => new GymClassesViewModel
                {
                Id=g.Id,
                Name=g.Name,
                Duration=g.Duration,
                Description=g.Description,
                StartDate=g.StartDate,
                Attending =g.AttendingMembers
                .Any (a=> a.ApplicationUserId==userId)  
                }).ToListAsync();



            return View(gymClasses);
                
                
                
                // View(await _context.GymClass.ToListAsync()) :
           // View(await _context.GymClasses.Include(g => g.AttendingMembers).ThenInclude(a => a.ApplicationUser).ToListAsync()) :

            //            Problem("Entity set 'ApplicationDbContext.GymClass'  is null.");
        }









        [Authorize]
        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GymClasses == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses.Include(g => g.AttendingMembers).ThenInclude(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }
        [Authorize]
        // GET: GymClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GymClasses == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
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
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GymClasses == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GymClasses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GymClass'  is null.");
            }
            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass != null)
            {
                _context.GymClasses.Remove(gymClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
            return (_context.GymClasses?.Any(e => e.Id == id)).GetValueOrDefault();
        }




        [Authorize]
        public async Task<IActionResult> BookingToggle(int? id)
        {
            if (id == null) return NotFound();

            // var user = await userManger.GetUserAsync(User);
            var userId = userManger.GetUserId(User);
            if (userId == null) return BadRequest();
            //1
            //var att = _context.ApplicationUserGyms.FindAsync(userId, id);

            //2
            var att = await _context.ApplicationUserGyms.FirstOrDefaultAsync(a => a.ApplicationUserId == userId && a.GymClassId == id);

            //3
            var pass = await _context.GymClasses.Include(g => g.AttendingMembers).FirstOrDefaultAsync(a => a.Id == id);
            //Add null check
            if (pass == null) return NotFound();


            var attending = pass.AttendingMembers.FirstOrDefault(a => a.ApplicationUserId == userId);

            if (attending is null)
            {
                // skapa ny ApplicationUserGymClass med GymClassId=id och ApplicationUserId=userid

                ApplicationUserGymClass applicationUserGymClass = new ApplicationUserGymClass
                {
                    ApplicationUserId = userId,
                    GymClassId = (int)id
                };
                _context.ApplicationUserGyms.Add(applicationUserGymClass);

            }
            else
            {
                // ta bort ApplicationUserGymClass med GymClassId=id och ApplicationUserId=userid
                _context.ApplicationUserGyms.Remove(attending);


            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));







        }
    }
}
