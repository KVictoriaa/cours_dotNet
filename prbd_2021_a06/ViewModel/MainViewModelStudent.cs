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
        public ICommand LogoutCommand { get; set; }
        public MainViewModelStudent() : base()
        {
            Register<Course>(this, AppContext.MSG_DISPLAY_COURSE, course =>
            {
                DisplayCourse?.Invoke(course, false);
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
    }
}
