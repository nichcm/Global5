using FluentValidation.AspNetCore;

namespace Global5.Application.Validation
{
    public class FluentContainer
    {
        public static void ConfigureValidations(FluentValidationMvcConfiguration options)
        {
            options.RegisterValidatorsFromAssemblyContaining<SelectValidation>();
        }
    }
}