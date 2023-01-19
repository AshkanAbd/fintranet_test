using System.Net;

namespace Application.Common.Response;

public static class StdResponseFactory
{
    public static StdResponse<TValue> Ok<TValue>(object? data = default, string? message = null) where TValue : class =>
        new(HttpStatusCode.OK, message, data);

    public static StdResponse<TValue> OkMsg<TValue>(string? message = null) where TValue : class =>
        new(HttpStatusCode.OK, message);

    public static StdResponse<TValue> BadRequest<TValue>(object? data = default, string? message = null) where TValue : class =>
        new(HttpStatusCode.BadRequest, message, data);

    public static StdResponse<TValue> BadRequestMsg<TValue>(string? message = null) where TValue : class =>
        new(HttpStatusCode.BadRequest, message);

    public static StdResponse<TValue> NotFound<TValue>(object? data = default, string? message = null) where TValue : class =>
        new(HttpStatusCode.NotFound, message, data);

    public static StdResponse<TValue> NotFoundMsg<TValue>(string? message = null) where TValue : class =>
        new(HttpStatusCode.NotFound, message);

    public static StdResponse<TValue> PermissionDenied<TValue>(object? data = default, string? message = null) where TValue : class =>
        new(HttpStatusCode.Forbidden, message, data);

    public static StdResponse<TValue> PermissionDeniedMsg<TValue>(string? message = null) where TValue : class =>
        new(HttpStatusCode.Forbidden, message);

    public static StdResponse<TValue> NotAuth<TValue>(object? data = default, string? message = null) where TValue : class =>
        new(HttpStatusCode.Unauthorized, message, data);

    public static StdResponse<TValue> NotAuthMsg<TValue>(string? message = null) where TValue : class =>
        new(HttpStatusCode.Unauthorized, message);

    public static StdResponse<TValue> ValidationError<TValue>(object? data = default, string? message = null) where TValue : class =>
        new(HttpStatusCode.UnprocessableEntity, message, data);

    public static StdResponse<TValue> ValidationErrorMsg<TValue>(string? message = null) where TValue : class =>
        new(HttpStatusCode.UnprocessableEntity, message);

    public static StdResponse<TValue> InternalError<TValue>(object? data = default, string? message = null) where TValue : class =>
        new(HttpStatusCode.InternalServerError, message, data);

    public static StdResponse<TValue> InternalErrorMsg<TValue>(string? message = null) where TValue : class =>
        new(HttpStatusCode.InternalServerError, message);
}