using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using prbd_2021_a06.Properties;
using PRBD_Framework;

namespace prbd_2021_a06.ViewModel
{
    public class LoginViewModel : ViewModelCommon
    {
        public event Action OnLoginSuccess;
        public event Action OnSignup;

        public ICommand LoginCommand { get; set; }

        public ICommand SignupCommand { get; set; }

        private string email;
        public string Email { get => email; set => SetProperty<string>(ref email, value, () => Validate()); }

        private string password;
        public string Password { get => password; set => SetProperty<string>(ref password, value, () => Validate()); }

        public LoginViewModel() : base()
        {
            LoginCommand = new RelayCommand(
                LoginAction,
                () => { return email != null && password != null && !HasErrors; }
            );
            SignupCommand = new RelayCommand(
                SignupAction,
                () => { return true; });
        }

        private void LoginAction()
        {
            if (Validate())
            {
                var user = (from u in App.Context.Users
                            where u.Email.Equals(Email)
                            select u).FirstOrDefault();
                Login(user);
                OnLoginSuccess?.Invoke();
            }
        }

        private void SignupAction() 
        {
            OnSignup.Invoke();

        }

        public override bool Validate()
        {
            ClearErrors();

            var user = (from u in App.Context.Users
                        where u.Email.Equals(Email)
                        select u).FirstOrDefault();
           
            if (user != null)
               
            if (string.IsNullOrEmpty(Email))
                AddError(nameof(Email), Resources.Error_Required);
            else if (Email.Length < 3)
                AddError(nameof(Email), Resources.Error_LengthGreaterEqual3);
            else if (user == null)
                AddError(nameof(Email), Resources.Error_DoesNotExist);
            else
            {
                if (string.IsNullOrEmpty(Password))
                    AddError(nameof(Password), Resources.Error_Required);
                else if (Password.Length < 3)
                    AddError(nameof(Password), Resources.Error_LengthGreaterEqual3);
                else if (user != null && user.Password != Password)
                    AddError(nameof(Password), Resources.Error_WrongPassword);
            }

            RaiseErrors();
            return !HasErrors;
        }

        protected override void OnRefreshData()
        {
        }
    }
}

