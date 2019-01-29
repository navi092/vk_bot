using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskForTochka.Models
{
    public class GET_VK
    {
        public int owner_id { get; set; }
        public string domain { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public string filter { get; set; }
        public int extended { get; set; }
        public string fields { get; set; }
        public string access_token { get; set; }
        public double v { get; set; }

    }
}
