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
    public partial class App : ApplicationBase {
        public static Context Context { get => Context<Context>(); }

        public static User CurrentUser { get; private set; }

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
            //Context.Database.EnsureDeleted();
            //Context.Database.EnsureCreated();
            //Context.SeedData();
        }

        protected override void OnRefreshData()
        {
            throw new NotImplementedException();
        }
        
    }
}
