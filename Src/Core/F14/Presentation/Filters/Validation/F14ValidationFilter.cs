using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using F14.Common;
using F14.Presentation.Filters.SetStateBag;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace F14.Presentation.Filters.Validation;

public sealed class F14ValidationFilter : IAsyncActionFilter
{
    private readonly IValidator<F14Request> _validator;

    public F14ValidationFilter(IValidator<F14Request> validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var stateBag = context.HttpContext.Items[nameof(F14StateBag)] as F14StateBag;
        var request = stateBag.HttpRequest;

        var result = await _validator.ValidateAsync(request);
        if (!result.IsValid)
        {
            context.Result = new ContentResult
            {
                StatusCode = F14Constant.DefaultResponse.Http.VALIDATION_FAILED.HttpCode,
                Content = JsonSerializer.Serialize(
                    F14Constant.DefaultResponse.Http.VALIDATION_FAILED
                ),
                ContentType = MediaTypeNames.Application.Json,
            };

            return;
        }

        await next();
    }
}
