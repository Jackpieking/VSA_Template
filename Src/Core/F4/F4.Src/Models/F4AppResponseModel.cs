using F4.Src.Common;
using FCommon.Src.FeatureService;

namespace F4.Src.Models;

public sealed class F4AppResponseModel : IServiceResponse
{
    public F4Constant.AppCode AppCode { get; set; }

    public BodyModel Body { get; set; }

    public sealed class BodyModel
    {
        public string ResetPasswordToken { get; set; }
    }
}
