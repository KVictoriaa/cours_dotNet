using prbd_2021_a06.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using prbd_2021_a06.Properties;

namespace prbd_2021_a06.View
{
    /// <summary>
    /// Logique d'interaction pour QuestionMakerView.xaml
    /// </summary>
    public partial class QuestionMakerView : UserControlBase
    {
        

        public QuestionMakerView(Course course)
        {
            InitializeComponent();
            vm.Init(course);
        }


        //public Course Course
        //{
        //    get { return (Course)GetValue(CourseProperty); }
        //    set { SetValue(CourseProperty, value); }
            
        //}



       /* private void button_Click(object sender, RoutedEventArgs e)

        {

            TextBox txt = new TextBox();

            txt.Height = 50;

            txt.Width = 50;

            txt.VerticalAlignment = VerticalAlignment.Top;

            txt.HorizontalAlignment = HorizontalAlignment.Left;

            txt.Name = "Test";

            txt.Text = "Test";

            txt.Visibility = Visibility.Visible;

        }*/
    }
}
