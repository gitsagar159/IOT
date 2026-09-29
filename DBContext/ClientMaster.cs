using System;
using System.Collections.Generic;

namespace IOT.DBContext;

public partial class ClientMaster
{
    public int Id { get; set; }

    public string ClientCode { get; set; } = null!;

    public string? ClientName { get; set; }

    public ulong? IsActive { get; set; }

    public ulong? IsDelete { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<Usersmaster> Usersmasters { get; set; } = new List<Usersmaster>();
}
