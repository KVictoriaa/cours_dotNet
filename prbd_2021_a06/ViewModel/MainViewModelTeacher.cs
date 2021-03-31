using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;


namespace prbd_2021_a06.ViewModel
{
    public class MainViewModelTeacher : ViewModelCommon
    {
        public MainViewModelTeacher() : base()
        {
            Console.WriteLine("Teacher");
        }
        protected override void OnRefreshData()
        {
            
        }
    }
}
