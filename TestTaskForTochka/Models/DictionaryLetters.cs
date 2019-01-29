using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskForTochka.Models
{
    public class DictionaryLetters
    {
        public virtual Dictionary<string, double> GetDictionaryLetters()
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            string word = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            for (int i = 0; i < word.Length; i++)
            {
                dictionary.Add(word[i].ToString(), 0.0);
            }
            return dictionary;
        }
    }
}
