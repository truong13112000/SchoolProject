using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Models;
using SchoolProject.ValidateService;
using System.Reflection;
using System.Resources;

namespace SchoolProject.Controllers
{
    public class SchoolEntitiesController : Controller
    {
        private readonly DataContext _context;
        private readonly ResourceManager _rm;
        private readonly IValidateService _validateServiceClass;
        public SchoolEntitiesController(DataContext context, IValidateService validateServiceClass)
        {
            _context = context;
            _rm = new ResourceManager("SchoolProject.ResourceManager.ResourceLanguage", Assembly.GetExecutingAssembly());
            _validateServiceClass = validateServiceClass;
        }

        // GET: SchoolEntities
        public async Task<IActionResult> Index()
        {
            //if (_context.SchoolEntitiess != null)
            //{
            //    _context.SchoolEntitiess.ForEachAsync(e => e.FoundedTime = e.FoundedTime.Date);
            //}
            return _context.SchoolEntitiess != null ?
                          View(await _context.SchoolEntitiess.ToListAsync()) :
                          Problem("School is null.");
        }

        // GET: SchoolEntities/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.SchoolEntitiess == null)
            {
                return NotFound();
            }

            var schoolEntities = await _context.SchoolEntitiess
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schoolEntities == null)
            {
                return NotFound();
            }

            return View(schoolEntities);
        }

        // GET: SchoolEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SchoolEntities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SchoolName,FoundedTime,Capacity,Address,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Id")] SchoolEntities schoolEntities)
        {
            if (!_validateServiceClass.ValidateCreateSchool(schoolEntities))
            {
                ViewBag.error = _rm.GetString("SchoolExisted");
                return View();
            };
            if (ModelState.IsValid)
            {
                schoolEntities.CreationTime = DateTime.Now;
                _context.Add(schoolEntities);
                await _context.SaveChangesAsync();
                ViewBag.success = _rm.GetString("SchoolCreate");
                return RedirectToAction(nameof(Index));
            }
            return View(schoolEntities);
        }

        // GET: SchoolEntities/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.SchoolEntitiess == null)
            {
                return NotFound();
            }

            var schoolEntities = await _context.SchoolEntitiess.FindAsync(id);
            if (schoolEntities == null)
            {
                return NotFound();
            }
            return View(schoolEntities);
        }

        // POST: SchoolEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SchoolName,FoundedTime,Capacity,Address,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Id")] SchoolEntities schoolEntities)
        {
            if (!_validateServiceClass.ValidateUpdateSchool(id,schoolEntities))
            {
                ViewBag.error = _rm.GetString("SchoolExisted");
                return View();
            };

            if (ModelState.IsValid)
            {
                try
                {
                    var newSchoolEntities = _context.SchoolEntitiess.FirstOrDefault(e => e.Id == id);
                    newSchoolEntities.SchoolName = schoolEntities.SchoolName;
                    newSchoolEntities.FoundedTime = schoolEntities.FoundedTime;
                    newSchoolEntities.Capacity = schoolEntities.Capacity;
                    newSchoolEntities.Address = schoolEntities.Address;
                    _context.Update(newSchoolEntities);
                    await _context.SaveChangesAsync();
                    ViewBag.success = _rm.GetString("UpdateSchool");
                }
                catch (DbUpdateConcurrencyException excepion)
                {
                    
                        throw excepion;
                    
                }
                return View(schoolEntities);
            }
            return View(schoolEntities);
        }

        // GET: SchoolEntities/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.SchoolEntitiess == null)
            {
                return NotFound();
            }

            var schoolEntities = await _context.SchoolEntitiess
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schoolEntities == null)
            {
                return NotFound();
            }

            return View(schoolEntities);
        }

        // POST: SchoolEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.SchoolEntitiess == null)
            {
                return Problem("School  is null.");
            }
            var schoolEntities = await _context.SchoolEntitiess.FindAsync(id);
            if (schoolEntities != null)
            {
                _context.SchoolEntitiess.Remove(schoolEntities);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchoolEntitiesExists(long id)
        {
          return (_context.SchoolEntitiess?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
