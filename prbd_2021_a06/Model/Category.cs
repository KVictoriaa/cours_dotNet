using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2021_a06.Model {
    public class Category: EntityBase<Context> {

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<CategoryQuestion> CategoryQuestions { get; set; } = new HashSet<CategoryQuestion>();
        [NotMapped]
        public IEnumerable<Question> Questions { get => CategoryQuestions.Select(sc => sc.Questions); }

        public Category (String title) { this.Title = title;  }


        public void AddNew() {
            Context.Categories.Add(this);
            Context.SaveChanges();
        }

        public void Remove() {
            Context.Categories.Remove(this);
            Context.SaveChanges();
        }



    }
}
