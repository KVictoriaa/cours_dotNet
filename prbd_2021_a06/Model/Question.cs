using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;
using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;

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
        public virtual Course Course { get; set; }
        public virtual ICollection<Proposition> Propositions { get; set; } = new HashSet<Proposition>();
        public virtual ICollection<CategoryQuestion> CategoryQuestions { get; set; } = new HashSet<CategoryQuestion>();
        public virtual ICollection<QuestionQuiz> QuestionQuizzes { get; set; } = new HashSet<QuestionQuiz>();

        [NotMapped]
        public IEnumerable<Category> Categories { get => CategoryQuestions.Select(sc => sc.Category); }
        [NotMapped]
        public IEnumerable<Quiz> Quizzes { get => QuestionQuizzes.Select(sc => sc.Quiz); }
        [NotMapped]
        public IEnumerable<Question> QuestionDate { get => QuestionQuizzes.Where(q=> q.Quiz.Debut < DateTime.Now).Select(sc => sc.Question); }
        [NotMapped]
        public IEnumerable<Proposition> CorrectPropos { get => Propositions.Where(sc => sc.IsCorrect); }

        public Question (
            string Enonce,
            Type Type
            )
        {
            this.Enonce = Enonce;
            this.Type = Type;

        }
        
        
        public Question() { }
        [NotMapped]
        public Visibility OneAnswer
        {
            get
            {
                if (this.Type != Type.One)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }
        [NotMapped]
        public Visibility ManyAnswer
        {
            get
            {
                if (this.Type != Type.Many)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }
        [NotMapped]
        public Visibility CourseQuestion
        {
            get
            {
                if (this.Course.Id != Course.Id)
                {

                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        public override string ToString()
        {
            return Enonce ;
        }

        [NotMapped]
        private bool editMode = false;
        [NotMapped]
        public bool EditMode
        {
            get { return editMode; }
            set
            {
                editMode = value;
                //RaisePropertyChanged(nameof(EditMode), nameof(ReadMode));
            }
        }
        [NotMapped]
        public bool ReadMode
        {
            get { return !EditMode; }
            set { EditMode = !value; }
        }
        public void Delete()
        {
            foreach (var questionQuiz in QuestionQuizzes)
            {
                Context.QuestionQuizzes.Remove(questionQuiz);
            }
            Context.Questions.Remove(this);
            Context.SaveChanges();
        }
        [NotMapped]
        public bool IsEnabled
        {
            get
            {
                return !QuestionDate.Contains(this);
            }
        }
        /* public static IQueryable<Question> GetAll(Course course)
         {
             return Context.Messages.OrderByDescending(m => m.DateTime);
             return (
                     from s in Context.Questions
                     where s.Course.Equals(course)

                     select s
                     ).FirstOrDefault();
         }*/
    }
}
