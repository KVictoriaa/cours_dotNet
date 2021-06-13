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
    public class MainViewModelStudent : ViewModelCommon
    {
        public event Action OnLogout;
        public event Action<Course, bool> DisplayCourse;
        public event Action<Quiz, bool> DiplayQuizzStudent;
        public event Action<Quiz> CloseTabQuizStudent;

        public ICommand LogoutCommand { get; set; }
        public MainViewModelStudent() : base()
        {
            LogoutCommand = new RelayCommand(LogoutAction);
            Register<Course>(this, AppContext.MSG_DISPLAY_COURSE, course =>
            {
                DisplayCourse?.Invoke(course, false);
            });
            Register<Quiz>(this, AppContext.MSG_DISPLAY_QUIZZSTUDENT, quiz =>
            {

                DiplayQuizzStudent?.Invoke(quiz, false);

            });
            Register<Quiz>(this, AppContext.MSG_CLOSE_TABQUIZZ_ETUDIANT, quizz =>
            {
                CloseTabQuizStudent?.Invoke(quizz);
            });

        }
        private void LogoutAction()
        {
            Logout();
            OnLogout?.Invoke();
        }
        protected override void OnRefreshData()
        {
            
        }
        public string Title
        {
            get => $"My School ({CurrentUser.FirstName + " - " +CurrentUser.Role})";
        }
    }
}
