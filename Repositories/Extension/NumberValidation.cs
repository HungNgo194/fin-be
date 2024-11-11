using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Extension
{
    public class PositiveNumberValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int intValue && intValue <= 0)
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be a positive number.");
            }
            return ValidationResult.Success;
        }
    }
}
