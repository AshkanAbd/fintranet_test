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

    public static JsonResult Ok<T>(T? data = default, string? msg = null, long? logId = null) where T : class
    {
        return Base(HttpStatusCode.OK, msg, data, logId);
    }

    public static JsonResult OkMsg<T>(string? msg = null, T? data = default, long? logId = null) where T : class
    {
        return Ok(data, msg, logId);
    }

    public static JsonResult NotFound<T>(T? data = default, string msg = "داده پیدا نشد.", long? logId = null)
        where T : class
    {
        return Base(HttpStatusCode.NotFound, msg, data, logId);
    }

    public static JsonResult NotFoundMsg<T>(string msg = "داده پیدا نشد.", T? data = default, long? logId = null)
        where T : class
    {
        return NotFound(data, msg, logId);
    }

    public static JsonResult PermissionDenied<T>(T? data = default, string? msg = null, long? logId = null)
        where T : class
    {
        return Base(HttpStatusCode.Forbidden, msg, data, logId);
    }

    public static JsonResult PermissionDeniedMsg<T>(string? msg = null, T? data = default, long? logId = null)
        where T : class
    {
        return PermissionDenied(data, msg, logId);
    }

    public static JsonResult NotAuth<T>(T? data = default, string msg = "لطفا وارد سیستم شوید.", long? logId = null)
        where T : class
    {
        return Base(HttpStatusCode.Unauthorized, msg, data, logId);
    }

    public static JsonResult NotAuthMsg<T>(string msg = "لطفا وارد سیستم شوید.", T? data = default,
        long? logId = null) where T : class
    {
        return NotAuth(data, msg, logId);
    }

    public static JsonResult BadRequest<T>(T? data = default, string? msg = null, long? logId = null) where T : class
    {
        return Base(HttpStatusCode.BadRequest, msg, data, logId);
    }

    public static JsonResult BadRequestMsg<T>(string? msg = null, T? data = default, long? logId = null) where T : class
    {
        return BadRequest(data, msg, logId);
    }

    public static JsonResult InternalError<T>(T? data = default, string? msg = null, long? logId = null) where T : class
    {
        return Base(HttpStatusCode.InternalServerError, msg, data, logId);
    }

    public static JsonResult InternalErrorMsg<T>(string? msg = null, T? data = default, long? logId = null)
        where T : class
    {
        return InternalError(data, msg, logId);
    }
}