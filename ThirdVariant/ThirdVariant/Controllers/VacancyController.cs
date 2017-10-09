using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

//using Sakura.AspNetCore;
using ThirdVariant.DAL;
using ThirdVariant.Models;

namespace ThirdVariant.Controllers
{
    public class VacancyController : Controller
    {
        private IVacancyRepository vacancyRepository;

        public VacancyController()
        {
            vacancyRepository = new VacancyRepository(new VacancyContext());
        }


        public ViewResult Index(string sortOrder, string currentFilter, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                Vars.GetVacanciesFromHh();

            }
                    
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            
            ViewBag.CurrentFilter = searchString;

            var vacancies = vacancyRepository.GetVacancies().Select(s => s).Take(15);

            if (!String.IsNullOrEmpty(searchString))
            {
                vacancies = vacancies.Where(s => s.vacancy_name.ToUpper().Contains(searchString.ToUpper()));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    vacancies = vacancies.OrderByDescending(s => s.vacancy_name);
                    break;
                case "Date":
                    vacancies = vacancies.OrderBy(s => s.published_at);
                    break;
                case "date_desc":
                    vacancies = vacancies.OrderByDescending(s => s.published_at);
                    break;
                case "employer_name":
                    vacancies = vacancies.OrderByDescending(s => s.employer_name);
                    break;
                default:  // Name ascending 
                    vacancies = vacancies.OrderBy(s => s.vacancy_name);
                    break;
            }

            return View(vacancies);
        }

        public ViewResult Details(int id)
        {
            Vacancy vacancy = vacancyRepository.GetVacancyById(id);
            return View(vacancy);
        }
    }
}