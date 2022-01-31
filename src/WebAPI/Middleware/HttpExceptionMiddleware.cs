using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.DTOs.Common;
using System;
using Serilog;
using Serilog.Events;
using System.Collections.Generic;
using Serilog.Parsing;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
using System.IO;

namespace WebAPI.Middleware
{
    internal class HttpExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public HttpExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);
            }
            catch (HttpException exception)
            {
                context.Response.StatusCode = exception.StatusCode;
                context.Response.ContentType = "application/json";

                context.Response.WriteAsync(new ErrorDetails
                {
                    Message = exception.Message
                }.ToString());
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                context.Response.WriteAsync(new ErrorDetails
                {
                    Message = exception.StackTrace
                }.ToString());
            }
        }
    }
}
