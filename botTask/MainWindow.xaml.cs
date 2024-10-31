using botTask.DataBase;
using botTask.DataBase.Tables;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;

namespace botTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationConnection AC; //Переменная, через которую взаимодействуем с базой!
        public MainWindow()
        {
            InitializeComponent();

            AC = new ApplicationConnection();

            if (!CheckKey())
            {
              KeyGrid.Visibility = Visibility.Visible;
            }

            LVersion.Content = "v"+GlobalSettings.version;
            SetVersion();

        }
        public bool CheckKey() //Проверка наличия ключа от телеграм бота
        {
            var settings = AC.Settings.ToList();
            if (settings.Count != 0)
            {
                if(settings[0].telegramKey.Length > 0)
                {
                    GlobalSettings.telegramKey = settings[0].telegramKey;
                    return true;
                }
                else
                { return false; }
            }
            else
            {
                return false;
            }
        }

        public void SetVersion()
        {
            var settings = AC.Settings.ToList();
            if (settings.Count != 0)
            {
                if (settings[0].Ver>GlobalSettings.version)
                {
                    MessageBox.Show("Вы открыли старую версию программы", GlobalSettings.nameApplication, MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(0);
                }
                else if (settings[0].Ver == GlobalSettings.version)
                {
                    return;
                }
                else
                {
                    settings[0].Ver = GlobalSettings.version;
                    AC.SaveChanges();
                }
            }
            else
            {
                Setting setting = new Setting()
                {
                    Ver = GlobalSettings.version,
                };
                AC.Settings.Add(setting);
                AC.SaveChanges();
            }
        }

        public bool SetKey(string key)
        {
            if (key.Length > 0)
            {
                if (key.Length == 46)
                {
                    var settings = AC.Settings.ToList();
                    if (settings.Count != 0)
                    {
                        settings[0].telegramKey = key;
                    }
                    else
                    {
                        Setting setting = new Setting()
                        {
                            telegramKey = key,
                        };
                        AC.Settings.Add(setting);
                    }
                    AC.SaveChanges();
                    
                    return true;
                }
                else
                {
                    MessageBox.Show("Неверный формат ключа", GlobalSettings.nameApplication, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Вы ничего не ввели!", GlobalSettings.nameApplication, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e) //Вставка ключа в базу данных
        {
            if(SetKey(TBKey.Text)) KeyGrid.Visibility= Visibility.Hidden;
        }

        private void BSettings_Click(object sender, RoutedEventArgs e)
        {
            if(GSettings.Visibility == Visibility.Hidden)
            {
                GSettings.Visibility = Visibility.Visible;
            }
            else
            {
                GSettings.Visibility = Visibility.Hidden;
            }
        }
    }
}
