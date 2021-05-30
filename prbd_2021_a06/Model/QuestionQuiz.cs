using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace prbd_2021_a06.Model
{
    public class QuestionQuiz : EntityBase<Context>
    {
        [Key]
        public int Id { get; set; }
        public int Point { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual Question Question { get; set; }


        //public QuestionQuiz(int Points, Question Question, Quiz Quiz)
        //{
        //    this.Point = Points;
        //    this.Question = Question;
        //    this.Quiz = Quiz;
        //}
        
        public QuestionQuiz()
        {

        }


    }
}
