using prbd_2021_a06.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using prbd_2021_a06.Properties;

namespace prbd_2021_a06.View
{
    /// <summary>
    /// Logique d'interaction pour QuestionMakerView.xaml
    /// </summary>
    public partial class QuestionMakerView : UserControlBase
    {
        

        public QuestionMakerView(Course course)
        {
            InitializeComponent();
            vm.Init(course);
        }




       private void QestionsByCategory(object sender, RoutedEventArgs e)

        {

            vm.CategoryChecked.Execute(null);

        }
    }
}
