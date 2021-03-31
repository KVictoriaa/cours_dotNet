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
   public class Quiz : EntityBase<Context>
    {
        [Key]
        public int Id { get; set; }
        public DateTime debut { get; set; }
        public DateTime fin { get; set; }
        public virtual ICollection<QuestionQuiz> QuestionQuizzes { get; set; } = new HashSet<QuestionQuiz>();
        [NotMapped]
        public IEnumerable<Question> Questions { get => QuestionQuizzes.Select(sc => sc.Question); }
        public virtual Course Course { get; set; }
    }
}
