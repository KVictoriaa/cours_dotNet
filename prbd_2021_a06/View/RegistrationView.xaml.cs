using prbd_2021_a06.Model;
using prbd_2021_a06.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prbd_2021_a06.View {
    /// <summary>
    /// Logique d'interaction pour RegistrationView.xaml
    /// </summary>
    public partial class RegistrationView : UserControlBase {

        public RegistrationViewModel VmRegistration { get; set; }

        

        public RegistrationView() {
            InitializeComponent();
            VmRegistration = new RegistrationViewModel();
            DataContext = this;
        }



        public static readonly DependencyProperty CourseProperty =
           DependencyProperty.Register(" Course", typeof(Course), typeof(RegistrationView), new
              PropertyMetadata(null, new PropertyChangedCallback(OnCoursePropertyChanged)));

   
        public Course Course {
            get { return (Course)GetValue(CourseProperty); }
            set { SetValue(CourseProperty, value); }
        }

        private static void OnCoursePropertyChanged(DependencyObject d,
        DependencyPropertyChangedEventArgs e) {
            RegistrationView UserControl1Control = d as RegistrationView;
            UserControl1Control.OnCoursePropertyChanged(e);
        }

        private void OnCoursePropertyChanged(DependencyPropertyChangedEventArgs e) {
            //todo
            var course = (Course)e.NewValue;

            VmRegistration.CourseId = course.Id;

        }

        private void btnAction_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            StudentCourseHelper model = b.CommandParameter as StudentCourseHelper;
            // MessageBox.Show(model.StudentName);
            VmRegistration.ChangeStatusCommand.Execute(model.StudentCourseId);
        }

        private void btndoubleright_Click(object sender, RoutedEventArgs e) {
            VmRegistration.OnPermute(true);
        }

        private void btnright_Click(object sender, RoutedEventArgs e) {
            btndoubleright_Click(sender, e);
        }

        private void btndoubleleft_Click(object sender, RoutedEventArgs e) {
            VmRegistration.OnPermute(false);
        }

        private void btnleft_Click(object sender, RoutedEventArgs e) {
            btndoubleleft_Click(sender,  e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvUserUnregister_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            VmRegistration.UnRegisterStudentCoursesSelected.Clear();
            ListView b = sender as ListView;
            var models = b.SelectedItems;
            foreach (var model in models) {
                var sch = model as StudentHelper;
                VmRegistration.UnRegisterStudentCoursesSelected.Add(sch);
            }

            if (models == null || models.Count == 0 && b.SelectedItem != null) {
                VmRegistration.UnRegisterStudentCoursesSelected.Add(b.SelectedItem as StudentHelper);
            }

            VmRegistration.RefrechLeft();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvUserregister_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            VmRegistration.RegisterStudentCoursesSelected.Clear();
            ListView b = sender as ListView;
            var models = b.SelectedItems;
            if(models == null || models.Count == 0 && b.SelectedItem !=null) {
                VmRegistration.RegisterStudentCoursesSelected.Add(b.SelectedItem as StudentCourseHelper);
            }
            foreach(var model in models) {
                var sch = model as StudentCourseHelper;
                //MessageBox.Show(sch.StudentName);
                VmRegistration.RegisterStudentCoursesSelected.Add(sch);
            }
            VmRegistration.RefrechRight();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            VmRegistration.ClearFilterCommand.Execute(null);
        }
    }
}
