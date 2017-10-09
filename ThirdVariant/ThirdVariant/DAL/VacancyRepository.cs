using System;
using System.Collections.Generic;
using System.Linq;
using ThirdVariant.Models;

namespace ThirdVariant.DAL
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly VacancyContext _context;

        public VacancyRepository(VacancyContext context)
        {
            _context = context;
        }

        public IEnumerable<Vacancy> GetVacancies()
        {
            return _context.Vacancies.ToList();
        }

        public Vacancy GetVacancyById(int vacancyId)
        {
            return _context.Vacancies.Find(vacancyId);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
