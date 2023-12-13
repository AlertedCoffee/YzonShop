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

        private List<Model.Order> _orders;

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (((TabControl)sender).SelectedIndex)
                {
                    case 0:
                        GoodsDataGrid.ItemsSource = _sqlHelper.GetGoods();
                        break;
                    case 1:
                        ShopsDataGrid.ItemsSource = _sqlHelper.GetShops();
                        break;
                    case 2:
                        _orders = _sqlHelper.GetOrders();
                        OrdersDataGrid.ItemsSource = _orders;
                        break;
                    default:
                        break;
                }
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
                GoodsDataGrid.ItemsSource = _sqlHelper.GetGoods();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Window window = new AddGoodsWindow();
                window.ShowDialog();

                GoodsDataGrid.ItemsSource = _sqlHelper.GetGoods();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window window = new EditGoodsWindow();
                window.ShowDialog();

                GoodsDataGrid.ItemsSource = _sqlHelper.GetGoods();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshShopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShopsDataGrid.ItemsSource = _sqlHelper.GetShops();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddShopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window window = new AddShopWindow();
                window.ShowDialog();
                ShopsDataGrid.ItemsSource = _sqlHelper.GetShops();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditShopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window window = new EditShopWindow();
                window.ShowDialog();
                ShopsDataGrid.ItemsSource = _sqlHelper.GetShops();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OrdersDataGrid.ItemsSource = _sqlHelper.GetOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void ApplyOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _sqlHelper.ApplyOrder(_orders[Int32.Parse(IDTextBox.Text) - 1]);
                OrdersDataGrid.ItemsSource = _sqlHelper.GetOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
