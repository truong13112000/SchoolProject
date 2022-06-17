using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Dto;
using SchoolProject.Models;
using SchoolProject.ValidateService;
using System.Reflection;
using System.Resources;

namespace SchoolProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;
        private readonly ResourceManager _rm;
        private readonly IValidateService _validateServiceClass;

        public HomeController(ILogger<HomeController> logger,
            DataContext context,
            IValidateService validateServiceClass
        )
        {
            _logger = logger;
            _context = context;
            _rm = new ResourceManager("SchoolProject.ResourceManager.ResourceLanguage", Assembly.GetExecutingAssembly());
            _validateServiceClass = validateServiceClass;
        }


        public ActionResult Index()
        {
            ViewBag.success = _rm.GetString("LoginSuccess");
            return View();
        }

        public ActionResult SignUp()
        {
            SignUpDto newUser = new SignUpDto();
            newUser.SchoolList = _context.SchoolEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.SchoolName }).ToList();
            newUser.DepartmentList = _context.DepartmentEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.DepartmentName }).ToList();
            newUser.ClassList = _context.ClassEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.ClassName }).ToList();
            newUser.RoleList = _context.Role.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.RoleName }).ToList();
            return View(newUser);
        }


        [HttpPost]
        public ActionResult SignUp(SignUpDto input)
        {
            SignUpDto newUser = new SignUpDto();
            newUser.SchoolList = _context.SchoolEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.SchoolName }).ToList();
            newUser.DepartmentList = _context.DepartmentEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.DepartmentName }).ToList();
            newUser.ClassList = _context.ClassEntitiess.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.ClassName }).ToList();
            newUser.RoleList = _context.Role.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.RoleName }).ToList();
            var tesst = _validateServiceClass?.ValidateSigUp(input);
            if (tesst != 0)
            {
                switch (tesst)
                {
                    case 1:
                        ViewBag.error = _rm.GetString("MaxSchool");
                        break;
                    case 2:
                        ViewBag.error = _rm.GetString("MaxDepartment");
                        break;
                    case 3:
                        ViewBag.error = _rm.GetString("MaxClass");
                        break;
                    case 4:
                        ViewBag.errorEmail = _rm.GetString("EmailExisted");
                        break;
                    case 5:
                        ViewBag.error = _rm.GetString("AccountExisted");
                        break;
                }
                return View(newUser);
            }
            else
            {
                input.UserEntities.CreationTime = DateTime.Now;
                input.UserEntities.Password = HashCodeAndDecodePassWord.HashCodePassword(input.UserEntities.Password);
                _context.UserEntitiess.AddAsync(input.UserEntities);
                _context.SaveChangesAsync();
                return RedirectToAction("LogIn");
            }
        }

        public ActionResult LogIn()
        {
            return View();
        }

        public ActionResult ChangeSignUp()
        {
            return Redirect("SignUp");
        }
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LogIn");
        }
        public ActionResult Cancel()
        {
            return RedirectToAction("LogIn");
        }

        public ActionResult EditInfo()
        {
            var id = HttpContext.Session.GetInt32("Id").ToString();
            return RedirectToAction("Edit", "UserEntities", new { id = id });
        }

        [HttpPost]
        public ActionResult LogIn(LoginDto input)
        {
            var userExisted = _context.UserEntitiess.FirstOrDefault(e => (e.UserName.ToLower().Trim() == input.UserName.ToLower().Trim()) && (e.Password == HashCodeAndDecodePassWord.HashCodePassword(input.Password)));
            if (_validateServiceClass?.ValidateLogin(input) == false)
            {
                ViewBag.error = _rm.GetString("LoginFail");
                return View();
            }
            else
            {
                HttpContext.Session.SetString("UserName", userExisted.UserName);
                HttpContext.Session.SetInt32("Id", (int)userExisted.Id);
                HttpContext.Session.SetInt32("Role", (int)userExisted.Role);
                return RedirectToAction("Index");
            }
        }
    }
}