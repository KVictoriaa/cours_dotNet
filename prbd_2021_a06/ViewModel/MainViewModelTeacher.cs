using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using prbd_2021_a06.Model;
using PRBD_Framework;


namespace prbd_2021_a06.ViewModel
{
    public class MainViewModelTeacher : ViewModelCommon
    {
        public event Action<Course, bool> DisplayCourse;
        public event Action<Course> CloseTab;
        public event Action<Quiz> CloseTabQuiz;
        public event Action<Course,string> RenameTab;
        public event Action<Quiz, string> RenameTabQuizz;
        public event Action<Quiz, bool> DisplayQuizz;

        public MainViewModelTeacher() : base()
        {
            LogoutCommand = new RelayCommand(LogoutAction);
            Register<Course>(this, AppContext.MSG_DISPLAY_COURSE, course =>
              {
                  DisplayCourse?.Invoke(course, false);
              });
            Register(this, AppContext.MSG_NEW_COURSE, () => {
                // crée une nouvelle instance pour un nouveau course "vide"
                var course = App.Context.CreateCourse();
                // demande à la vue de créer dynamiquement un nouvel onglet avec le titre "<new member>"
                DisplayCourse?.Invoke(course, true);
            });
            Register<Course>(this, AppContext.MSG_CLOSE_TAB, course =>
              {
                  CloseTab?.Invoke(course);
              });


            Register<Course>(this, AppContext.MSG_RENAME_TAB, course =>
            {
                RenameTab?.Invoke(course,course.Title);
            });
            
            Register<Quiz>(this, AppContext.MSG_DISPLAY_QUIZZ, quiz =>
            {
               
               DisplayQuizz?.Invoke(quiz, false); 
               
            });
            
            Register<Course>(this, AppContext.MSG_NEW_Quizz, course =>
            {
                var quizz = App.Context.CreateQuizz();
                quizz.Course = course;
                DisplayQuizz?.Invoke(quizz, true);
            });

            Register<Quiz>(this, AppContext.MSG_RENAMEQuizz_TAB, quiz =>
            {
                RenameTabQuizz?.Invoke(quiz, quiz.Title);
            });
            //tab quizz etudiant

            Register<Quiz>(this, AppContext.MSG_CLOSE_TABQUIZZ, quizz =>
            {
                CloseTabQuiz?.Invoke(quizz);
            });

            

        }
        //public CourseViewModel CourseViewTeacher { get; private set; } = new CourseViewModel();

        protected override void OnRefreshData()
        {
            
        }
        public event Action OnLogout;

        public ICommand LogoutCommand { get; set; }
        private void LogoutAction()
        {
            Logout();
            OnLogout?.Invoke();
        }
        public string Title
        {
            get => $"My School ({CurrentUser.FirstName + " - " +CurrentUser.Role})";
        }
    }
}
