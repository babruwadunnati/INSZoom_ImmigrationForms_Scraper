using Dapper;
using ZoomScraper.Models;
using System.Web.Mvc;

namespace ZoomScraper.Controllers
{
    public class InstructionEditController : Controller
    {

      
        [HttpPost]
        public ActionResult AddorEdit(InstructionsEdit InstructionsEdit)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Instruction_type", InstructionsEdit.Instruction_type);
            param.Add("@Instruction_value", InstructionsEdit.Instruction_value);
            param.Add("@Form_Id", InstructionsEdit.Form_Name);
            DapperORM.ExecuteWithoutReturn("InsertPassport", param);
            return RedirectToAction("Instruction");
        }
    }
}