using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class AccountAdmin
{
    public int Id { get; set; }
    [DisplayName("Họ và tên")]
    public string? FullName { get; set; }
    [DisplayName("Tài khoản")]
    public string? UserName { get; set; }
    [DisplayName("Mật khẩu")]
    public string? Password { get; set; }
    [DisplayName("Người tạo")]
    public string? CreateBy { get; set; }
    [DisplayName("Ngày tạo")]
    public DateTime? CreateDate { get; set; }
    [DisplayName("Ngày cập nhật")]
    public DateTime? UpdateDate { get; set; }

    public byte? IsActive { get; set; }
    [DisplayName("Quyền")]
    public int? RoleId { get; set; }
    [DisplayName("Ghi nhớ")]
    public bool? Remember { get; set; }

    public virtual Role? Role { get; set; }
}
