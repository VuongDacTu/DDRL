using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class AccountStudent
{
    public int Id { get; set; }
    [DisplayName("Tài khoản")]
    public string? UserName { get; set; }
    [DisplayName("Mật khẩu")]
    public string? Password { get; set; }
    [DisplayName("Thay đổi bởi")]
    public string? CreateBy { get; set; }
    [DisplayName("Ngày tạo")]
    public DateTime? CreateDate { get; set; }
    [DisplayName("Ngày cập nhật")]
    public DateTime? UpdateDate { get; set; }
    [DisplayName("Tài khoản")]
    public byte? IsActive { get; set; }

    public string? StudentId { get; set; }

    public virtual Students? Student { get; set; }
}
