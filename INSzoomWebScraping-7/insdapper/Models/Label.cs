using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace insdapper.Models
{
    public class Label
    {
        public string Page_id { get; set; }

        public string Label_answer_id { get; set; }
        
        public string Label_text { get; set; }

        public int Formid { get; set; }

    }
}
