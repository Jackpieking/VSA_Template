using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using F11.Common;
using F11.Presentation.Filters.SetStateBag;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace F11.Presentation.Filters.Validation;

public sealed class F11ValidationFilter : IAsyncActionFilter
{
    private readonly IValidator<F11Request> _validator;

    public F11ValidationFilter(IValidator<F11Request> validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var stateBag = context.HttpContext.Items[nameof(F11StateBag)] as F11StateBag;
        var request = stateBag.HttpRequest;

        var result = await _validator.ValidateAsync(request);
        if (!result.IsValid)
        {
            context.Result = new ContentResult
            {
                StatusCode = F11Constant.DefaultResponse.Http.VALIDATION_FAILED.HttpCode,
                Content = JsonSerializer.Serialize(
                    F11Constant.DefaultResponse.Http.VALIDATION_FAILED
                ),
                ContentType = MediaTypeNames.Application.Json,
            };

            return;
        }

        await next();
    }
}
