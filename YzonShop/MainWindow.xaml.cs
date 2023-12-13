using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YzonShop.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace YzonShop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _authCount = 0;
        public MainWindow()
        {
            InitializeComponent();
        }


        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window test = new ClientWindow(1);
                test.Show();
                this.Close();
                return;

                SQLHelper sqlHelper = SQLHelper.GetSQLHelper();

                var appointment = sqlHelper.Login(LoginTextBox.Text, PasswordBox.Password);

                if (CaptchaGrid.Visibility == Visibility.Visible)
                {
                    int answer = -1;
                    Int32.TryParse(CaptchaAnswerTextBox.Text, out answer);

                    if (a + b != answer)
                    {
                        MessageBox.Show("Неверная captcha", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        CaptchaInit();
                        return;
                    }

                }

                int access = 1;

                Window window = null;
                switch (appointment[0])
                {
                    case "Пользователь":
                        //thread = new Thread(Run => Application.Run(new Director(sqlHelper)));
                        break;
                    case "Менеджер":
                        window = new ManagerWindow();
                        break;
                    case "Доставщик":
                        //thread = new Thread(Run => Application.Run(new Collector(sqlHelper)));
                        break;
                    case "Администратор":
                        window = new AdministratorWindow();
                        break;
                    default:
                        access = 0;
                        MessageBox.Show("Отказано в доступе", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        _authCount++;
                        CaptchaInit();
                        if (_authCount == 1) CaptchaGrid.Visibility = Visibility.Visible;
                        if (_authCount == 2)
                        {
                            LoginButton.IsEnabled = false;
                            CaptchaGrid.Visibility = Visibility.Hidden;
                            TimerGrid.Visibility = Visibility.Visible;
                            await Task.Run(Timer);
                            CaptchaGrid.Visibility = Visibility.Visible;
                            TimerGrid.Visibility = Visibility.Hidden;
                            LoginButton.IsEnabled = true;
                        }
                        if (_authCount >= 3)
                        {
                            LoginButton.IsEnabled = false;
                            CaptchaGrid.Visibility = Visibility.Hidden;
                            BlockedGrid.Visibility = Visibility.Visible;
                        }
                        sqlHelper.SaveAuthLog(LoginTextBox.Text, access);
                        return;
                }
                sqlHelper.SaveAuthLog(LoginTextBox.Text, access);
                window.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int a;
        private int b;

        private void Timer()
        {            
            Thread.Sleep(180000);
        }

        private void CaptchaInit()
        {
            
            LoginButton.Margin = new Thickness(226, 326, 323, 0);
            Random random = new Random();
            a = random.Next(0, 100);
            b = random.Next(0, 100);

            CaptchaQuestionLabel.Content = $"{a} + {b} =";
        }
    }
}
