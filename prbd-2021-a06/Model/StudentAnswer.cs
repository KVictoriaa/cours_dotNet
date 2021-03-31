using System;
using System.Collections.Generic;
using System.Text;

namespace prbd_2021_a06.Model
{
    class StudentAnswer
    {
        private Student student;
        private Question question;
        List<Answer> lstanswers;
        public DateTime dateTime = DateTime.Now;
    }
}
