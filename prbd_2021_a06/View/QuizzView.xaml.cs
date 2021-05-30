using prbd_2021_a06.Model;
using PRBD_Framework;
using System;
using System.Windows;
using System.Windows.Input;

namespace prbd_2021_a06.View
{
    /// <summary>
    /// Logique d'interaction pour QuizzView.xaml
    /// </summary>
    public partial class QuizzView : UserControlBase
    {
        public QuizzView()
        {
            InitializeComponent();
        }

        /*private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // vm.DisplayMemberDetails.Execute(listView.SelectedItem);
            Console.WriteLine("Hello World!");
        }*/

        private void QuizzView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Quiz q = (Quiz)gridQuizz.SelectedItem;
           if(App.CurrentUser.IsTeacher)
            {
               
                vm.DisplayQuizzDetails.Execute(gridQuizz.SelectedItem);
            }
          else
            {
                
                vm.DisplayQuizzStudent.Execute(gridQuizz.SelectedItem);
            }
            

        }

        public static readonly DependencyProperty CourseProperty =
            DependencyProperty.Register(
                nameof(Course),                 // nom de la propriété
                typeof(Course),                 // type associé à la propriété
                typeof(QuizzView),     // type "propriétaire" qui déclare la propriété
                new PropertyMetadata(null)      // métadonnées associées qui définissent la valeur par défaut (ici null)
            );

        public Course Course
        {
            get { return (Course)GetValue(CourseProperty); }
            set { SetValue(CourseProperty, value); }
        }
        public Visibility AnswerQuizz
        {
            get
            {
                if (!(App.CurrentUser.IsTeacher) && (Course.GetQuizBytime.Debut > DateTime.Now))
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

    }
}
