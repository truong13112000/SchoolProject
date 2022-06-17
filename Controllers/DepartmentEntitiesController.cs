using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Models;
using SchoolProject.ValidateService;
using System.Reflection;
using System.Resources;

namespace SchoolProject.Controllers
{
    public class DepartmentEntitiesController : Controller
    {
        private readonly DataContext _context;
        private readonly ResourceManager _rm;
        private readonly IValidateService _validateServiceClass;


        public DepartmentEntitiesController(DataContext context, IValidateService validateServiceClass)
        {
            _context = context;
            _rm = new ResourceManager("SchoolProject.ResourceManager.ResourceLanguage", Assembly.GetExecutingAssembly());
            _validateServiceClass = validateServiceClass;
        }

        // GET: DepartmentEntities
        public async Task<IActionResult> Index()
        {
            return _context.DepartmentEntitiess != null ?
                        View(await _context.DepartmentEntitiess.ToListAsync()) :
                        Problem("Department is null.");
        }

        // GET: DepartmentEntities/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.DepartmentEntitiess == null)
            {
                return NotFound();
            }

            var departmentEntities = await _context.DepartmentEntitiess
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departmentEntities == null)
            {
                return NotFound();
            }

            return View(departmentEntities);
        }

        // GET: DepartmentEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DepartmentEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentName,Capacity,SchoolId,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Id")] DepartmentEntities departmentEntities)
        {
            if (!_validateServiceClass.ValidateCreateDeparment(departmentEntities))
            {
                ViewBag.error = _rm.GetString("DepartmentExisted");
                return View();
            };
            if (ModelState.IsValid)
            {
                departmentEntities.CreationTime = DateTime.Now;
                _context.Add(departmentEntities);
                await _context.SaveChangesAsync();
                ViewBag.success = _rm.GetString("CreateDepartment");
                return RedirectToAction(nameof(Index));
            }
            return View(departmentEntities);
        }

        // GET: DepartmentEntities/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.DepartmentEntitiess == null)
            {
                return NotFound();
            }

            var departmentEntities = await _context.DepartmentEntitiess.FindAsync(id);
            if (departmentEntities == null)
            {
                return NotFound();
            }
            return View(departmentEntities);
        }

        // POST: DepartmentEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("DepartmentName,Capacity,SchoolId,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Id")] DepartmentEntities departmentEntities)
        {
            if (!_validateServiceClass.ValidateUpdateDeparment(id, departmentEntities))
            {
                ViewBag.error = _rm.GetString("DepartmentExisted");
                return View();
            };
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departmentEntities);
                    await _context.SaveChangesAsync();
                    ViewBag.success = _rm.GetString("UpdateDepartment");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentEntitiesExists(departmentEntities.Id))
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
            return View(departmentEntities);
        }

        // GET: DepartmentEntities/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.DepartmentEntitiess == null)
            {
                return NotFound();
            }

            var departmentEntities = await _context.DepartmentEntitiess
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departmentEntities == null)
            {
                return NotFound();
            }

            return View(departmentEntities);
        }

        // POST: DepartmentEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.DepartmentEntitiess == null)
            {
                return Problem(" Class  is null.");
            }
            var departmentEntities = await _context.DepartmentEntitiess.FindAsync(id);
            if (departmentEntities != null)
            {
                _context.DepartmentEntitiess.Remove(departmentEntities);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentEntitiesExists(long id)
        {
            return (_context.DepartmentEntitiess?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
