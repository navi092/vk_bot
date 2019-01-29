using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestTaskForTochka.Models;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using TestTaskForTochka.Controller;
namespace TestTaskForTochka
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Введите ID аккаунта vk, например (id1, 1, durov,  public147415323, tech)");
                string readEnterUser = Console.ReadLine().ToLower().Replace(" ", "");
                if (readEnterUser.Replace(" ", "") != "")
                {
                    int Id = new VerificationEntryLine().GetId(readEnterUser); // проверяем входную строку на число

                    VkRequest vkRequest = new VkRequest();
                    if (vkRequest.Authentication())
                    {
                        if (Id != 0)
                        {
                            if (vkRequest.GetFullName(Id.ToString()))  //Проверка на существование аккаунта
                            {
                                if (vkRequest.SenderPostVK(vkRequest.StatisticsCalculation(vkRequest.GetCountPost(Id))))
                                    Message("Статистика опубликована");
                            }
                        }
                        else
                        {
                            if (vkRequest.GetFullName(readEnterUser))  //Проверка на существование аккаунта
                            {
                                if (vkRequest.SenderPostVK(vkRequest.StatisticsCalculation(vkRequest.GetCountPost(readEnterUser))))
                                    Message("Статистика опубликована");
                            }
                        }
                    }
                    else
                        Message("Ошибка при авторизации пользователя с ID: " + vkRequest.MyId);
                    Console.WriteLine(new String('*', 60));
                }
                else
                    Message("Повторите попытку"); 
                
            }
        }
        public static void Message(string msg)
        {
            Console.WriteLine(msg);
        }
    }

}

