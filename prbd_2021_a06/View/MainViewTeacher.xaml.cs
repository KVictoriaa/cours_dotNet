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
    /// Logique d'interaction pour MainViewTeacher.xaml
    /// </summary>
    public partial class MainViewTeacher : WindowBase
    {
        public MainViewTeacher()
        {
            InitializeComponent();
        }
        private void Vm_OnLogout()
        {
            App.NavigateTo<LoginView>();
        }
        private void Vm_DiplayCourse(Course course, bool isNew) 
        { 
            if(course != null)
            {
                var tab = tabControl.FindByTag(course.Title);
                if (isNew)
                    tab = tabControl.FindByTag("<new Course>");
                if (tab == null)
                    tabControl.Add(
                        new CourseViewDetails(course, isNew),
                        isNew ? "<new Course>" : course.Title
                     ) ;
                
                else
                {
                    tabControl.SetFocus(tab);
                }
            }
        }
        private void Vm_DiplayQuizz(Quiz quizz, bool isNew)
        {
            
            if (quizz != null)
            {
                Console.WriteLine(quizz);
               
                var tab = tabControl.FindByTag(quizz.Title);
                if (isNew)
                    tab = tabControl.FindByTag("<new Quizz>");
                if (tab == null)
                    tabControl.Add(
                        new QuizMakerView(quizz, isNew),
                        isNew ? "<new Quizz>" : quizz.Title
                     );
                
                else
                {
                    tabControl.SetFocus(tab);
                }
            }
        }
       

        private void Vm_CloseTab(Course course)
        {
            var tab = tabControl.FindByTag(course.Title);
            tabControl.Items.Remove(tab);
        }
        private void Vm_CloseTabQuiz(Quiz quiz)
        {
            var tab = tabControl.FindByTag(quiz.Title);
            tabControl.Items.Remove(tab);
        }

        private void Vm_RenameTab(Course course , string Title)
        {
            var tab = tabControl.SelectedItem as TabItem;
            if(tab != null)
            {
                tab.Header = tab.Tag =Title = string.IsNullOrEmpty(Title) ? "new Course" : Title;
            }
        }
        private void Vm_RenameTabQuizz(Quiz quizz, string Title)
        {
            var tab = tabControl.SelectedItem as TabItem;
            if (tab != null)
            {
                tab.Header = tab.Tag = Title = string.IsNullOrEmpty(Title) ? "new Quizz" : Title;
            }
        }
    }
}
