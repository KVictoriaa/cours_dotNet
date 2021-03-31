using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PRBD_Framework;


namespace prbd_2021_a06.ViewModel
{
    public class MainViewModelStudent : ViewModelCommon
    {
        public event Action OnLogout;

        public ICommand LogoutCommand { get; set; }
        public MainViewModelStudent() : base()
        {
            Console.WriteLine("Student");
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
