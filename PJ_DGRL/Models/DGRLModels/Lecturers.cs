using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Lecturers
{
    [DisplayName("Mã giảng viên")]
    public string Id { get; set; } = null!;
    [DisplayName("Họ và tên")]
    public string? FullName { get; set; }
    [DisplayName("Mã chuyên ngành")]
    public int? DepartmentId { get; set; }

    [DisplayName("Mã chức vụ")]
    public string? PositionId { get; set; }
    [DisplayName("Ngày sinh")]
    public DateOnly? Birthday { get; set; }

    public string? Email { get; set; }
    [DisplayName("Số điện thoại")]
    public string? Phone { get; set; }
    [DisplayName("Trạng thái")]
    public byte? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<AccountLecturer> AccountLecturers { get; set; } = new List<AccountLecturer>();

    public virtual Department? Department { get; set; }

    public virtual Position? Position { get; set; }
}
