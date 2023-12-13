using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace YzonShop.Forms.Manager
{
    /// <summary>
    /// Логика взаимодействия для AddGoodsWindow.xaml
    /// </summary>
    public partial class AddGoodsWindow : Window
    {
        public AddGoodsWindow()
        {
            InitializeComponent();
        }

        Goods goods = new Goods();

        string imagePath;

        private SQLHelper _sqlHelper = SQLHelper.GetSQLHelper();

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                ImageButton.Content = "Файл..";
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                goods.Name = NameTextBox.Text;
                goods.Description = DescriptionTextBox.Text;
                goods.Firm = FirmTextBox.Text;
                goods.Model = ModelTextBox.Text;

                try
                {
                    goods.Cost = float.Parse(CostTextBox.Text);
                }
                catch (Exception)
                {
                    throw new Exception("Cost должна быть числом");
                }

                try
                {
                    goods.Warranty = Int32.Parse(WarrantyTextBox.Text);
                }
                catch (Exception)
                {
                    throw new Exception("Warranty должен быть целым числом");
                }

                if (imagePath != null)
                {
                    string extension = '.' + imagePath.Split('.')[imagePath.Split('.').Length - 1];
                    Directory.CreateDirectory(Config.ImageSourceDir);
                    var dirs = Directory.GetFiles(Config.ImageSourceDir);
                    string filePath = Config.ImageSourceDir + (dirs.Length + 1).ToString() + extension;
                    goods.Image = filePath;
                    File.Copy(imagePath, filePath, true);
                }

                _sqlHelper.SetGoods(goods);

                this.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
