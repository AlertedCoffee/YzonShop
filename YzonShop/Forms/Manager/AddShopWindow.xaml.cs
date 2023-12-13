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

namespace YzonShop.Forms.Manager
{
    /// <summary>
    /// Логика взаимодействия для AddShopWindow.xaml
    /// </summary>
    public partial class AddShopWindow : Window
    {
        public AddShopWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SQLHelper sqlHelper = SQLHelper.GetSQLHelper();

                sqlHelper.SetShop(new Model.Shop(0, EmailTextBox.Text, (bool)DeliverPayCheckBox.IsChecked));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
