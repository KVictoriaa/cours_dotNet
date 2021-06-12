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
        public DateTime Debut { get; set; } = DateTime.Today;
        public DateTime Fin { get; set; } = DateTime.Today;
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
        [NotMapped]
        public bool IsEnabled
        {
            get
            {


                var studentCourse = (from u in App.Context.StudentCourses
                                     where u.Student.Equals(App.CurrentUser) && u.Course.Equals(Course)
                                     select u).FirstOrDefault();

                var anwers = (from a in App.Context.AnswerQuestions
                              where a.StudentCourse.Id == studentCourse.Id 
                              select a).Select(a => a.QuestionQuiz.Quiz).ToList();
                Console.WriteLine(!anwers.Contains(this));
                Console.WriteLine( Fin < DateTime.Now);
                return  (Fin < DateTime.Now && anwers.Contains(this)) || (DateTime.Now < Fin );

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
