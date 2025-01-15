using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using F5.Src.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace F5.Src.Presentation.Filters.Validation;

public sealed class F5ValidationFilter : IAsyncActionFilter
{
    private readonly IValidator<F5Request> _validator;

    public F5ValidationFilter(IValidator<F5Request> validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var request = context.ActionArguments[F5Constant.REQUEST_ARGUMENT_NAME] as F5Request;

        var result = await _validator.ValidateAsync(request);
        if (!result.IsValid)
        {
            context.Result = new ContentResult
            {
                StatusCode = F5Constant.DefaultResponse.Http.VALIDATION_FAILED.HttpCode,
                Content = JsonSerializer.Serialize(
                    F5Constant.DefaultResponse.Http.VALIDATION_FAILED
                ),
                ContentType = MediaTypeNames.Application.Json,
            };

            return;
        }

        await next();
    }
}
