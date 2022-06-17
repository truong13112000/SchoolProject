using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Dto.Class;
using SchoolProject.Models;
using SchoolProject.ValidateService;
using System.Reflection;
using System.Resources;

namespace SchoolProject.Controllers
{
    public class ClassEntitiesController : Controller
    {
        private readonly DataContext _context;
        private readonly ResourceManager _rm;
        private readonly IValidateService _validateServiceClass;

        public ClassEntitiesController(DataContext context, IValidateService validateServiceClass)
        {
            _context = context;
            _rm = new ResourceManager("SchoolProject.ResourceManager.ResourceLanguage", Assembly.GetExecutingAssembly());
            _validateServiceClass = validateServiceClass;
        }

        // GET: ClassEntities
        public async Task<IActionResult> Index()
        {
            var result = from c in _context.ClassEntitiess.AsNoTracking()
                         join d in _context.DepartmentEntitiess on c.DepartmentId equals d.Id into deparmentJoin
                         from d in deparmentJoin.DefaultIfEmpty()
                         join s in _context.SchoolEntitiess on c.SchoolId equals s.Id into schoolJoin
                         from s in schoolJoin.DefaultIfEmpty()
                         select new ClassViewDto
                         {
                             Id = c.Id,
                             ClassName = c.ClassName,
                             Department = d.DepartmentName,
                             Capacity = c.Capacity,
                             School = s.SchoolName,
                         };

            return result != null ?
                        View(await result.ToListAsync()) :
                        Problem("Class is empty.");
        }

        // GET: ClassEntities/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.ClassEntitiess == null)
            {
                return NotFound();
            }
            var classEntities = (from c in _context.ClassEntitiess.AsNoTracking().Where(m => m.Id == id)
                                 join d in _context.DepartmentEntitiess on c.DepartmentId equals d.Id into deparmentJoin
                                 from d in deparmentJoin.DefaultIfEmpty()
                                 join s in _context.SchoolEntitiess on c.SchoolId equals s.Id into schoolJoin
                                 from s in schoolJoin.DefaultIfEmpty()
                                 select new ClassViewDto
                                 {
                                     Id = c.Id,
                                     ClassName = c.ClassName,
                                     Department = d.DepartmentName,
                                     Capacity = c.Capacity,
                                     School = s.SchoolName,
                                 }).FirstOrDefault();


            if (classEntities == null)
            {
                return NotFound();
            }

            return View(classEntities);
        }

        // GET: ClassEntities/Create
        public IActionResult Create()
        {
            CreateOrEditClassDto newClass = new CreateOrEditClassDto();
            newClass.SchoolList = _context.SchoolEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.SchoolName }).ToList();
            newClass.DepartmentList = _context.DepartmentEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.DepartmentName }).ToList();
            return View(newClass);
        }

        // POST: ClassEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassName,Capacity,DepartmentId,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Id,SchoolId")] ClassEntities classEntities)
        {
            CreateOrEditClassDto newClass = new CreateOrEditClassDto();
            newClass.ClassEntities = classEntities;
            newClass.SchoolList = _context.SchoolEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.SchoolName }).ToList();
            newClass.DepartmentList = _context.DepartmentEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.DepartmentName }).ToList();

            var departmentCapacity = _context.DepartmentEntitiess.FirstOrDefault(e => e.Id == classEntities.DepartmentId).Capacity;
            var classCapacity = _context.ClassEntitiess.Where(e => e.DepartmentId == classEntities.DepartmentId).Sum(e => e.Capacity) + classEntities.Capacity;
            var availability = departmentCapacity - (classCapacity - classEntities.Capacity);
            var validate = _validateServiceClass.ValidateCreateClass(classEntities);
            if (validate != 0)
            {
                switch (validate)
                {
                    case 1:
                        ViewBag.error = _rm.GetString("DepartmentOver") + availability.ToString();
                        break;
                    case 2:
                        ViewBag.error = _rm.GetString("ClassExisted");
                        break;
                }
                return View(newClass);
            }
            if (ModelState.IsValid)
            {
                newClass.ClassEntities.CreationTime = DateTime.Now;
                _context.Add(newClass.ClassEntities);
                await _context.SaveChangesAsync();
                ViewBag.success = _rm.GetString("CreateClass");
                return View(newClass);
            }
            return View(newClass);
        }

        // GET: ClassEntities/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            CreateOrEditClassDto newClass = new CreateOrEditClassDto();
            newClass.SchoolList = _context.SchoolEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.SchoolName }).ToList();
            newClass.DepartmentList = _context.DepartmentEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.DepartmentName }).ToList();

            if (id == null || _context.ClassEntitiess == null)
            {
                return NotFound();
            }

            newClass.ClassEntities = await _context.ClassEntitiess.FindAsync(id);
            if (newClass.ClassEntities == null)
            {
                return NotFound();
            }
            return View(newClass);
        }

        // POST: ClassEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ClassName,Capacity,DepartmentId,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Id,SchoolId")] ClassEntities classEntities)
        {
            CreateOrEditClassDto newClass = new CreateOrEditClassDto();
            newClass.ClassEntities = classEntities;
            newClass.DepartmentList = _context.DepartmentEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.DepartmentName }).ToList();
            newClass.SchoolList = _context.SchoolEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.SchoolName }).ToList();
            var departmentCapacity = _context.DepartmentEntitiess.FirstOrDefault(e => e.Id == classEntities.DepartmentId).Capacity;
            var classCapacity = _context.ClassEntitiess.Where(e => e.DepartmentId == classEntities.DepartmentId && e.Id != id).Sum(e => e.Capacity) + classEntities.Capacity;
            var availability = departmentCapacity - (classCapacity - classEntities.Capacity);
            var editClass = _context.ClassEntitiess.FirstOrDefault(e => e.Id == id);
            var validate = _validateServiceClass.ValidateUpdateClass(id,classEntities);
            if (validate != 0)
            {
                switch (validate)
                {
                    case 1:
                        ViewBag.error = _rm.GetString("ClassExisted");
                        break;
                    case 2:
                        ViewBag.error = _rm.GetString("DepartmentOver") + availability.ToString();
                        break;
                }
                return View(newClass);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    editClass.ClassName = classEntities.ClassName;
                    editClass.Capacity = classEntities.Capacity;
                    editClass.SchoolId = classEntities.SchoolId;
                    editClass.DepartmentId = classEntities.DepartmentId;
                    _context.ClassEntitiess.Update(editClass);
                    await _context.SaveChangesAsync();
                    ViewBag.success = _rm.GetString("UpdateClass");
                }
                catch (DbUpdateConcurrencyException error)
                {
                    throw error;
                }
                return View(newClass);
            }
            return View(newClass);
        }

        // GET: ClassEntities/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.ClassEntitiess == null)
            {
                return NotFound();
            }

            var classEntities = await _context.ClassEntitiess
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classEntities == null)
            {
                return NotFound();
            }

            return View(classEntities);
        }

        // POST: ClassEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.ClassEntitiess == null)
            {
                return Problem("Class is null.");
            }
            var classEntities = await _context.ClassEntitiess.FindAsync(id);
            if (classEntities != null)
            {
                _context.ClassEntitiess.Remove(classEntities);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassEntitiesExists(long id)
        {
            return (_context.ClassEntitiess?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
