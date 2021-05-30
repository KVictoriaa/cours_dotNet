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
        public virtual Question Questions { get; set; }
        public virtual Category Categories { get; set; } 
        public CategoryQuestion ()
        {

        }
    }

   
}
