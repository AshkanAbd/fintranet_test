using System.Net;

namespace Application.Common.Response;

public class StdResponse<TValue> where TValue : class
{
    protected internal StdResponse(HttpStatusCode status = HttpStatusCode.OK, string? message = null,
        object? data = default)
    {
        Status = status;
        Message = message;
        Data = data;
    }

    public object? Data { get; set; }
    public TValue? DataStruct { get; set; }
    public HttpStatusCode Status { get; set; }
    public string? Message { get; set; }

    public TValue? DataAsDataStruct() => Data as TValue;
}