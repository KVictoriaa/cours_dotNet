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
using PRBD_Framework;
using prbd_2021_a06.Properties;
using prbd_2021_a06.Model;

namespace prbd_2021_a06.View
{
    /// <summary>
    /// Logique d'interaction pour MainViewStudent.xaml
    /// </summary>
    public partial class MainViewStudent : WindowBase
    {
        public MainViewStudent() 
        {
            InitializeComponent();
        }
        private void Vm_OnLogout()
        {
            App.NavigateTo<LoginView>();
        }
        private void Vm_DiplayCourse(Course course, bool isNew)
        {
            if (course != null)
            {
                var tab = tabControl.FindByTag(course.Title);
                if (tab == null)
                {
                    tabControl.Add(
                        new CourseViewDetails(course, isNew),
                        isNew ? "<new Course>" : course.Title
                     );
                }
                else
                {
                    tabControl.SetFocus(tab);
                }
            }
        }
    }
}
