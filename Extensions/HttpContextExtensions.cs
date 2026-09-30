namespace IOT.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetClientIpAddress(this HttpContext context)
        {
            if (context == null) return string.Empty;

            // 1. Check for X-Forwarded-For header (useful if behind Nginx/Cloudflare/Reverse Proxy)
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                var ipList = forwardedFor.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (ipList.Length > 0)
                {
                    return ipList[0].Trim(); // The first IP in the list is the original client
                }
            }

            // 2. Fall back to direct RemoteIpAddress
            var remoteIp = context.Connection.RemoteIpAddress;
            if (remoteIp == null) return string.Empty;

            // Convert IPv4-mapped IPv6 address (e.g., ::ffff:192.168.1.50) to standard IPv4
            if (remoteIp.IsIPv4MappedToIPv6)
            {
                remoteIp = remoteIp.MapToIPv4();
            }

            string ipString = remoteIp.ToString();

            // Convert IPv6 loopback to IPv4 loopback for local testing
            return ipString == "::1" ? "127.0.0.1" : ipString;
        }
    }
}
