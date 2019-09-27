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
using MailSender_Pattern_MVVM.ViewModels;

namespace MailSender_Pattern_MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для NotifyWindow.xaml
    /// </summary>
    public partial class WarningWindow : Window
    {
        public WarningWindow()
        {
            InitializeComponent();
            DataContext = MainViewModel.GetInstance(nameof(MainViewModel));
        }

        private void Button_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
