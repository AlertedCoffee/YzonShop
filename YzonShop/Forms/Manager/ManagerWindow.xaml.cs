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
using YzonShop.Forms.Manager;

namespace YzonShop.Forms
{
    /// <summary>
    /// Логика взаимодействия для Manager.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private SQLHelper _sqlHelper;
        public ManagerWindow()
        {
            InitializeComponent();
            _sqlHelper = SQLHelper.GetSQLHelper();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((TabControl)sender).SelectedIndex)
            {
                case 0:
                    GoodsDataGrid.ItemsSource = _sqlHelper.GetGoods();
                    break;
                default:
                    break;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            GoodsDataGrid.ItemsSource = _sqlHelper.GetGoods();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new AddGoodsWindow();
            window.ShowDialog();
            
            GoodsDataGrid.ItemsSource = _sqlHelper.GetGoods();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new EditGoodsWindow();
            window.ShowDialog();

            GoodsDataGrid.ItemsSource = _sqlHelper.GetGoods();
        }
    }
}
