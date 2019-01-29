using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestTaskForTochka.Controller
{
   public class VerificationEntryLine
    {
        public virtual int GetId(string Id)
        {
            if (Id[0] == 'i' && Id[1] == 'd' && Char.IsDigit(Id[2]))
                return Convert.ToInt32(Id.Substring(2, Id.Length - 2));

            if (Regex.IsMatch(Id, @"^\d+$"))
                return Convert.ToInt32(Id);

            return 0;
        }

    }
}
