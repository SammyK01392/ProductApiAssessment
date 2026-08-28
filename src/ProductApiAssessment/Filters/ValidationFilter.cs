using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductApiAssessment.Filters
{
    public class ValidationFilter<T> : IAsyncActionFilter where T : class
    {
        private readonly IValidator<T> _validator;

        public ValidationFilter(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var arg = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

            if (arg != null)
            {
                var result = await _validator.ValidateAsync(arg);
                if (!result.IsValid)
                {
                    var errors = result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                    context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(new
                    {
                        statusCode = 400,
                        message = "Validation failed.",
                        errors
                    });
                    return;
                }
            }

            await next();
        }
    }
}
