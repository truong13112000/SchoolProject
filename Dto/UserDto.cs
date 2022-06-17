using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolProject.Dto
{
    public class UserDto
    {
        public UserListDto userList { get; set; }
        public IEnumerable<SelectListItem> SchoolList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> ClassList { get; set; }
    }
}
