using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Models
{
    [Table("SchoolEntities")]
    public class SchoolEntities: FullAuditedEntity<long> ,IEntity<long>
    {
        public string SchoolName { get; set; }
        public DateTime FoundedTime { get; set; }
        public int Capacity { get; set; }
        [StringLength(20)]
        public string? Address { get; set; }
    }
}
