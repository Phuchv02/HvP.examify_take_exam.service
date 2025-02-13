using System.ComponentModel.DataAnnotations;

namespace HvP.examify_take_exam.DB.Entities
{
    public class TakeExamEntity : BaseEntity
    {
        [StringLength(255)]
        public string Name { get; set; }

    }
}
