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
            set => SetProperty<string>(ref email, value, () => Validate());
        }

        private string firstName;
        public string FirstName { 
            get => firstName; 
            set => SetProperty<string>(ref firstName, value, () => Validate());
        }

        private string lastName;
        public string LastName { 
            get => lastName; 
            set => SetProperty<string>(ref lastName, value, () => Validate());
        }

        private string password;
        public string Password { 
            get => password; 
            set => SetProperty<string>(ref password, value, () => Validate()); 
        }

        private string confirm_password;
        public string ConfirmPassword { 
            get => confirm_password; 
            set => SetProperty<string>(ref confirm_password, value, () => Validate());
        }
        private string picturePath;
        public string PicturePath
        {
            get => picturePath = null;
            //set => SetProperty<string>(ref password, value, () => Validate());
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
                    return email != null && firstName != null && lastName != null && password != null && confirm_password != null && !HasErrors;
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
            if (Validate())
           
            {
                var user = App.Context.CreateUser(
                    Email , 
                    FirstName, 
                    LastName,
                    Password,
                    PicturePath 
                   
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
                //else if (user.Email != Email)
                   // AddError(nameof(Email), Resources.Error_DoesExist);
            
            
               if (string.IsNullOrEmpty(FirstName))
                    AddError(nameof(FirstName), Resources.Error_Required);
                else if (FirstName.Length < 3)
                    AddError(nameof(FirstName), Resources.Error_LengthGreaterEqual3);
                
            
                if (string.IsNullOrEmpty(LastName))
                    AddError(nameof(LastName), Resources.Error_Required);
                else if (LastName.Length < 3)
                    AddError(nameof(LastName), Resources.Error_LengthGreaterEqual3);
                
            
                if ( Password != ConfirmPassword)
                    AddError(nameof(ConfirmPassword), Resources.Error_WrongPasswordConfirm);
            

            
                if (string.IsNullOrEmpty(Password))
                    AddError(nameof(Password), Resources.Error_Required);
                else if (Password.Length < 3)
                    AddError(nameof(Password), Resources.Error_LengthGreaterEqual3);
               
            

            RaiseErrors();
            return !HasErrors;
        }



        protected override void OnRefreshData()
        {
        }
    }
}

