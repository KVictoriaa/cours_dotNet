using PRBD_Framework;
using prbd_2021_a06.Model;

namespace prbd_2021_a06.ViewModel {
    public abstract class ViewModelCommon : ViewModelBase<Context> {

        public ViewModelCommon() : base() {
        }

        public static bool IsTeacher {
            get => App.IsLoggedIn && App.CurrentUser.IsTeacher;
        }

        public static bool IsNotTeacher {
            get => !IsTeacher;
        }

        public static User CurrentUser {
            get => App.CurrentUser;
        }

        public static void Login(User user) {
            App.Login(user);
        }

        public static void Signup(User user)
        {
            App.Login(user);
        }
        public static void Logout() {
            App.Logout();
        }
        public static Course CurrentCourse
        {
            get => App.CurrentCourse;
        }
        public static bool IsLoggedIn { get => App.IsLoggedIn; }
    }
}