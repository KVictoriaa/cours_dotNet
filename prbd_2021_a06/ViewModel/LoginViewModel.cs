using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using prbd_2021_a06.Properties;
using PRBD_Framework;

namespace prbd_2021_a06.ViewModel {
    public class LoginViewModel : ViewModelCommon {
        public event Action OnLoginSuccess;
        public event Action OnSignup;


        public ICommand LoginCommand { get; set; }

        public ICommand SignupCommand { get; set; }
        public ICommand LoginTeacher { get; set; }
        public ICommand LoginStudent { get; set; }
        private String email;
        private string emailT = "benoit@epfc.eu";
        private string emailS = "hello@bonjour";
        public string Email { get => email; set => SetProperty<string>(ref email, value, () => Validate()); }
        public string EmailT { get => emailT; set => SetProperty<string>(ref emailT, value, () => Validate()); }
        public string EmailS { get => emailS; set => SetProperty<string>(ref emailS, value, () => Validate()); }
        private string password;
        // private string passwordT= "Password1,";
        // private string passwordS = "Bonjour1";
        public string Password { get => password; set => SetProperty<string>(ref password, value, () => Validate()); }
        // public string PasswordT { get => passwordT; set => SetProperty<string>(ref passwordT, value, () => Validate()); }
        //public string PasswordS { get => passwordS; set => SetProperty<string>(ref passwordS, value, () => Validate()); }
        public LoginViewModel() : base() {
            LoginCommand = new RelayCommand(
                LoginAction,
                () => { return email != null && password != null && !HasErrors; }
            );
            SignupCommand = new RelayCommand(
                SignupAction,
                () => { return true; });

            LoginTeacher = new RelayCommand(
                LoginActionTeacher,
                () => { return true; }
            );
            LoginStudent = new RelayCommand(
                LoginActionStudent,
                () => { return true; }
            );
        }

        private void LoginActionStudent() {
            //if (Validate()) {

                var user = (from u in App.Context.Users
                            where u.Email.Equals(EmailS)
                            select u).FirstOrDefault();
                Login(user);
                OnLoginSuccess?.Invoke();
            //}
        }

        private void LoginActionTeacher() {
            //if (Validate()) {

                var user = (from u in App.Context.Users
                            where u.Email.Equals(EmailT)
                            select u).FirstOrDefault();
                Login(user);
                OnLoginSuccess?.Invoke();
            //}
        }

        private void LoginAction() {
            if (Validate()) {
                var user = (from u in App.Context.Users
                            where u.Email.Equals(Email)
                            select u).FirstOrDefault();
                Login(user);
                OnLoginSuccess?.Invoke();
            }
        }

        private void SignupAction() {
            OnSignup.Invoke();

        }

        public override bool Validate() {
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
             
            else {
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

        protected override void OnRefreshData() {
        }
    }
}


