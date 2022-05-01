using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ClassSchedule.Models;

namespace ClassSchedule.Controllers
{
    public class HomeController : Controller
    {
        private IHttpContextAccessor http { get; set; }
        private IClassScheduleUnitOfWork data { get; set; }

        public HomeController(IClassScheduleUnitOfWork unit, IHttpContextAccessor ctx)
        {
            data = unit;
            http = ctx;
        }

        public ViewResult Index(int id)
        {
            if (id > 0)
            {
                http.HttpContext.Session.SetInt32("dayid", id);
            }

            var dayOptions = new QueryOptions<Day>
            {
                OrderBy = d => d.DayId
            };

            var classOptions = new QueryOptions<Class>
            {
                Includes = "Teacher, Day"
            };

            if (id == 0)
            {
                classOptions.OrderBy = c => c.DayId;
            }
            else
            {
                classOptions.Where = c => c.DayId == id;
                classOptions.OrderBy = c => c.MilitaryTime;
            }

            ViewBag.Days = data.Days.List(dayOptions);
            return View(data.Classes.List(classOptions));
        }
    }
}