using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
public class ApiException(int status, string message, Dictionary<string, string[]>? errors = null) : Exception(message)
{
    public int Status { get; } = status;
    public Dictionary<string, string[]> Errors { get; } = errors ?? [];
    public static ApiException Field(string field, string message, int status = 400) => new(status, message, new() { [field] = [message] });
}
public class ApiErrors(RequestDelegate next, ILogger<ApiErrors> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try { await next(context); }
        catch (Exception e)
        {
            var error = e switch
            {
                ApiException a => a,
                DbUpdateConcurrencyException => new ApiException(409, "Dữ liệu đã thay đổi ở cửa sổ khác. Hãy tải lại trang."),
                DbUpdateException { InnerException: SqliteException { SqliteErrorCode: 19 } } => new ApiException(409, "Dữ liệu trùng hoặc vi phạm ràng buộc cơ sở dữ liệu."),
                BadHttpRequestException => new ApiException(400, "Dữ liệu gửi lên không hợp lệ."),
                _ => new ApiException(500, "Không thể hoàn tất thao tác. Vui lòng thử lại.")
            };
            if (error.Status == 500) logger.LogError(e, "Request failed: {Trace}", context.TraceIdentifier);
            context.Response.StatusCode = error.Status;
            await context.Response.WriteAsJsonAsync(new { error = error.Message, errors = error.Errors, traceId = context.TraceIdentifier });
        }
    }
}
public sealed class WriteGate
{
    private readonly SemaphoreSlim gate = new(1, 1);
    public async Task<IDisposable> Enter() { await gate.WaitAsync(); return new Release(gate); }
    private sealed class Release(SemaphoreSlim gate) : IDisposable { public void Dispose() => gate.Release(); }
}
