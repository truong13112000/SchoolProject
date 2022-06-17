using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Models
{
    [Table("Role")]
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
