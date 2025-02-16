using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HvP.examify_take_exam.DB.Entities
{
    [Table("t_take_exams")]
    public class TakeExamEntity : BaseEntity
    {
        [StringLength(255)]
        public string Name { get; set; }

    }
}
