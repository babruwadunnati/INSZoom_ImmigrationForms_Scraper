using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZoomScraper.Models;

namespace ZoomScraper.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Change()
        {


            return View(DapperORM.ReturnChange<Change>(("ViewChangeLog")));
        }

        public ActionResult Instructions()
        {
           

            return View("Instructions");
        }

        public ActionResult Forms()
        {
          
            return View("Forms");
        }

        
      
        [HttpPost]
        public ActionResult AddorEdit(InstructionsEdit NewInstruction)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Instruction_type", NewInstruction.Instruction_type);
            param.Add("@Instruction_value", NewInstruction.Instruction_value);
            param.Add("@Form_Id", NewInstruction.Form_Name);
            DapperORM.ExecuteWithoutReturn("InsertPassport", param);
            return RedirectToAction("Instruction");
        }


    }
}