using prbd_2021_a06.Model;
using PRBD_Framework;
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

namespace prbd_2021_a06.View
{
    /// <summary>
    /// Logique d'interaction pour GradeView.xaml
    /// </summary>
    public partial class GradeView : UserControlBase
    {
        public GradeView(Course course)
        {
            InitializeComponent();
            vm.Init(course);

        }
    }
}
