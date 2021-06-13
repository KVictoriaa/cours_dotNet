using System;
using System.Linq;
using System.Windows;
using PRBD_Framework;
using prbd_2021_a06.Model;
using System.Threading;
using System.Globalization;
using prbd_2021_a06.Properties;

namespace prbd_2021_a06
{ 
    public enum AppContext{ 
        MSG_DISPLAY_COURSE,
        MSG_NEW_COURSE,
        MSG_TITLECOURSE_CHANGED,
        MSG_COURSE_CHANGED,
        MSG_CLOSE_TAB,
        MSG_COURSES,
        MSG_RENAME_TAB,
        MSG_REFRESH_QUESTIONS,
        MSG_DISPLAY_QUIZZ,
        MSG_TITLEQUIZZ_CHANGED,
        MSG_NEW_Quizz,
        MSG_RENAMEQuizz_TAB,
        MSG_Quizz_CHANGED,
        MSG_Quizz,
        MSG_DISPLAY_QUIZZSTUDENT,
        MSG_CLOSE_TABQUIZZ,
        MSG_REFRESH_CATEGORY,
        MSG_CLOSE_TABQUIZZ_ETUDIANT,
        MSG_QUESTIONQUIZZ_CHANGED,
        MSG_QUIZZ_CHANGED,
        MSG_QUIZZ

    }
    public partial class App : ApplicationBase {
        public static Context Context { get => Context<Context>(); }

        public static User CurrentUser { get; private set; }
        public static Course CurrentCourse { get; private set; }

        public static void Login(User user)
        {
            CurrentUser = user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
        

        public static bool IsLoggedIn { get => CurrentUser != null; }

        public App()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Culture);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
            Context.SeedData();
        }

        protected override void OnRefreshData()
        {
            throw new NotImplementedException();
        }
        
    }
}
