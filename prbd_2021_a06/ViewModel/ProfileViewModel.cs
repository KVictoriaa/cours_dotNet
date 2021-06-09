using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using prbd_2021_a06.Model;
using PRBD_Framework;
namespace prbd_2021_a06.ViewModel
{
    public class ProfileViewModel : ViewModelCommon
    {
        public event Func<string> OnLoadImage;
        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Delete { get; set; }
        public ICommand LoadImage { get; set; }
        public ICommand ClearImage { get; set; }

        private ImageHelper imageHelper;
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
            set => SetProperty<string>(ref ancien_password, value);
        }
        private string password;
        public string Password
        {
            get => password;
            set => SetProperty<string>(ref password, value);
        }

        private string confirm_password;
        public string ConfirmPassword
        {
            get => confirm_password;
            set => SetProperty<string>(ref confirm_password, value);
        }
        public ProfileViewModel() : base()
        {
            Save = new RelayCommand(SaveAction, CanSaveAction);
            Cancel = new RelayCommand(CancelAction, CanCancelAction);
            Delete = new RelayCommand(DeleteAction/*, () => !IsNew*/);
            LoadImage = new RelayCommand(LoadImageAction);
            ClearImage = new RelayCommand(ClearImageAction, () => PicturePath != null);
            Console.WriteLine(App.CurrentUser);
            User = App.CurrentUser;
            imageHelper = new ImageHelper(App.IMAGE_PATH, User.PicturePath);
        }
        public string PicturePath
        {
            get { return User?.AbsolutePicturePath; }
            set
            {
                User.PicturePath = value;
                RaisePropertyChanged(nameof(PicturePath));
            }
        }
        private void SaveAction()
        {
            //if (IsNew)
            //{
            //    // Un petit raccourci ;-)
            //    Member.Password = Member.Pseudo;
            //    Context.Add(Member);
            //    //IsNew = false;
            //}
            imageHelper.Confirm(User.Email);
            PicturePath = imageHelper.CurrentFile;
            Context.SaveChanges();
            OnRefreshData();
            //NotifyColleagues(AppMessages.MSG_MEMBER_CHANGED, Member);
        }

        private bool CanSaveAction()
        {
            //if (IsNew)
            //    return !string.IsNullOrEmpty(Pseudo);
            return User != null && (Context?.Entry(User)?.State == EntityState.Modified);
        }

        private void CancelAction()
        {
            if (imageHelper.IsTransitoryState)
            {
                imageHelper.Cancel();
            }
            
            else
            {
                Context.Reload(User);
                RaisePropertyChanged();
            }
        }

        private bool CanCancelAction()
        {
            return User != null /*&& (IsNew || Context?.Entry(Member)?.State == EntityState.Modified)*/;
        }

        private void LoadImageAction()
        {
            var res = OnLoadImage?.Invoke();
            if (res != null)
            {
                imageHelper.Load(res);
                PicturePath = imageHelper.CurrentFile;
            }
        }

        private void ClearImageAction()
        {
            imageHelper.Clear();
            PicturePath = imageHelper.CurrentFile;
        }

        private void DeleteAction()
        {
            CancelAction();
            if (File.Exists(PicturePath))
                File.Delete(PicturePath);
            //User.Delete();
            //NotifyColleagues(AppMessages.MSG_MEMBER_CHANGED, Member);
            //NotifyColleagues(AppMessages.MSG_CLOSE_TAB, Member);
        }
        public override void Dispose()
        {
            if (imageHelper.IsTransitoryState)
                imageHelper.Cancel();
            // Ne pas oublier sinon il continue à recevoir les notifications et risque de planter !
            
            base.Dispose();
        }
        protected override void OnRefreshData()
        {
            
        }
    }
}
