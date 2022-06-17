using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Models
{
    [Table("UserEntities")]
    public class UserEntities : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public string? Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        [MinimumAge(10,50)]
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public long? SchoolId { get; set; }
        public long? DepartmentId { get; set; }
        public long? ClassId { get; set; }
        [Required]
        public int? Role { get; set; }
    }
}
