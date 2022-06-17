using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Models
{
    [Table("DepartmentEntities")]
    public class DepartmentEntities: FullAuditedEntity<long>, IEntity<long>
    {
        public string DepartmentName { get; set; }
        public int Capacity { get; set; }
        public long? SchoolId { get; set; }
    }
}
