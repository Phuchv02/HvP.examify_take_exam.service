using HvP.examify_take_exam.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HvP.examify_take_exam.DB.Models
{
    public class CreateTakeExamModel
    {
        public string? Name { get; set; }

        public TakeExamEntity PlainToEntity()
        {
            TakeExamEntity entity = new TakeExamEntity();

            entity.Name = Name ?? "";

            return entity;
        }
    }
}
