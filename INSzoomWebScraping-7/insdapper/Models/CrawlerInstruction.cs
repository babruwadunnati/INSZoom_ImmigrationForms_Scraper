using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace insdapper.Models
{
    public class CrawlerInstruction
    {
        public int FormId { get; set; }

        public string InstructionType { get; set; }

        public string InstructionValue { get; set; }

        public int id { get; set; }
    }
}
