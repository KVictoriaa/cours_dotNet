using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace prbd_2021_a06.Model
{
    public class Note : EntityBase<Context>
    {
        [Key]
        public int Id { get; set; }
        public virtual StudentCourse StudentCourses { get; set; }
        [NotMapped]
        public virtual ICollection<Quiz> Quizzes { get; set; } = new HashSet<Quiz>();


    }
}
