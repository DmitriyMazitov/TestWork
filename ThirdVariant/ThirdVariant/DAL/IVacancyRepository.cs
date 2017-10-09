using System;
using System.Collections.Generic;
using ThirdVariant.Models;

namespace ThirdVariant.DAL
{
    public interface IVacancyRepository : IDisposable
    {
        IEnumerable<Vacancy> GetVacancies();
        Vacancy GetVacancyById(int vacancyId);
    }
}
