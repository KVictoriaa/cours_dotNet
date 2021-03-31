using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;
using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace prbd_2021_a06.Model
{
    public enum Type {
        One,Many
    }
    public class Question : EntityBase<Context>
    {

        [Key]
        public int Id { get; set; }
        public string Enonce { get; set; }
        public Type Type { get; set; }
        public virtual ICollection<Proposition> Propositions { get; set; } = new HashSet<Proposition>();
        public virtual ICollection<CategoryQuestion> CategoryQuestions { get; set; } = new HashSet<CategoryQuestion>();
        public virtual ICollection<QuestionQuiz> QuestionQuizzes { get; set; } = new HashSet<QuestionQuiz>();

        [NotMapped]
        public IEnumerable<Category> Categories { get => CategoryQuestions.Select(sc => sc.Categories); }
        [NotMapped]
        public IEnumerable<Quiz> Quizzes { get => QuestionQuizzes.Select(sc => sc.Quiz); }

    }
}
