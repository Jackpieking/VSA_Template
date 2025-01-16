using System.Threading;
using System.Threading.Tasks;
using F6.Src.BusinessLogic;
using F6.Src.Common;
using F6.Src.Mapper;
using F6.Src.Models;
using F6.Src.Presentation.Filters.Authorization;
using F6.Src.Presentation.Filters.Validation;
using FCommon.Src.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F6.Src.Presentation;

public sealed class F6Endpoint : ControllerBase
{
    private readonly F6Service _service;

    public F6Endpoint(F6Service service)
    {
        _service = service;
    }

    [HttpPost(F6Constant.ENDPOINT_PATH)]
    [Authorize(Policy = nameof(F6AuthorizationRequirement))]
    [ServiceFilter<F6ValidationFilter>(Order = 1)]
    public async Task<IActionResult> ExecuteAsync(
        [FromBody] F6Request request,
        CancellationToken ct
    )
    {
        var stateBag = HttpContext.Items[AppConstants.STATE_BAG_NAME] as F6StateBag;

        var appRequest = new F6AppRequestModel
        {
            AccessTokenId = stateBag.AccessTokenId,
            RefreshToken = request.RefreshToken,
        };
        var appResponse = await _service.ExecuteAsync(appRequest, ct);

        var httpResponse = F6HttpResponseMapper.Get(appRequest, appResponse);

        return StatusCode(httpResponse.HttpCode, httpResponse);
    }
}
