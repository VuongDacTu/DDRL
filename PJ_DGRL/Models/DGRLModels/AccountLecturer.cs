using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class AccountLecturer
{
    [DisplayName("Mã giảng viên")]
    public int Id { get; set; }
    [DisplayName("Tên đăng nhập")]
    public string? UserName { get; set; }
    [DisplayName("Mật khẩu")]
    public string? Password { get; set; }
    [DisplayName("Người tạo")]

    public string? CreateBy { get; set; }
    [DisplayName("Ngày tạo")]
    public DateTime? CreateDate { get; set; }
    [DisplayName("Ngày cập nhật")]
    public DateTime? UpdateDate { get; set; }
    [DisplayName("Trạng thái")]
    public byte? IsActive { get; set; }

    public string? LecturerId { get; set; }
    public bool? IsDelete { get; set; }

    public virtual Lecturers? Lecturer { get; set; }
}
