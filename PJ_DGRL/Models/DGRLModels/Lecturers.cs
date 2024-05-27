using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Lecturers
{
    [DisplayName("Mã giảng viên")]
    [Required(ErrorMessage = "Mã giảng viên không được trống")]
    public string Id { get; set; } = null!;
    [DisplayName("Họ và tên")]
    [Required(ErrorMessage = "Họ và tên không được trống")]
    public string? FullName { get; set; }

    public int? DepartmentId { get; set; }
    [DisplayName("Ngày sinh")]
    public DateOnly? Birthday { get; set; }

    public string? Email { get; set; }
    [DisplayName("Số điện thoại")]
    public string? Phone { get; set; }
    [DisplayName("Trạng thái")]
    public byte? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public int? PositionId { get; set; }

    public byte? IsLeader { get; set; }
    public string? Education { get; set; }
    public virtual ICollection<AccountLecturer> AccountLecturers { get; set; } = new List<AccountLecturer>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<LecturerInfor> LecturerInfors { get; set; } = new List<LecturerInfor>();

    public virtual Position? Position { get; set; }
}
