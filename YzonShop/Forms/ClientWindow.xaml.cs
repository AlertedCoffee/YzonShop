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
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }
        private int _userId;
        private SQLHelper _sqlHelper;
        private List<Goods> _goods;
        private int _index;

        private void GoodsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _index = ((DataGrid)sender).SelectedIndex;
            if (_index >= 0)
            {
                DescriptionTextBlock.Text = _goods[_index].Description;
                try
                {
                    GoodsImage.Source = new BitmapImage(new Uri(@"pack://siteoforigin:,,," + _goods[_index].Image, UriKind.RelativeOrAbsolute));
                }
                catch (Exception)
                {
                    GoodsImage.Source = null;
                }
            }
            else
            {
                DescriptionTextBlock.Text = "";
                GoodsImage.Source = null;
            }

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {    
                _sqlHelper = SQLHelper.GetSQLHelper();
                _goods = _sqlHelper.GetGoods();
                GoodsDataGrid.ItemsSource = _goods;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _goods = _sqlHelper.GetSortedGoods(NameTextBox.Text, ModelTextBox.Text);
                GoodsDataGrid.ItemsSource = _goods;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_index == -1) throw new Exception("Товар не выбран");
                _sqlHelper.AddOrder(_goods[_index].Id, 1, 1, _userId);
                MessageBox.Show("Оформлен!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
