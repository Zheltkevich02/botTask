using botTask.DataBase;
using botTask.DataBase.Tables;
using botTask.Log;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public ApplicationConnection AC; //Переменная, через которую взаимодействуем с базой!
        BotClass bot; //Класс для взаимодействия с ботом.
        LogWindowClass logWindow;
        public MainWindow()
        {
            InitializeComponent();

            AC = new ApplicationConnection();                       //инициализируем переменную для взаимодействия с БД

            if (!CheckKey())                                        //проверка наличия ключа.
            {
              KeyGrid.Visibility = Visibility.Visible;  
            }

            LVersion.Content = "v"+GlobalSettings.version;     
            SetVersion();                                           //Проверяем и подставляем версию

            if(!Directory.Exists(GlobalSettings.pathDoc))           //Проверка папки в документах, для лога.
            {
                Directory.CreateDirectory(GlobalSettings.pathDoc);
            }

            bot = new BotClass(this);                               //инициализация класса для взаимодействия с ботом
            logWindow = new LogWindowClass(this);                   //инициализация класса для журнала событий

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

        public async System.Threading.Tasks.Task CheckUser(string chatID, string userName)
        {
            try
            {
                // Асинхронно проверяем, существует ли пользователь с заданным ChatID
                var cClient = await AC.Users.FirstOrDefaultAsync(c => c.tgChatID == chatID);

                if (cClient != null)
                {
                    // Пользователь уже существует, выходим из метода
                    return;
                }
                else
                {
                    // Если пользователь не найден, создаем и добавляем нового
                    User user = new User()
                    {
                        nickName = userName,
                        tgChatID = chatID,
                        dateReg = DateTime.Now,
                    };

                    // Добавляем пользователя в контекст и сохраняем изменения асинхронно
                    AC.Users.Add(user);
                    await AC.SaveChangesAsync();

                    // Логгируем успешное добавление пользователя
                    WriteLog("Зарегистрировался пользователь - " + userName);
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок, если что-то пошло не так
                WriteLog("Ошибка при регистрации пользователя - " + userName + ": " + ex.Message);
            }
        }

        public void WriteLog(string text)
        {
            string path = GlobalSettings.pathDoc + "\\LogBot.txt";
            text += " |" + DateTime.Now.ToString();
            text = text.Replace("\n", "");
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            fs.Close();
            using (var w = System.IO.File.AppendText(path))
            {
                w.WriteLine(text);
            }
        }

        public void MainTelegramBotConnect()
        {
            if(!GlobalSettings.BotConnect)
            {
                GlobalSettings.telegramBotName = bot.TelegramBotConnect();
                GlobalSettings.BotConnect=true;
                LStatusBot.Content = GlobalSettings.telegramBotName + " подключен";
                LStatusBot.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(81, 255, 76));
                BConnect.Content = "Выключить";

                WriteLog(GlobalSettings.telegramBotName + " успешно подключен!");
            }
            else
            {
                bot.TelegramBotDisconnect();
                GlobalSettings.BotConnect = false;
                LStatusBot.Content = "Бот отключен";
                LStatusBot.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(212, 52, 52));
                BConnect.Content = "Включить";
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

        private void BConnect_Click(object sender, RoutedEventArgs e)
        {
            MainTelegramBotConnect();
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            var t = (TextBlock)sender;
            if (t.Text.Contains("Зарегистрировался"))
            {
                t.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(81, 255, 76));
            }
            //if (t.Text.Contains("Оповещение статуса для"))
            //{
            //    t.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 239, 70));
            //}
            if (t.Text.Contains("Ошибка") || t.Text.Contains("Проблема") || t.Text.Contains("Сообщение не было отправлено"))
            {
                t.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 75, 75));
            }
        }

        private void TextBlock_Loaded_1(object sender, RoutedEventArgs e)
        {
            var t = (TextBlock)sender;
            string date = DateTime.Now.ToString("dd.MM.yyyy");
            if(t.Text.Contains(date))
            {
                t.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 239, 70));
            }
        }

        private void LStatusBot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(GlobalSettings.BotConnect)
            {
                Clipboard.SetText(@"https://t.me/" + GlobalSettings.telegramBotName);
            }
        }
    }
}
