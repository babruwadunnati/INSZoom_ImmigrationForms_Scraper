using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZoomScraper.Models
{
    public class Change
    {
        public string Change_time { get; set; }

        public string Change_type { get; set; }

        public string Old_id { get; set; }

        public string New_id { get; set; }

        public string Page_id { get; set; }
    }
}