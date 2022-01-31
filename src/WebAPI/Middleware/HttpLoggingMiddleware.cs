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
    internal class HttpLoggingMiddleware
    {
        private readonly RequestDelegate next;

        public HttpLoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            long start = Stopwatch.GetTimestamp();
            var requestBody = await GetRequestBodyAsync(context);
            var originalResponseBody = context.Response.Body;
            string responseBody = default;

            using (var memStream = new MemoryStream())
            {
                context.Response.Body = memStream;
                await this.next.Invoke(context);

                memStream.Position = 0;
                responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();

                memStream.Position = 0;
                await memStream.CopyToAsync(originalResponseBody);
            }

            context.Response.Body = originalResponseBody;

            await this.LogCompletion(
                context,
                context.Response.StatusCode,
                GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()),
                requestBody,
                responseBody,
                LogEventLevel.Information,
                (Exception)null);
        }

        private async Task LogCompletion(
            HttpContext context,
            int statusCode,
            double elapsedMs,
            string requestBody,
            string responseBody,
            LogEventLevel level,
            Exception exception)
        {
            var messageTemplate = new MessageTemplateParser()
                .Parse(GetMessageTemplate(requestBody, responseBody));

            var properties = new List<LogEventProperty>
            {
                new LogEventProperty("RequestMethod", (LogEventPropertyValue) new ScalarValue((object) context.Request.Method)),
                new LogEventProperty("RequestPath", (LogEventPropertyValue) new ScalarValue((object) GetPath(context))),
                new LogEventProperty("StatusCode", (LogEventPropertyValue) new ScalarValue((object) statusCode)),
                new LogEventProperty("Elapsed", (LogEventPropertyValue) new ScalarValue((object) elapsedMs)),
                new LogEventProperty("RequestBody", (LogEventPropertyValue) new ScalarValue((object) requestBody)),
                new LogEventProperty("ResponseBody", (LogEventPropertyValue) new ScalarValue((object) responseBody))

            };

            var logEvent = new LogEvent(DateTimeOffset.Now, level, exception, messageTemplate, properties);
            Log.Write(logEvent);
        }

        private static string GetMessageTemplate(string requestBody, string responseBody)
        {
            var template = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            if (!string.IsNullOrWhiteSpace(requestBody))
            {
                template += "\r\n Request body: {RequestBody}";
            }

            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                template += "\r\n Response body: {ResponseBody}";
            }

            return template;
        }

        private static double GetElapsedMilliseconds(long start, long stop) => (double)((stop - start) * 1000L) / (double)Stopwatch.Frequency;

        private static string GetPath(HttpContext httpContext)
        {
            string rawTarget = httpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;
            if (string.IsNullOrEmpty(rawTarget))
                rawTarget = httpContext.Request.Path.ToString();
            return rawTarget;
        }

        private static async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            var body = context.Request.Body;
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            string requestBody = Encoding.UTF8.GetString(buffer);

            body.Seek(0, SeekOrigin.Begin);
            context.Request.Body = body;

            return requestBody;
        }
    }
}
