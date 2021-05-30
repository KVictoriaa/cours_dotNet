using prbd_2021_a06.Model;
using prbd_2021_a06.ViewModel;
using PRBD_Framework;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prbd_2021_a06.View {
    /// <summary>
    /// Logique d'interaction pour CategoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControlBase {
        public CategoryViewModel VmCategory { get; set; }


        public CategoryView() {
            InitializeComponent();
            VmCategory = new CategoryViewModel();

            DataContext = this;
        }



        public static readonly DependencyProperty CourseProperty =
           DependencyProperty.Register(" Course", typeof(Course), typeof(CategoryView), new
              PropertyMetadata(null, new PropertyChangedCallback(OnCoursePropertyChanged)));


        public Course Course {
            get { return (Course)GetValue(CourseProperty); }
            set { SetValue(CourseProperty, value); }
        }

        private static void OnCoursePropertyChanged(DependencyObject d,
        DependencyPropertyChangedEventArgs e) {
            CategoryView UserControl1Control = d as CategoryView;
            UserControl1Control.OnCoursePropertyChanged(e);
        }

        private void OnCoursePropertyChanged(DependencyPropertyChangedEventArgs e) {
            //todo
            var course = (Course)e.NewValue;

            VmCategory.CourseId = course.Id;
            if (course  != null)
            {
                 //var l = VmCategory.CategoryQuestions.Count();
            }
           

        }

        //private void lvCategory_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        //    ListView b = sender as ListView;
        //    var model = b.SelectedItem as CategoryQuestionsHelper;
        //    MessageBox.Show(model.CategoryName);
        //}

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            VmCategory.RegisterCommand.Execute(null);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            VmCategory.CancelCommand.Execute(null);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            if (lvCategory.SelectedItem != null) {
                var model = lvCategory.SelectedItem as CategoryQuestionsHelper;
                if (model.Id > 0)
                    VmCategory.DeleteCommand.Execute(model.Id);
            } else {
                MessageBox.Show("Veuillez selectionner un élement");
            }

        }
    }
}

