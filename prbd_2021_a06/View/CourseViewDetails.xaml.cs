using prbd_2021_a06.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace prbd_2021_a06.View
{
    /// <summary>
    /// Logique d'interaction pour CourseViewDetails.xaml
    /// </summary>
    public partial class CourseViewDetails : UserControlBase
    {
       
        

        public CourseViewDetails( Course course , bool isNew)
        {
            InitializeComponent();
            vm.Init(course, isNew);
            vr.Course = course;
            vc.Course = course;
            //vq.Course = course;
            if(App.CurrentUser.IsTeacher)
            {
            var tab = tabControl.FindByTag(course.Title);
            if (tab == null)
                tabControl.Add(
                    new QuestionMakerView(course),
                    "Questions"
                 );
            }
            

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void QuizzView_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
