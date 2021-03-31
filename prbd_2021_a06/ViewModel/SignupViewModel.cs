using prbd_2021_a06.Properties;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace prbd_2021_a06.ViewModel
{
    public class SignupViewModel : ViewModelCommon
    {
        

        private string email;
        public string Email { 
            get => email; 
            set => SetProperty<string>(ref email, value); 
        }

        private string firstName;
        public string FirstName { 
            get => firstName; 
            set => SetProperty<string>(ref firstName, value); 
        }

        private string lastName;
        public string LastName { 
            get => lastName; 
            set => SetProperty<string>(ref lastName, value); 
        }

        private string password;
        public string Password { 
            get => password; 
            set => SetProperty<string>(ref password, value); 
        }

        private string confirm_password;
        public string ConfirmPassword { 
            get => confirm_password; 
            set => SetProperty<string>(ref confirm_password, value); 
        }

        public event Action OnSignupSuccess;
        public event Action OnLogin;
        public ICommand SignupCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        public SignupViewModel() : base()
        {
            SignupCommand = new RelayCommand(
                SignupAction,
                () =>
                {
                    return true;
                }
                    //email != null && firstName != null && lastName != null && password != null && confirm_password != null && !HasErrors; }
            );
            LoginCommand = new RelayCommand(
                LoginAction,
                () => { return true; }
                    );
        }
       
        private void SignupAction()
        {
            //if (Validate())
            Console.WriteLine(Email);
            {
                var user = App.Context.CreateUser(
                    Email , 
                    FirstName, 
                    LastName,
                    Password
                   
                );
                Login(user);
                OnSignupSuccess?.Invoke();
            }
        }
        public void LoginAction()
        {
            OnLogin?.Invoke();
        }
        public override bool Validate()
        {
            ClearErrors();

   

            var user = (from u in App.Context.Users
                         where u.Email.Equals(Email)
                         select u).FirstOrDefault();

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

