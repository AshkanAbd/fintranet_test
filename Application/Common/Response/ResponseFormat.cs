using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Application.Common.Response;

public static class ResponseFormat
{
    public static JsonResult Base<T>(StdResponse<T> stdResponse) where T : class
    {
        return new JsonResult(stdResponse) {
            StatusCode = (int) stdResponse.Status,
        };
    }

    public static JsonResult Base<T>(HttpStatusCode status = HttpStatusCode.OK, string? msg = null, T? data = default,
        long? logId = null) where T : class
    {
        return new JsonResult(new StdResponse<T> {
            Status = status,
            Message = msg,
            Data = data
        }) {
            StatusCode = (int) status,
        };
    }

    public static JsonResult Ok<T>(T? data = default, string? msg = "Success", long? logId = null) where T : class
    {
        return Base(HttpStatusCode.OK, msg, data, logId);
    }

    public static JsonResult OkMsg<T>(string? msg = "Success", T? data = default, long? logId = null) where T : class
    {
        return Ok(data, msg, logId);
    }

    public static JsonResult NotFound<T>(T? data = default, string? msg = "NotFound", long? logId = null)
        where T : class
    {
        return Base(HttpStatusCode.NotFound, msg, data, logId);
    }

    public static JsonResult NotFoundMsg<T>(string? msg = "NotFound", T? data = default, long? logId = null)
        where T : class
    {
        return NotFound(data, msg, logId);
    }

    public static JsonResult PermissionDenied<T>(T? data = default, string? msg = "Forbidden", long? logId = null)
        where T : class
    {
        return Base(HttpStatusCode.Forbidden, msg, data, logId);
    }

    public static JsonResult PermissionDeniedMsg<T>(string? msg = "Forbidden", T? data = default, long? logId = null)
        where T : class
    {
        return PermissionDenied(data, msg, logId);
    }

    public static JsonResult NotAuth<T>(T? data = default, string? msg = "UnAuthorized", long? logId = null)
        where T : class
    {
        return Base(HttpStatusCode.Unauthorized, msg, data, logId);
    }

    public static JsonResult NotAuthMsg<T>(string? msg = "UnAuthorized", T? data = default,
        long? logId = null) where T : class
    {
        return NotAuth(data, msg, logId);
    }

    public static JsonResult BadRequest<T>(T? data = default, string? msg = "BadRequest", long? logId = null)
        where T : class
    {
        return Base(HttpStatusCode.BadRequest, msg, data, logId);
    }

    public static JsonResult BadRequestMsg<T>(string? msg = "BadRequest", T? data = default, long? logId = null)
        where T : class
    {
        return BadRequest(data, msg, logId);
    }

    public static JsonResult InternalError<T>(T? data = default, string? msg = "ServerInternalError",
        long? logId = null) where T : class
    {
        return Base(HttpStatusCode.InternalServerError, msg, data, logId);
    }

    public static JsonResult InternalErrorMsg<T>(string? msg = "ServerInternalError", T? data = default,
        long? logId = null) where T : class
    {
        return InternalError(data, msg, logId);
    }

    public static JsonResult ValidationError<T>(T? data = default, string? msg = "ValidationError", long? logId = null)
        where T : class
    {
        return Base(HttpStatusCode.InternalServerError, msg, data, logId);
    }

    public static JsonResult ValidationErrorMsg<T>(string? msg = "ValidationError", T? data = default,
        long? logId = null) where T : class
    {
        return InternalError(data, msg, logId);
    }
}