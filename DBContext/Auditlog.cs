using System;
using System.Collections.Generic;

namespace IOT.DBContext;

public partial class Auditlog
{
    public long Id { get; set; }

    public string Method { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string? QueryString { get; set; }

    public string? RequestBody { get; set; }

    public int StatusCode { get; set; }

    public string? ResponseBody { get; set; }

    public string? IpAddress { get; set; }

    public DateTime RequestedAt { get; set; }

    public double DurationMs { get; set; }
}
