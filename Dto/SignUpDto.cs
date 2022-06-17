using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Models;

namespace SchoolProject.Dto
{
    public class SignUpDto
    {
        public UserEntities UserEntities { get; set; }
        public IEnumerable<SelectListItem>? SchoolList { get; set; }
        public IEnumerable<SelectListItem>? DepartmentList { get; set; }
        public IEnumerable<SelectListItem>? ClassList { get; set; }
        public IEnumerable<SelectListItem>? RoleList { get; set; }
    }
}
