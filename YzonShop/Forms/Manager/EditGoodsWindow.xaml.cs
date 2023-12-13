using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для EditGoodsWindow.xaml
    /// </summary>
    public partial class EditGoodsWindow : Window
    {
        public EditGoodsWindow()
        {
            InitializeComponent();
        }
        SQLHelper _sqlHelper;
        private List<Goods> _goodsItems;
        string imagePath;

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                ImageButton.Content = "Файл..";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _sqlHelper = SQLHelper.GetSQLHelper();
                _goodsItems = _sqlHelper.GetGoods();
                IDCombobox.ItemsSource = _goodsItems;
                IDCombobox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IDCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NameTextBox.Text = _goodsItems[IDCombobox.SelectedIndex].Name;
            FirmTextBox.Text = _goodsItems[IDCombobox.SelectedIndex].Firm;
            ModelTextBox.Text = _goodsItems[IDCombobox.SelectedIndex].Model;
            DescriptionTextBox.Text = _goodsItems[IDCombobox.SelectedIndex].Description;
            CostTextBox.Text = _goodsItems[IDCombobox.SelectedIndex].Cost.ToString();
            WarrantyTextBox.Text = _goodsItems[IDCombobox.SelectedIndex].Warranty.ToString();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Goods goods = _goodsItems[IDCombobox.SelectedIndex];

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
                    File.Delete(goods.Image);

                    string extension = '.' + imagePath.Split('.')[imagePath.Split('.').Length - 1];
                    Directory.CreateDirectory(Config.ImageSourceDir);
                    var dirs = Directory.GetFiles(Config.ImageSourceDir);
                    string filePath = Config.ImageSourceDir + (dirs.Length + 1).ToString() + extension;
                    goods.Image = filePath;
                    File.Copy(imagePath, filePath, true);
                }

                _sqlHelper.UpdateGoods(goods);

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
                MessageBoxResult result = MessageBox.Show("Уверены?", Title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if(result == MessageBoxResult.Yes)
                {
                    _sqlHelper.DeleteGoods(_goodsItems[IDCombobox.SelectedIndex].Id);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
