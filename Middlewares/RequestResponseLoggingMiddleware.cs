using IOT.DBContext;
using IOT.Models;
using System.Diagnostics;
using System.Text;

namespace IOT.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var auditLog = new Auditlog
            {
                RequestedAt = DateTime.UtcNow,
                Method = context.Request.Method,
                Path = context.Request.Path,
                QueryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : null,
                IpAddress = context.Connection.RemoteIpAddress?.ToString()
            };

            // 1. Read and buffer Request Body
            auditLog.RequestBody = await ReadRequestBodyAsync(context.Request);

            // 2. Intercept Response Stream
            var originalResponseBodyStream = context.Response.Body;
            using var responseBodyMemoryStream = new MemoryStream();
            context.Response.Body = responseBodyMemoryStream;

            try
            {
                await _next(context);

                // 3. Read Response details
                auditLog.StatusCode = context.Response.StatusCode;
                auditLog.ResponseBody = await ReadResponseBodyAsync(context.Response);

                // Copy stream back to the client
                responseBodyMemoryStream.Position = 0;
                await responseBodyMemoryStream.CopyToAsync(originalResponseBodyStream);
            }
            finally
            {
                context.Response.Body = originalResponseBodyStream;
                stopwatch.Stop();
                auditLog.DurationMs = stopwatch.ElapsedMilliseconds;

                // 4. Save to Database
                await SaveAuditLogToDbAsync(context, auditLog);
            }
        }

        private async Task<string?> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();

            if (request.ContentLength > 0 && request.Body.CanRead)
            {
                request.Body.Position = 0;
                using var reader = new StreamReader(
                    request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024,
                    leaveOpen: true);

                var bodyText = await reader.ReadToEndAsync();
                request.Body.Position = 0; // Reset position for Controllers/Model Binding
                return bodyText;
            }

            return null;
        }

        private async Task<string?> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Position = 0;
            using var reader = new StreamReader(
                response.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true);

            var bodyText = await reader.ReadToEndAsync();
            response.Body.Position = 0; // Reset position for CopyToAsync
            return bodyText;
        }

        private async Task SaveAuditLogToDbAsync(HttpContext context, Auditlog log)
        {
            try
            {
                // Resolve the DbContext created by Database-First scaffolding
                var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>();

                dbContext.Auditlogs.Add(log);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception internally so database errors do not crash API responses
                _logger.LogError(ex, "Error saving HTTP audit log to MySQL.");
            }
        }
    }
}
