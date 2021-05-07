using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace prbd_2021_a06.Model {
    public enum IsValide { True}
    public enum IsActif { False }
    public class StudentCourse : EntityBase<Context> {
        [Key]
        public int Id { get; set; }
        public Boolean IsValide { get; set; }
        public Boolean IsActif { get; set; }
        public virtual User Student { get; set; }
        public virtual Course Course { get; set; }

        public virtual ICollection<AnswerQuestions> AnswerQuestions { get; set; } = new HashSet<AnswerQuestions>();
        public virtual ICollection<Note> Notes { get; set; } = new HashSet<Note>();


        public StudentCourse (
            )
        {
        }
        
    }
}
