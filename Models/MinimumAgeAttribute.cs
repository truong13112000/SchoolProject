using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Models
{
   public class MinimumAgeAttribute : ValidationAttribute
    {
        int _miniumAge;
        int _maxAge;

        public MinimumAgeAttribute(int miniumAge, int maxAge)
        {
            _miniumAge = miniumAge;
            _maxAge = maxAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            DateTime date = Convert.ToDateTime(value);
            var minYear = DateTime.Now.AddYears(- _miniumAge);
            var maxYear = DateTime.Now.AddYears(- _maxAge);
            if (date > minYear)
            {
                return new ValidationResult("The Birthday must be less than "+ (DateTime.Now.AddYears(-_miniumAge)).Date.ToString("dd/MM/yyyy"));
            }
            else if (date < maxYear)
            {
                return new ValidationResult("The age must be greater than " + (DateTime.Now.AddYears(- _maxAge)).Date.ToString("dd/MM/yyyy"));
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}