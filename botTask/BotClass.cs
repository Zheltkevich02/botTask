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
                    if (message.Text == "/start")
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "Добро пожаловать в botTask!\nВыберите категории.", replyMarkup: GenerateStartButton());
                        await window.CheckUser(message.Chat.Id.ToString(), message.From.Username);
                        return;
                    }
                    else if (message.Text.Contains("/project"))
                    {
                        string name = message.Text.Replace("/project ", "");
                        if (name == "" || name == null)
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, "Вы ничего не ввели..", replyMarkup: GenerateStartButtonTwo());
                            return;
                        }
                        else
                        {
                            int idproject = await window.CreateProject(name, message.Chat.Id.ToString());
                            if(idproject == -1) { return; }

                        }
                    }
                }
                else if(callBack != null)
                {
                    if(callBack.Data == "&start")
                    {
                        await client.SendTextMessageAsync(callBack.From.Id, "Добро пожаловать в botTask!\nВыберите категории.", replyMarkup: GenerateStartButton());
                        return;
                    }
                    if (callBack.Data == "&help")
                    {
                        await client.SendTextMessageAsync(callBack.From.Id, "Помошь!\n");
                        return;
                    }
                    if(callBack.Data == "&createproject")
                    {
                        await client.SendTextMessageAsync(callBack.From.Id, "Введите название для вашего проекта через \"/project [название проекта]\"", replyMarkup: GenerateCreateProjectButton());
                        return;
                    }
                    if(callBack.Data == "&cancel")
                    {
                        await client.DeleteMessageAsync(callBack.From.Id, callBack.Message.MessageId);
                        return;
                    }
                    if(callBack.Data == "&myprojectlist")
                    {
                        await client.SendTextMessageAsync(callBack.From.Id, "Ваш список проектов.", replyMarkup: GenerateMyProject(callBack.From.Id.ToString()));
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
        public InlineKeyboardMarkup GenerateCreateProjectButton()
        {
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Отменить","&cancel"),
                },
            });
            return keyboard;
        }
        public InlineKeyboardMarkup GenerateStartButtonTwo()
        {
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Старт","&start"),
                },
            });
            return keyboard;
        }
        public InlineKeyboardMarkup GenerateMyProject(string tgchatid)
        {
            var rows = new List<InlineKeyboardButton[]>();
            var user = window.AC.Users.Where(c => c.tgChatID == tgchatid).FirstOrDefault();
            var projectRole = window.AC.ProjectRoles.Where(c=>c.IDUser==user.IDUser).ToList();
            
            foreach (var role in projectRole)
            {
                var project = window.AC.Projects.Where(c=>c.IDProject==role.IDProject).FirstOrDefault();
                rows.Add(
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(project.nameProject,"&listProject;"+project.IDProject),
                    }
                );
            }
            return rows.ToArray();
        }
    }
}
