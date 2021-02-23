using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreeLancers4.Data;
using FreeLancers4.Models;
using Microsoft.AspNetCore.Identity;
using FreeLancers4.Areas.Identity.Data;

namespace FreeLancers4.Views.Projects
{
    public class ProjectsController : Controller
    {
        private readonly FreeLancers4Context _context;
        private readonly UserManager<FreeLancers4User> _userManager;

        public ProjectsController(FreeLancers4Context context, UserManager<FreeLancers4User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Projects
        public async Task<IActionResult> Index(string SearchString, string Skills, DateTime datesearch)
        {
            var pro = from m in _context.Work //checks the list
                      select m;
            if (!String.IsNullOrEmpty(SearchString))//checks whats in the search
            {
                pro = pro.Where(p => p.ProjectTitle.Equals(SearchString));//this filters by the SearchString
            }

            //the search drop down quesry
            IQueryable<string> TypeQuery = from p in _context.Work
                                           orderby p.Skills
                                           select p.Skills;
            //remove duplicates
            IEnumerable<SelectListItem> items = new SelectList(await TypeQuery.Distinct().ToListAsync());

            //attach to the drop down
            ViewBag.Skills = items;

            if(!String.IsNullOrEmpty(datesearch.ToString()))
            {
                pro = pro.Where(p => p.PostDate == datesearch);
            }

            if(!String.IsNullOrEmpty(Skills))
            {
                pro = pro.Where(p => p.Skills.Equals(SearchString));//this filters by the SearchString

            }

            return View(await _context.Work.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Work
                .FirstOrDefaultAsync(m => m.ID == id);
            if (work == null)
            {
                return NotFound();
            }

            return View(work);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectTitle,Description,Skills,Price,DueDate,ContactEmail")] Work work)
        {

            if (!(await _userManager.GetUserAsync(User)).Role.Equals("client"))
            {
                return NotFound();
            }
            
            // Minutes must be at least 10 after now
            if((work.DueDate - DateTime.Now).TotalMinutes < 30)
            {
                ModelState.AddModelError("DueDate", "The time set must be at least 30 after now");
            }

            // Add other missing details during creation of the job
            work.Owner =  await _userManager.GetUserAsync(User);
            work.PostDate = DateTime.Now;
            work.Skills = work.Skills.ToUpper();
            work.WorkStatus = "Unassigned";

            if (ModelState.IsValid)
            {
                _context.Add(work);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(work);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id ,string? projecttitle)
        {
            if (id == null)
            {
                return NotFound();
            }

            FreeLancers4User user = await _userManager.GetUserAsync(User);

            if (!user.Role.Equals("client"))
            {
                return NotFound();
            }

            ViewBag.Text = projecttitle;

            var work = await _context.Work.FindAsync(id);//changes the name at the url place
            if (work == null)
            {
                return NotFound();
            }

            if (!work.Owner.Equals(user))
            {
                return NotFound();
            }

            return View(work);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTitle,Description,Skills,Price,DueDate,ContactEmail")] Work work)
        {
            if (id != work.ID)
            {
                return NotFound();
            }

            FreeLancers4User user = await _userManager.GetUserAsync(User);

            if (!work.Owner.Equals(user))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(work);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkExists(work.ID))
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

            return View(work);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Work
                .FirstOrDefaultAsync(m => m.ID == id);

            if (work == null)
            {
                return NotFound();
            }

            FreeLancers4User user = await _userManager.GetUserAsync(User);

            if (!work.Owner.Equals(user))
            {
                return NotFound();
            }

            return View(work);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var work = await _context.Work.FindAsync(id);

            FreeLancers4User user = await _userManager.GetUserAsync(User);

            if (!work.Owner.Equals(user))
            {
                return NotFound();
            }

            _context.Work.Remove(work);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Accept")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int id)
        {
            var work = await _context.Work.FindAsync(id);

            if (work == null)
            {
                return NotFound();
            }

            FreeLancers4User user = await _userManager.GetUserAsync(User);

            if(work.AssignedTo != null)
            {
                ModelState.AddModelError("Assigned", "The job has already been assigned to someone else.");
            } 
            else
            {
                work.Assigned = user;
                work.WorkStatus = "Assigned";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.History.Add(new History {
                        Project = work,
                    });

                    _context.Update(work);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkExists(work.ID))
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

            return RedirectToAction(nameof(Index));
        }

        private bool WorkExists(int id)
        {
            return _context.Work.Any(e => e.ID == id);
        }
    }
}
