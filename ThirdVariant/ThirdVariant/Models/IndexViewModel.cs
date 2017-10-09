using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThirdVariant.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Vacancy> Vacancies { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
