using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using PRBD_Framework;

namespace prbd_2021_a06.Model
{
    public class Course : EntityBase<Context> {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int maxStudent { get; set; }
        public virtual User Teacher { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new HashSet<StudentCourse>();
        public virtual ICollection<Quiz> Quizzes { get; set; } = new HashSet<Quiz>();

        [NotMapped]
        public IEnumerable<User> Students { get => StudentCourses.Select(sc => sc.Student); }
        
    }
}
