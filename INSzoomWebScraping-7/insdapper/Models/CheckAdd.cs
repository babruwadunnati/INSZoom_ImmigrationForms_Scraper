using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace insdapper.Models
{
    public class CheckAdd
    {
        public string Change_time { get; set; }

        public string Change_type { get; set; }

        public string Old_id { get; set; }

        public string New_id { get; set; }

        public string Page_id { get; set; }

        public string Option_value { get; set; }

        public int Formid { get; set; }
    }
}
