using System;
using System.Collections.Generic;

namespace IOT.DBContext;

public partial class Usersmaster
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public string UserName { get; set; } = null!;

    public string MobileNumber { get; set; } = null!;

    public ulong? Isactive { get; set; }

    public ulong? Isdelete { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ClientMaster Client { get; set; } = null!;
}
