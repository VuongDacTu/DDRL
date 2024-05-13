using System;
using System.Collections.Generic;

namespace PJ_DGRL.Models.DGRLModels;

public partial class AccountStudent
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public byte? IsActive { get; set; }

    public string? StudentId { get; set; }

    public virtual Students? Student { get; set; }
}
