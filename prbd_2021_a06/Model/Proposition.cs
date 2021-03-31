using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace prbd_2021_a06.Model {
    public class Proposition : EntityBase<Context> {
        [Key]
        public int Id { get; set; }
        public Boolean IsCorrect { get; set; }
        public string Body { get; set; }
        public virtual Question Question { get; set; }
        ///public virtual AnswerQuestions AnswerQuestion { get; set; }
    }
}
