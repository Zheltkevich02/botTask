using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask
{
    public class GlobalSettings
    {
        public static string nameApplication = "botTask"; //название приложения
        public static int version = 2;
        public static string telegramKey; //ключ с ботом
        public static string pathDoc= "C:\\Users\\Public\\Documents\\botTask";

        public static bool BotConnect = false;
        public static string telegramBotName = "";
    }
}
