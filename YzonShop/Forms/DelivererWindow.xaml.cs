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
using YzonShop.Model;

namespace YzonShop.Forms
{
    /// <summary>
    /// Логика взаимодействия для DelivererWindow.xaml
    /// </summary>
    public partial class DelivererWindow : Window
    {
        public DelivererWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }
        private int _userId;
        private SQLHelper _sqlHelper;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _sqlHelper = SQLHelper.GetSQLHelper();
                DeliverDataGrid.ItemsSource = _sqlHelper.GetDeliver(_userId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DeliverDataGrid.ItemsSource = _sqlHelper.GetDeliver(_userId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
