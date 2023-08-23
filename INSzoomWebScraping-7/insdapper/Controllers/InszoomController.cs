using Microsoft.AspNetCore.Mvc;
using insdapper.Models;

namespace insdapper.Controllers
{
    public class InszoomController : Controller
    {
        public IActionResult Index()
        {
           
            Inszoom.Webscrape();
            return View(DapperORM.Return_list<CheckAdd>("ViewChangeLog"));
            //return View();
        }
                   


    }

}