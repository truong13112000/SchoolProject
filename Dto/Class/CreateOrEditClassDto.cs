using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Models;

namespace SchoolProject.Dto.Class
{
    public class CreateOrEditClassDto
    {
        public ClassEntities ClassEntities { get; set; }
        public IEnumerable<SelectListItem>? SchoolList { get; set; }
        public IEnumerable<SelectListItem>? DepartmentList { get; set; }
    }
}
