using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PRBD_Framework;

namespace prbd_2021_a06.Model
{
    public class Quiz : EntityBase<Context>
    {
        [Key]
        public int Id { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public string Title {get;set; }
        public virtual ICollection<QuestionQuiz> QuestionQuizzes { get; set; } = new HashSet<QuestionQuiz>();
        [NotMapped]
        public IEnumerable<Question> Questions { get => QuestionQuizzes.Select(sc => sc.Question); }
        public virtual Course Course { get; set; }


        public Quiz() { }
        public Quiz(string Title,DateTime Debut, DateTime Fin)
        {
            this.Title = Title;
            this.Debut = Debut;
            this.Fin = Fin;
            //this.Course = Course;
        }
        [NotMapped]
        public int NbQuestions
        {
            get
            {
                return (
                     from s in QuestionQuizzes
                     where s.Quiz.Id.Equals(this.Id)
                     select s
                     ).Count();
            }
        }
        public Question GetQuestion(Quiz observer)
        {
             return  Context.QuestionQuizzes.Where(q => q.Quiz == observer).Select(q => q.Question).FirstOrDefault();
           
        }
        

        public void AddQuestionToQuiz(Question Question, int Points)
        {
            QuestionQuiz NewQuestion = new QuestionQuiz();
            NewQuestion.Point = Points;
            NewQuestion.Question = Question;
            NewQuestion.Quiz = this;
            QuestionQuizzes.Add(NewQuestion);
            Context.SaveChanges();
        }

        public void RemoveQuestionToQuiz(QuestionQuiz Question)
        {
            QuestionQuizzes.Remove(Question);
            Context.SaveChanges();
        }
        public void Delete()
        {
            Context.Quizzes.Remove(this);
            Context.SaveChanges();
        }
        public Visibility PermissionAddQuizz
        {
            get
            {
                if (!(App.CurrentUser.IsTeacher))
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }
        
        public Visibility AnswerQuizz
        {
            get
            {
                if (!(App.CurrentUser.IsTeacher) && (Debut > DateTime.Now))
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }
        public override string ToString()
        {
            //string str = "Cours : ";
           // str += this.Title + " Cours:  " +  this.Course.Title + " Début : " + this.Debut + " Fin : " + this.Fin;
            return this.Title;
        }
    }
}
