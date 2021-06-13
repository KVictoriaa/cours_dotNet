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
using Microsoft.Win32;
using prbd_2021_a06.Model;
using PRBD_Framework;

namespace prbd_2021_a06.View
{
    /// <summary>
    /// Logique d'interaction pour ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControlBase
    {
        public ProfileView()
        {
            InitializeComponent();
           
        }
        private string Vm_OnLoadImage()
        {
            var fd = new OpenFileDialog();
            if (fd.ShowDialog() == true)
            {
                Console.WriteLine("hello");
                return fd.FileName;
            }
            return null;
        }
    }
}
