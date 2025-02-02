using FCommon.FeatureService;

namespace F5.Models;

public sealed class F5AppRequestModel : IServiceRequest<F5AppResponseModel>
{
    public long ResetPasswordTokenId { get; set; }

    public long UserId { get; set; }

    public string NewPassword { get; set; }
}
