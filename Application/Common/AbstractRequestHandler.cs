using Application.Common.Response;
using MediatR;

namespace Application.Common;

public abstract class AbstractRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken _);

    protected StdResponse<T> Ok<T>(T? data = default, string msg = "Success") where T : class
    {
        return StdResponseFactory.Ok<T>(data, msg);
    }

    protected StdResponse<T> OkMsg<T>(string msg = "Success") where T : class
    {
        return StdResponseFactory.OkMsg<T>(msg);
    }

    protected StdResponse<T> NotFound<T>(object? data = default, string msg = "NotFound") where T : class
    {
        return StdResponseFactory.NotFound<T>(data, msg);
    }

    protected StdResponse<T> NotFoundMsg<T>(string? msg = "NotFound") where T : class
    {
        return StdResponseFactory.NotFoundMsg<T>(msg);
    }

    protected StdResponse<T> PermissionDenied<T>(object? data = default, string msg = "Forbidden") where T : class
    {
        return StdResponseFactory.PermissionDenied<T>(data, msg);
    }

    protected StdResponse<T> PermissionDeniedMsg<T>(string msg = "Forbidden") where T : class
    {
        return StdResponseFactory.PermissionDeniedMsg<T>(msg);
    }

    protected StdResponse<T> NotAuth<T>(object? data = default, string msg = "UnAuthorized") where T : class
    {
        return StdResponseFactory.NotAuth<T>(data, msg);
    }

    protected StdResponse<T> NotAuthMsg<T>(string msg = "UnAuthorized") where T : class
    {
        return StdResponseFactory.NotAuthMsg<T>(msg);
    }

    protected StdResponse<T> BadRequest<T>(object? data = default, string msg = "BadRequest") where T : class
    {
        return StdResponseFactory.BadRequest<T>(data, msg);
    }

    protected StdResponse<T> BadRequestMsg<T>(string msg = "BadRequest") where T : class
    {
        return StdResponseFactory.BadRequestMsg<T>(msg);
    }

    protected StdResponse<T> ValidationError<T>(object? data = default, string msg = "ValidationError") where T : class
    {
        return StdResponseFactory.ValidationError<T>(data, msg);
    }

    protected StdResponse<T> ValidationErrorMsg<T>(string msg = "ValidationError") where T : class
    {
        return StdResponseFactory.ValidationErrorMsg<T>(msg);
    }

    protected StdResponse<T> InternalError<T>(object? data = default, string msg = "ServerInternalError")
        where T : class
    {
        return StdResponseFactory.InternalError<T>(data, msg);
    }

    protected StdResponse<T> InternalErrorMsg<T>(string msg = "ServerInternalError") where T : class
    {
        return StdResponseFactory.InternalErrorMsg<T>(msg);
    }
}