using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThirdVariant.Models
{
    [Table("v_get_vacany_body")]
    public class Vacancy
    {
        [Key]
        public int id { get; set; }
        public string vacancy_id { get; set; }
        [Display(Name = "Vacancy Name")]
        public string vacancy_name { get; set; }
        public string description { get; set; }
        public string schedule { get; set; }
        public string experience { get; set; }
        public string department { get; set; }
        public string employment { get; set; }
        public string salary_to { get; set; }
        public string salary_from { get; set; }
        public string salary_currency { get; set; }
        public string area_name { get; set; }
        public string employer_name { get; set; }
        public DateTime published_at { get; set; }

    }
}
