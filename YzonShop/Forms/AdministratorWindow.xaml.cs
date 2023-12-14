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
using SQLHelperLib;
using SQLHelperLib.Model;

namespace YzonShop.Forms
{
    /// <summary>
    /// Логика взаимодействия для Administrator.xaml
    /// </summary>
    public partial class AdministratorWindow : Window
    {
        public AdministratorWindow()
        {
            InitializeComponent();
        }

        private SQLHelper _sqlHelper;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                _sqlHelper = SQLHelper.GetSQLHelper();

                DataGrid.ItemsSource = _sqlHelper.GetAuthList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                DataGrid.ItemsSource = _sqlHelper.GetSortedAuthList(LoginTextBox.Text, DatePikachu.SelectedDate == null ? DateTime.MinValue : (DateTime)DatePikachu.SelectedDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
