using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
namespace prbd_2021_a06.Model
{
    public class AnswerQuestions : EntityBase<Context>
    {
        [Key]
        public int Id { get; set; }
        public virtual StudentCourse StudentCourse { get; set; }
        public virtual Proposition Proposition { get; set; }
        public virtual QuestionQuiz QuestionQuiz { get; set; }

        public AnswerQuestions(StudentCourse StudentCourse,
            Proposition Proposition,
            QuestionQuiz QuestionQuiz)
        {
            this.QuestionQuiz = QuestionQuiz;
            this.Proposition = Proposition;
            this.StudentCourse = StudentCourse;
        }

        public AnswerQuestions()
        {

        }
    }
}
