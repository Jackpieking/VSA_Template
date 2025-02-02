using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using F6.Common;
using F6.Presentation.Filters.SetStateBag;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace F6.Presentation.Filters.Validation;

public sealed class F6ValidationFilter : IAsyncActionFilter
{
    private readonly IValidator<F6Request> _validator;

    public F6ValidationFilter(IValidator<F6Request> validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var stateBag = context.HttpContext.Items[nameof(F6StateBag)] as F6StateBag;
        var request = stateBag.HttpRequest;

        var result = await _validator.ValidateAsync(request);
        if (!result.IsValid)
        {
            context.Result = new ContentResult
            {
                StatusCode = F6Constant.DefaultResponse.Http.VALIDATION_FAILED.HttpCode,
                Content = JsonSerializer.Serialize(
                    F6Constant.DefaultResponse.Http.VALIDATION_FAILED
                ),
                ContentType = MediaTypeNames.Application.Json,
            };

            return;
        }

        await next();
    }
}
