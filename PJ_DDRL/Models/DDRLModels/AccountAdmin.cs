using System;
using System.Collections.Generic;

namespace PJ_DDRL.Models.DDRLModels;

public partial class AccountAdmin
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public byte? IsActive { get; set; }

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }
}
