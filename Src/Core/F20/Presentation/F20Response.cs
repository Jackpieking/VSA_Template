using System.Text.Json.Serialization;

namespace F20.Presentation;

public sealed class F20Response
{
    [JsonIgnore]
    public int HttpCode { get; set; }

    public int AppCode { get; set; }

    public BodyDto Body { get; set; }

    public sealed class BodyDto { }
}
