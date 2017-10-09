using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ThirdVariant.Models;

namespace ThirdVariant.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        //private IVacancyRepository vacancyRepository;

        //public HomeController()
        //{
        //    this.vacancyRepository = new VacancyRepository(new VacancyContext());
        //}

        //public HomeController(IVacancyRepository vacancyRepository)
        //{
        //    this.vacancyRepository = vacancyRepository;
        //}


        ////GET: /Vacancy/

        //public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        //{
        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }
        //    ViewBag.CurrentFilter = searchString;

        //    var vacancies = from s in vacancyRepository.GetVacancies()
        //                    select s;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        vacancies = vacancies.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
        //    }
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            vacancies = vacancies.OrderByDescending(s => s.Name);
        //            break;
        //        case "Date":
        //            vacancies = vacancies.OrderBy(s => s.PublishedAt);
        //            break;
        //        case "date_desc":
        //            vacancies = vacancies.OrderByDescending(s => s.PublishedAt);
        //            break;
        //        default:  // Name ascending 
        //            vacancies = vacancies.OrderBy(s => s.Name);
        //            break;
        //    }

        //    int pageSize = 3;
        //    int pageNumber = (page ?? 1);
        //    return View(vacancies.ToPagedList(pageNumber, pageSize));
        //}

        ////public ViewResult Details(int id)
        ////{
        ////    Vacancy vacancy = vacancyRepository.GetVacancyById(id);
        ////    return View(vacancy);
        ////}
    }
}
