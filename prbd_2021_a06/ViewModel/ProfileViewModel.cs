using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using prbd_2021_a06.Model;
using prbd_2021_a06.Properties;
using PRBD_Framework;
namespace prbd_2021_a06.ViewModel
{
    public class ProfileViewModel : ViewModelCommon
    {
       
        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Delete { get; set; }
        
        
        private string email;
        public string Email
        {
            get => email;
            set => SetProperty<string>(ref email, value);
        }

        private User user;
        public User User { get => user; set => SetProperty(ref user, value); }

        private string ancien_password;
        public string AncienPassword
        {
            get => ancien_password;
            set => SetProperty<string>(ref ancien_password, value, () => Validate());
        }
        private string password;
        public string Password
        {
            get => password;
            set => SetProperty<string>(ref password, value, () => Validate());
        }

        private string confirm_password;
        public string ConfirmPassword
        {
            get => confirm_password;
            set => SetProperty<string>(ref confirm_password, value, () => Validate());
        }
        
        public ProfileViewModel() : base()
        {
            Save = new RelayCommand(SaveAction, CanSaveAction);
            Cancel = new RelayCommand(CancelAction, CanCancelAction);
            User = App.CurrentUser;
            Email = App.CurrentUser.Email;
          
        }

        
        private void SaveAction()
        {
            if (Validate())
            {
                var user = (from u in App.Context.Users
                            where u.Email.Equals(Email)
                            select u).FirstOrDefault();

                
                user.Password = Password;
                Context.Update(user);
                
            }

            Context.SaveChanges();
            OnRefreshData();
            //NotifyColleagues(AppMessages.MSG_MEMBER_CHANGED, Member);
        }

        private bool CanSaveAction()
        {
           
            return  email != null && password != null && confirm_password != null && !HasErrors;
        }

        private void CancelAction()
        {

            Context.Reload(User);
            RaisePropertyChanged();

        }

        private bool CanCancelAction()
        {
            return User != null /*&& (IsNew || Context?.Entry(Member)?.State == EntityState.Modified)*/;
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
            //else if (user.Email == Email)
            // AddError(nameof(Email), Resources.Error_DoesExist);


            
            if (Password != ConfirmPassword)
                AddError(nameof(ConfirmPassword), Resources.Error_WrongPasswordConfirm);

            if (AncienPassword != user.Password)
                AddError(nameof(AncienPassword), Resources.Error_PasswordDoesNotExist);

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
