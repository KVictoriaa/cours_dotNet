using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using prbd_2021_a06.Model;
using PRBD_Framework;

namespace prbd_2021_a06.View
{
    /// <summary>
    /// Logique d'interaction pour CourseView.xaml
    ///// </summary>
    public partial class CourseView : UserControlBase
    {
        public CourseView()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Course c = (Course)listView.SelectedItem;
            if (c.GetStudentCourseByUser != null && c.GetStudentCourseByUser.IsValide || App.CurrentUser.IsTeacher)
            {
                vm.DisplayCourseDetails.Execute(listView.SelectedItem);
                Console.WriteLine("on est entré");
            }
            else
            {
                Console.WriteLine("vous n'êtes pas inscrit a ce cours");
            }
           
        }
        
        
    }
    
}
