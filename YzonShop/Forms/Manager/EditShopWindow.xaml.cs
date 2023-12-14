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

namespace YzonShop.Forms.Manager
{
    /// <summary>
    /// Логика взаимодействия для EditShopWindow.xaml
    /// </summary>
    public partial class EditShopWindow : Window
    {
        public EditShopWindow()
        {
            InitializeComponent();
        }

        SQLHelper _sqlHelper;
        private List<Shop> _shopItems;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _sqlHelper = SQLHelper.GetSQLHelper();
                _shopItems = _sqlHelper.GetShops();
                IDCombobox.ItemsSource = _shopItems;
                IDCombobox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IDCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmailTextBox.Text = _shopItems[IDCombobox.SelectedIndex].EmailAddress;
            DeliverPayCheckBox.IsChecked = _shopItems[IDCombobox.SelectedIndex].DeliverPay;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Shop shop = _shopItems[IDCombobox.SelectedIndex];

                shop.EmailAddress = EmailTextBox.Text;
                shop.DeliverPay = (bool)DeliverPayCheckBox.IsChecked;

                _sqlHelper.UpdateShop(shop);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _sqlHelper.DeleteShop(_shopItems[IDCombobox.SelectedIndex].Id);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
