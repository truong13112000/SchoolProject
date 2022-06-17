using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Dto;
using SchoolProject.Models;
using SchoolProject.ValidateService;
using System.Reflection;
using System.Resources;

namespace SchoolProject.Controllers
{
    public class UserEntitiesController : Controller
    {
        private readonly DataContext _context;
        private readonly ResourceManager _rm;
        private readonly IValidateService _validateServiceClass;

        public UserEntitiesController(DataContext context,
                    IValidateService validateServiceClass
            )
        {
            _context = context;
            _rm = new ResourceManager("SchoolProject.ResourceManager.ResourceLanguage", Assembly.GetExecutingAssembly());
            _validateServiceClass = validateServiceClass;
        }

        // GET: UserEntities
        public async Task<IActionResult> Index()
        {
            var userList = from u in _context.UserEntitiess.AsNoTracking().Where(e => e.UserName.ToLower() != "admin")
                           join s in _context.SchoolEntitiess.AsNoTracking() on u.SchoolId equals s.Id into schoolJoin
                           from s in schoolJoin.DefaultIfEmpty()
                           join d in _context.DepartmentEntitiess.AsNoTracking() on u.DepartmentId equals d.Id into departmentJoin
                           from d in departmentJoin.DefaultIfEmpty()
                           join c in _context.ClassEntitiess.AsNoTracking() on u.ClassId equals c.Id into classJoin
                           from c in classJoin.DefaultIfEmpty()
                           join r in _context.Role.AsNoTracking() on u.Role equals r.Id into roleJoin
                           from r in roleJoin.DefaultIfEmpty()
                           select new UserListDto
                           {
                               Id = u.Id,
                               UserName = u.UserName,
                               FullName = u.FullName,
                               Email = u.Email,
                               BirthDay = u.BirthDay.ToString("dd/MM/yyyy"),
                               Address = u.Address,
                               School = s.SchoolName,
                               Department = d.DepartmentName,
                               Class = c.ClassName,
                               Role = r.RoleName,
                           };
            return userList != null ?
                          View(await userList.ToListAsync()) :
                          Problem("UserList is null.");
        }

        // GET: UserEntities/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.UserEntitiess == null)
            {
                return NotFound();
            }

            var userEntities = await _context.UserEntitiess
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userEntities == null)
            {
                return NotFound();
            }

            return View(userEntities);
        }
        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: UserEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Password,FullName,Email,BirthDay,Address,SchoolId,DepartmentId,ClassId,Role,IsDeleted,DeleterUserId,DeletionTime,LastModificationTime,LastModifierUserId,CreationTime,CreatorUserId,Id")] UserEntities userEntities)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userEntities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userEntities);
        }

        // GET: UserEntities/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            SignUpDto userEntities = new SignUpDto();
            userEntities.SchoolList = _context.SchoolEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.SchoolName }).ToList();
            userEntities.DepartmentList = _context.DepartmentEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.DepartmentName }).ToList();
            userEntities.ClassList = _context.ClassEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.ClassName }).ToList();
            userEntities.RoleList = _context.Role.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.RoleName }).ToList();
            userEntities.UserEntities = await _context.UserEntitiess.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

            return View(userEntities);
        }

        // POST: UserEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UserName,FullName,Email,BirthDay,Address,SchoolId,DepartmentId,ClassId,Role,CreationTime,CreatorUserId,Id")] UserEntities userEntities)
        {

            SignUpDto user = new SignUpDto();
            user.UserEntities = userEntities;
            user.SchoolList = _context.SchoolEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.SchoolName }).ToList();
            user.DepartmentList = _context.DepartmentEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.DepartmentName }).ToList();
            user.ClassList = _context.ClassEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.ClassName }).ToList();
            user.RoleList = _context.Role.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.RoleName }).ToList();

            var editUser = _context.UserEntitiess.FirstOrDefault(e => e.Id == id);

            if (!ModelState.IsValid)
            {
                var a = 1;
                return View(user);
            }


            var tesst = _validateServiceClass?.ValidateUpdateUser(id, userEntities);
            if (tesst != 0)
            {
                switch (tesst)
                {
                    case 1:
                        ViewBag.error = _rm.GetString("UserNameExisted");
                        break;
                    case 2:
                        ViewBag.error = _rm.GetString("EmailExisted");
                        break;

                }
                return View(user);
            }
            try
            {
                editUser.FullName = userEntities.FullName;
                editUser.UserName = userEntities.UserName;
                editUser.Email = userEntities.Email;
                editUser.BirthDay = userEntities.BirthDay;
                editUser.SchoolId = userEntities.SchoolId;
                editUser.DepartmentId = userEntities.DepartmentId;
                editUser.ClassId = userEntities.ClassId;
                editUser.Role = userEntities.Role;
                _context.UserEntitiess.Update(editUser);
                await _context.SaveChangesAsync();
            }
            catch (ArgumentException Exception)
            {
                throw Exception;
            }

            if (HttpContext.Session.GetInt32("Role") != 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: UserEntities/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.UserEntitiess == null)
            {
                return NotFound();
            }

            var userEntities = await _context.UserEntitiess
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userEntities == null)
            {
                return NotFound();
            }

            return View(userEntities);
        }

        // POST: UserEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.UserEntitiess == null)
            {
                return Problem("Entity set 'DataContext.UserEntitiess'  is null.");
            }
            var userEntities = await _context.UserEntitiess.FindAsync(id);
            if (userEntities != null)
            {
                _context.UserEntitiess.Remove(userEntities);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
