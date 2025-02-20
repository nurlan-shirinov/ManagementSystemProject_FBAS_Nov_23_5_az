﻿using Common.Exceptions;
using Common.GlobalResponses;
using System.Net;
using System.Text.Json;

namespace ManagementSystem.Api.Infrastructure.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            switch (error)
            {
                case BadRequestException:
                    var message = new List<string>() { error.Message };
                    await WriteError(context, HttpStatusCode.BadRequest, message);
                    break;

                case NotFoundException:
                    message= new List<string>() { error.Message };
                    await WriteError(context, HttpStatusCode.NotFound, message);
                    break;

                default:
                    message = new List<string>() { error.Message };
                    await WriteError(context, HttpStatusCode.BadRequest, message);
                    break;
            }
        }
    }

    static async Task WriteError(HttpContext context , HttpStatusCode statusCode , List<string> message)
    {
        context.Response.Clear();
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";

        var option = new JsonSerializerOptions { };
        var json = JsonSerializer.Serialize(new Result(message) , option);
        await context.Response.WriteAsync(json);
    }

}