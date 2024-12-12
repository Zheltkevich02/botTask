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
        public List<ChatClass> chatClass;
        public BotClass(MainWindow _window)
        {
            window = _window;
            chatClass = new List<ChatClass>();
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
                    if (message.Text == "/start" || message.Text == "📋 Главное меню")
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "Добро пожаловать в botTask!\nВыберите категории.", replyMarkup: GenerateStartButton());

                        var curMes = await client.SendTextMessageAsync(message.Chat.Id, "🛠 Все кнопки успешно обновлены!", replyMarkup: GetMainReply());

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
                            else
                            {
                                await GetProjectList(client, message.Chat.Id, "🔔 Проект успешно создан!\n\n");
                                return;
                            }

                        }
                    }
                    else
                    {
                        var currentChat = chatClass.Where(c => c.chatID == message.Chat.Id).FirstOrDefault();
                        if (currentChat != null)
                        {
                            if(currentChat.callbackString.Contains("&editNameProject;"))
                            {
                                int idProject = Convert.ToInt32(currentChat.callbackString.Split(';')[1]);
                                
                                if(await window.EditNameProject(idProject, message.Text) == true)
                                {
                                    string mainText = "🔔 Наименование проекта успешно изменено!\n";
                                    await SendInfoZakaz(client, message.Chat.Id, idProject, mainText);
                                    return;
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(message.Chat.Id, "Произошла ошибка, обратитесь к администратору!", replyMarkup: GenerateStartButtonTwo());
                                    return;
                                }
                            }
                            if(currentChat.callbackString.Contains("&editDisProject;"))
                            {
                                int idProject = Convert.ToInt32(currentChat.callbackString.Split(';')[1]);

                                if (await window.EditDisProject(idProject, message.Text) == true)
                                {
                                    string mainText = "🔔 Описание проекта успешно изменено!\n";
                                    await SendInfoZakaz(client, message.Chat.Id, idProject, mainText);
                                    return;
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(message.Chat.Id, "Произошла ошибка, обратитесь к администратору!", replyMarkup: GenerateStartButtonTwo());
                                    return;
                                }
                            }
                        }
                        else
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, "Добро пожаловать в botTask!\nВыберите категории.", replyMarkup: GenerateStartButton());

                            var curMes = await client.SendTextMessageAsync(message.Chat.Id, "🛠 Все кнопки успешно обновлены!", replyMarkup: GetMainReply());

                            await window.CheckUser(message.Chat.Id.ToString(), message.From.Username);
                            return;
                        }
                    }
                }
                else if(callBack != null)
                {
                    var currentChat = chatClass.Where(c=>c.chatID == callBack.From.Id).FirstOrDefault();
                    if(currentChat!=null)
                    {
                        currentChat.callbackString = callBack.Data;
                        if (callBack.Data.Contains("&myprojectlist")) currentChat.countPageProject = 1;
                    }
                    else
                    {
                        ChatClass curChat = new ChatClass(callBack.From.Id, callBack.Data);
                        if (callBack.Data.Contains("&myprojectlist")) curChat.countPageProject = 1;
                        chatClass.Add(curChat);
                    }

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
                        await GetProjectList(client, callBack.From.Id);
                        return;
                    }
                    if(callBack.Data.Contains("&listProject;"))
                    {
                        int idProject = Convert.ToInt32(callBack.Data.Split(';')[1]);

                        await SendInfoZakaz(client, callBack.From.Id, idProject);
                    }
                    if(callBack.Data.Contains("&editProject;"))
                    {
                        string idProject = callBack.Data.Split(';')[1];

                        await client.SendTextMessageAsync(callBack.From.Id, "📝 Выберите категорию", replyMarkup: GenerateEditProjectButton(idProject));
                        return;
                    }
                    if (callBack.Data.Contains("&editNameProject;"))
                    {
                        await client.SendTextMessageAsync(callBack.From.Id, "Напишите новое название проекта 👇");
                        return;
                    }
                    if(callBack.Data.Contains("&editDisProject;"))
                    {
                        await client.SendTextMessageAsync(callBack.From.Id, "Напишите описание для проекта 👇");
                        return;
                    }
                    if(callBack.Data.Contains("&delProject;"))
                    {
                        string idProject = callBack.Data.Split(';')[1];

                        await client.SendTextMessageAsync(callBack.From.Id, "Вы уверены, что хотите удалить проект?", replyMarkup: GenerateDelProjectButton(idProject));
                        return;
                    }
                    if(callBack.Data.Contains("&yesDelProject;"))
                    {
                        int idProject = Convert.ToInt32(callBack.Data.Split(';')[1]);

                        string saveName = await window.DelProject(idProject);

                        if (saveName!="")
                        {
                            await GetProjectList(client, callBack.From.Id, $"🔔 Проект \"{saveName}\" успешно удален!\n\n");
                            return;
                        }
                        else
                        {
                            await client.SendTextMessageAsync(callBack.From.Id, "Произошла ошибка, обратитесь к администратору!", replyMarkup: GenerateStartButtonTwo());
                            return;
                        }

                    }
                    if(callBack.Data == "&nextPageProject")
                    {
                        currentChat.countPageProject++;
                        await GetProjectList(client, callBack.From.Id, "", callBack.Message.MessageId);
                        return;
                    }
                    if(callBack.Data == "&prevPageProject")
                    {
                        currentChat.countPageProject--;
                        await GetProjectList(client, callBack.From.Id, "", callBack.Message.MessageId);
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

        public async Task SendInfoZakaz(ITelegramBotClient client, long id, int idProject, string mainText="")
        {
            string mes="";
            var project = window.AC.Projects.Where(c => c.IDProject == idProject).FirstOrDefault();

            mes += $"Проект №{project.IDProject}\n";
            mes += $"<b>{project.nameProject}</b>";
            mes += "\n\n";
            if (project.discription != "") mes += $"📒 {project.discription}\n";
            mes += $"🗓 Дата создания проекта: {project.dateCreate}";

            await client.SendTextMessageAsync(id, mainText+mes, replyMarkup: GenerateMenuProjectButton(project.IDProject.ToString()), parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            return;
        }

        Task ErrorBot(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public async Task GetProjectList(ITelegramBotClient client, long chatID, string mainText = "", int messageId = 0)
        {
            if (messageId == 0)
            {
                await client.SendTextMessageAsync(chatID, mainText + "Ваш список проектов.", replyMarkup: GenerateMyProject(chatID.ToString()));
                return;
            }
            else
            {
                await client.EditMessageTextAsync(chatID, messageId, mainText + "Ваш список проектов.", replyMarkup: GenerateMyProject(chatID.ToString()));
                return;
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
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

        public InlineKeyboardMarkup GenerateMenuProjectButton(string idProject)
        {
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("🗒 Список задач",$"&mytasklist;{idProject}"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("👥 Участники", $"&projectUsers;{idProject}"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("📝 Редактировать проект", $"&editProject;{idProject}"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("🗳 Удалить проект", $"&delProject;{idProject}"),
                },
            });
            return keyboard;
        }

        public InlineKeyboardMarkup GenerateDelProjectButton(string idProject)
        {
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("✔️ Да",$"&yesDelProject;{idProject}"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("❌ Нет", $"&noDelProject"),
                },
            });
            return keyboard;
        }

        public InlineKeyboardMarkup GenerateEditProjectButton(string idProject)
        {
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Переименовать проект",$"&editNameProject;{idProject}"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Редактировать описание проекта", $"&editDisProject;{idProject}"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Указать статус проекта", $"&editStatusProject;{idProject}"),
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
            long chatId=Convert.ToInt32(tgchatid);
            var rows = new List<InlineKeyboardButton[]>();
            var user = window.AC.Users.Where(c => c.tgChatID == tgchatid).FirstOrDefault();
            var projectRole = window.AC.ProjectRoles.Where(c=>c.IDUser==user.IDUser).ToList();
            var userChat = chatClass.Where(c => c.chatID == chatId).FirstOrDefault();

            int startIndex = (userChat.countPageProject * 5)-5;
            int endIndex = startIndex + 5;
            if(projectRole.Count < endIndex) endIndex = projectRole.Count;

            
            for (int i = startIndex; i < endIndex; i++)
            {
                int idProject = projectRole[i].IDProject;
                var project = window.AC.Projects.Where(c => c.IDProject == idProject).FirstOrDefault();
                if (project != null)
                {
                    rows.Add(
                        new[]
                        {
                        InlineKeyboardButton.WithCallbackData(project.nameProject,"&listProject;"+project.IDProject),
                        }
                    );
                }
                if (i+1 == endIndex)
                {
                    if (endIndex != projectRole.Count)
                    {
                        rows.Add(
                            new[]
                            {
                              InlineKeyboardButton.WithCallbackData("Следующая страница ⏩","&nextPageProject"),
                            }
                        );
                    }
                    if (userChat.countPageProject > 1)
                    {
                        rows.Add(
                            new[]
                            {
                                  InlineKeyboardButton.WithCallbackData("Предыдущая страница ⏪","&prevPageProject"),
                            }
                        );
                    }
                    break;
                }
            }
            return rows.ToArray();
        }

        public ReplyKeyboardMarkup GetMainReply()
        {
            var reply = new ReplyKeyboardMarkup(new KeyboardButton("📋 Главное меню"))
            {
                ResizeKeyboard = true
            };
            return reply;
        }
    }
}
