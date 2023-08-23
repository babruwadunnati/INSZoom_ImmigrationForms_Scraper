using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace insdapper.Models
{
    public class Option
    {
        public string Page_id { get; set; }

        public string Label_answer_id { get; set; }

        public string Option_text { get; set; }

        public string Option_value { get; set; }

        public int Formid { get; set; }
    }
}
