using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PRBD_Framework;

namespace prbd_2021_a06.Model
{
    public class QuestionQuiz : EntityBase<Context>
    {
        [Key]
        public int Id { get; set; }
        public double Point { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual Question Question { get; set; }
        [NotMapped]
        public virtual AnswerQuestions AnswerQuestions { get; set; }
        public QuestionQuiz()
        {

        }


    }
}
