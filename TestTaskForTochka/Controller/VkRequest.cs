using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskForTochka.Models;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace TestTaskForTochka.Controller
{
    public partial class VkRequest
    {
        private VkApi vk;
        private int myId;
        private string accessToken;
        private Settings access;
        private string firstName;
        private string lastName;
        public virtual string AccessToken { get { return accessToken; } set { accessToken = value; } }
        public virtual int MyId { get { return myId; } set { myId = value; } }
        public virtual Settings Access { get { return access; } set { access = value; } }
        public VkRequest()
        {
            AccessToken = "7e5c36e71a55bf166f5363c19a685ba119e9fd5dc8c4b9164d54b420ac22093bdb8c0f72c6694dfb88c4d";
            MyId = 14873887;
            Access = Settings.Wall;
            vk = new VkApi();
        }
        public virtual bool Authentication()
        {
            try
            {
                vk.Authorize(new ApiAuthParams
                {
                    AccessToken = accessToken,
                    UserId = MyId,
                    Settings = access
                });
                if (vk.IsAuthorized)
                    return true;
                else return false;
            }
            catch (Exception x)
            {
                Program.Message(x.Message);
                return false;
            }

        }

        public virtual IReadOnlyCollection<Post> GetCountPost(int userId)
        {
            IReadOnlyCollection<Post> arrayPost = vk.Wall.Get(new WallGetParams
            {
                OwnerId = userId,
                Offset = 0,
                Count = 5,
                Filter = VkNet.Enums.SafetyEnums.WallFilter.All,
                Extended = true,
                Fields = ""
            }).WallPosts;
            return arrayPost;
        }
        public virtual IReadOnlyCollection<Post> GetCountPost(string domen)
        {
            IReadOnlyCollection<Post> arrayPost = vk.Wall.Get(new WallGetParams
            {
                Domain = domen,
                Offset = 0,
                Count = 5,
                Filter = VkNet.Enums.SafetyEnums.WallFilter.All,
                Extended = true,
                Fields = ""
            }).WallPosts;
            return arrayPost;

        }
        public virtual string StatisticsCalculation(IReadOnlyCollection<Post> arrayPost)
        {
            Dictionary<string, double> dictionaryLetter = new DictionaryLetters().GetDictionaryLetters();
            int countAllLetters = 0;
            for (int i = 0; i < arrayPost.Count; i++)
            {
                // Парсинг своих записей в посте
                arrayPost.ElementAt(i).Text = arrayPost.ElementAt(i).Text.ToLower();
                for (int t = 0; t < arrayPost.ElementAt(i).Text.Length; t++)
                {
                    if (dictionaryLetter.ContainsKey(arrayPost.ElementAt(i).Text[t].ToString()))
                    {
                        dictionaryLetter[arrayPost.ElementAt(i).Text[t].ToString()] = dictionaryLetter[arrayPost.ElementAt(i).Text[t].ToString()] + 1;
                        countAllLetters++;
                    }
                }
                // Парсин комментариев чужих постов у себя на стене
                for (int h = 0; h < arrayPost.ElementAt(i).CopyHistory.Count; h++)
                {
                    arrayPost.ElementAt(i).CopyHistory.ElementAt(h).Text = arrayPost.ElementAt(i).CopyHistory.ElementAt(h).Text.ToLower();

                    for (int y = 0; y < arrayPost.ElementAt(i).CopyHistory.ElementAt(h).Text.Length; y++)
                    {
                        if (dictionaryLetter.ContainsKey(arrayPost.ElementAt(i).CopyHistory.ElementAt(h).Text[y].ToString()))
                        {
                            dictionaryLetter[arrayPost.ElementAt(i).CopyHistory.ElementAt(h).Text[y].ToString()] = dictionaryLetter[arrayPost.ElementAt(i).CopyHistory.ElementAt(h).Text[y].ToString()] + 1;
                            countAllLetters++;
                        }
                    }

                }
            }
            //Очищаем словарь от ненужного
            Dictionary<string, double> dictionaryFinalLetter = new Dictionary<string, double>();
            foreach (var item in dictionaryLetter)
            {
                if (item.Value > 0)
                {
                    dictionaryFinalLetter.Add(item.Key, (item.Value / countAllLetters));
                }
            }
            string exitLetter = "";
            int flag = 0;
            // подсчет статистики
            foreach (var item in dictionaryFinalLetter)
            {
                if (flag == 0)

                    exitLetter += firstName + " " + lastName + ", статистика для последних 5 постов: {\"" + item.Key + "\": " + item.Value.ToString("0.0000") + ", ";
                else
                {
                    if (flag != dictionaryFinalLetter.Count - 1)
                        exitLetter += "\"" + item.Key + "\": " + item.Value.ToString("0.0000") + ", ";
                    else
                        exitLetter += "\"" + item.Key + "\": " + item.Value.ToString("0.0000") + "}";
                }
                flag++;
            }
            return exitLetter;
        }
        public virtual bool SenderPostVK(string post)
        {
            try
            {
                long f = vk.Wall.Post(new WallPostParams
                {
                    Message = post,
                    OwnerId = MyId
                });
                if (f > 0)
                {
                    ConsolePublicStatistik(post);
                    return true;
                }
                else
                    Program.Message("Нет данных для подсчета статистики");
                return false;
            }
            catch (Exception x)
            {
                Program.Message(x.Message);
                return false;
            }
        }
        public virtual void ConsolePublicStatistik(string post)
        {
            Program.Message(post);
        }
        public virtual bool GetFullName(string useIdOrDomen)

        {
            try
            {
                var g = vk.Users.Get(new string[]
                {
                       useIdOrDomen
                });
                if (g != null || g.Count != 0)
                {
                    firstName = g.FirstOrDefault().FirstName;
                    lastName = g.FirstOrDefault().LastName;
                    return true;
                }
                else
                {
                    var t = vk.Groups.GetById(null, useIdOrDomen, null).FirstOrDefault();
                    if (t != null)
                    {
                        firstName = t.Name;
                        return true;
                    }
                    else
                    {
                        Program.Message("Пользователя/сообщества с таким доменом или id (" + useIdOrDomen + ") не существует!");
                        return false;
                    }
                    
                }
            }
            catch (Exception h)
            {
                try
                {
                    var t = vk.Groups.GetById(null, useIdOrDomen, null).FirstOrDefault();
                    if (t != null)
                    {
                        firstName = t.Name;
                        return true;
                    }
                    else
                    {
                        Program.Message("Пользователя/сообщества с таким доменом или id (" + useIdOrDomen + ") не существует!");
                        return false;
                    }
                   
                }
                catch (Exception)
                {
                    Program.Message("Пользователя/сообщества с таким доменом или id (" + useIdOrDomen + ") не существует!");
                    return false;
                }               
            }
        }
    }
}

