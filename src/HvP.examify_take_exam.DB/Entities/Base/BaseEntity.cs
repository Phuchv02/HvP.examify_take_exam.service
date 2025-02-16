using System.ComponentModel.DataAnnotations;

namespace HvP.examify_take_exam.DB.Entities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? DeletedFlag { get; set; }
    }
}
