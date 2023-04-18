﻿using System.Net.Mail;
using System.Text.Json;
using Bugsnag;
using Microsoft.AspNetCore.Http;

namespace IdentityService.Exceptions
{
    internal class HttpExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IClient bugsnag;

        public HttpExceptionMiddleware(RequestDelegate next, IClient bugsnag)
        {
            this.next = next;
            this.bugsnag = bugsnag;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (HttpException httpException)
            {
                var body = new { httpException.StatusCode, httpException?.Message };
                var res = context.Response;
                res.StatusCode = httpException.StatusCode;
                res.ContentType = "application/json; charset=utf-8";
                await res.WriteAsync(JsonSerializer.Serialize(body));
            }
            catch (Exception exception)
            {
                bugsnag.Notify(exception);
                var body = new { Message = "Internal server error occured.", StatusCode = 500 };
                var res = context.Response;
                res.StatusCode = 500;
                res.ContentType = "application/json; charset=utf-8";
                await res.WriteAsync(JsonSerializer.Serialize(body));
            }
        }
    }
}
