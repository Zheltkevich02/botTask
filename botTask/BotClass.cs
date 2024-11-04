using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace botTask
{
    public class BotClass
    {
        private MainWindow window;
        public CancellationTokenSource cts;
        public TelegramBotClient Bot;
        public BotClass(MainWindow _window)
        {
            window = _window;
        }

        public string TelegramBotConnect()
        {
            cts = new CancellationTokenSource();
            Bot = new TelegramBotClient(GlobalSettings.telegramKey);

            var infoBot = Bot.GetMeAsync();

            Bot.StartReceiving(UpdateBot, ErrorBot, cancellationToken: cts.Token);
            return infoBot.Result.Username;
        }

        public void TelegramBotDisconnect()
        {
            cts.Cancel();
        }

        public async Task UpdateBot(ITelegramBotClient client, Update update, CancellationToken token)
        {
            try
            {
                var message = update.Message;
                var callBack = update.CallbackQuery;
                if (message != null)
                {
                    if(message.Text=="/start")
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "Добро пожаловать в botTask!\nВыберите категории.", replyMarkup: GenerateStartButton());
                        await window.CheckUser(message.Chat.Id.ToString(), message.From.Username);
                        return;
                    }
                }
                else if(callBack != null)
                {
                    if (callBack.Data == "&help")
                    {
                        await client.SendTextMessageAsync(callBack.From.Id, "Помошь!\n");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception e)
            {
                window.WriteLog("Ошибка из телеграм. " + e.Message);
                return;
            }
        }

        Task ErrorBot(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public InlineKeyboardMarkup GenerateStartButton()
        {
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Мои проекты","&myprojectlist"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Создать проект", "&createproject"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Настройки", "&usersettigns"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Помощь", "&help"),
                },
            });
            return keyboard;
        }
    }
}
