using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace prbd_2021_a06.Model
{
    public class CategoryQuestion : EntityBase<Context>
    { 
        [Key]
        public int Id { get; set; }
        public virtual Question Question { get; set; }
        public virtual Category Category { get; set; } 
        public CategoryQuestion ()
        {

        }
        public static IQueryable<CategoryQuestion> GetFilteredCategories(string Filter)
        {
            var filtered = from c in Context.CategoryQuestions
                           where c.Category.Title.Contains(Filter)
                           orderby c.Category.Title
                           select c;
            return filtered;
        } 
        public static IQueryable<CategoryQuestion> GetAll()
        {
            return Context.CategoryQuestions.OrderBy(m => m.Category.Title);
        }
    }

   
}
