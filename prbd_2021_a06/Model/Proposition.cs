using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PRBD_Framework;

namespace prbd_2021_a06.Model {
    public class Proposition : EntityBase<Context> {
        [Key]
        public int Id { get; set; }
        public bool IsCorrect { get; set; }
        public string Body { get; set; }
        public virtual Question Question { get; set; }
        public bool IsCheck { get; set; }
        public Proposition()
        {

        }
        public Proposition( string Body, bool IsCorrect)
        {
            
            this.Body = Body;
            this.IsCorrect = IsCorrect;
        }
        [NotMapped]
        public Question GetMany
        {
            get
            {
                return (
                    from  q in App.Context.Questions
                    where q.Type.Equals(Type.Many
                    )
                    select q
                    ).FirstOrDefault();
            }
        }
        [NotMapped]
        public Visibility IsTrue
        {
            get
            {
                if (!this.IsCorrect)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }
        [NotMapped]
        public Visibility OneAnswer
        {
            get
            {
                if (this.Question.Type != Type.One)
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
                if (this.Question.Type != Type.Many)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
            
        }
        public void Delete()
        {
            Context.Propositions.Remove(this);
            Context.SaveChanges();
        }

    }
}
