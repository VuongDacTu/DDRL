using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class AccountLecturer
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

    public byte? IsActive { get; set; }

    public string? LecturerId { get; set; }

    public virtual Lecturers? Lecturer { get; set; }
}
