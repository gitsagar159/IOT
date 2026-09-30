using System;
using System.Collections.Generic;

namespace IOT.DBContext;

public partial class SoilReading
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public string? Moisturevalue { get; set; }

    public ulong? Isactive { get; set; }

    public ulong? Isdelete { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Updateddate { get; set; }

    public int? Updatedby { get; set; }

    public string? Ipaddress { get; set; }

    public virtual Usersmaster? User { get; set; }
}
