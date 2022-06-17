using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Models
{
    [Table("ClassEntities")]
    public class ClassEntities : FullAuditedEntity<long>, IEntity<long>
    {
        public string ClassName { get; set; }
        public int Capacity { get; set; }
        public long? DepartmentId { get; set; }
        public long? SchoolId { get; set; }
    }
}
