using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZoomScraper.Models
{
    public class InstructionsEdit
    {
        public int Instruction_Id { get; set; }


        [Required(ErrorMessage = "Enter Instruction type")]
        public string Instruction_type { get; set; }

        [Required(ErrorMessage = "Enter Instruction Value")]
        public string Instruction_value { get; set; }

        [Required(ErrorMessage = "Select Form")]
        public string Form_Name { get; set; }

    }
}