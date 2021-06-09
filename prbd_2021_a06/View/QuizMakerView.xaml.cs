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
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class QuizMakerView : UserControlBase
    {
        public QuizMakerView(Quiz quizz, bool isNew)
        {
            InitializeComponent();
            vm.Init(quizz, isNew);
            //VmQuizz = new QuizMakerViewModel();
        }
       

        /*public static readonly DependencyProperty CourseProperty =
            DependencyProperty.Register(
                nameof(Course),                 // nom de la propriété
                typeof(Course),                 // type associé à la propriété
                typeof(QuizMaker),     // type "propriétaire" qui déclare la propriété
                new PropertyMetadata(null)      // métadonnées associées qui définissent la valeur par défaut (ici null)
            );

        public Course Course
        {
            get { return (Course)GetValue(CourseProperty); }
            set { SetValue(CourseProperty, value); }
        }*/
    }
}
