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
        public QuizzView(Course course)
        {
            InitializeComponent();
            vm.Init(course);
        }

        
        private void QuizzView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
           Quiz q = (Quiz)gridQuizz.SelectedItem;
            
           if (!(App.CurrentUser.IsTeacher)  )
            {
                if (q.IsEnabled)
                {
                    vm.DisplayQuizzStudent.Execute(gridQuizz.SelectedItem);
                }
               
            }
          else 
            {
                vm.DisplayQuizzDetails.Execute(gridQuizz.SelectedItem);
                
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
       

    }
}
