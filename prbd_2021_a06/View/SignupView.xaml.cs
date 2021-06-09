using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PRBD_Framework;

namespace prbd_2021_a06.View
{
    /// <summary>
    /// Logique d'interaction pour signupView.xaml
    /// </summary>
    public partial class SignupView : WindowBase
    {
        public SignupView()
        {
            InitializeComponent();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Vm_OnLogin()
        {
            
            App.NavigateTo<LoginView>();
        }

        private void Vm_OnSignupSuccess()
        {
            App.NavigateTo < MainViewStudent > ();
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtEmail.SelectAll();
        }
        private void txtFirstName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtNom.SelectAll();
        }
        private void txtLastName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPrenom.SelectAll();
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPassword.SelectAll();
        }
        private void txtConfirmPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txtConfirmPassword.SelectAll();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
