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
        public MainViewModelTeacher() : base()
        {
            LogoutCommand = new RelayCommand(LogoutAction);
            Register<Course>(this, AppContext.MSG_DISPLAY_COURSE, course =>
              {
                  DisplayCourse?.Invoke(course, false);
              });
            Register(this, AppContext.MSG_NEW_COURSE, () => {
                // crée une nouvelle instance pour un nouveau course "vide"
                var course = App.Context.CreateCourse("", "",0);
                // demande à la vue de créer dynamiquement un nouvel onglet avec le titre "<new member>"
                DisplayCourse?.Invoke(course, true);
            });

        }
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
        
    }
}
