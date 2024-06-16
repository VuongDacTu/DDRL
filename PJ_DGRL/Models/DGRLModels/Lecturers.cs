using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Lecturers
{
    [DisplayName("Mã giảng viên")]
    [Required(ErrorMessage = "Mã giảng viên không được trống")]
    [MaxLength(10,ErrorMessage ="Độ dài không được quá 10 ký tự")]
    public string Id { get; set; } = null!;
    [DisplayName("Họ và tên")]
    [Required(ErrorMessage = "Họ và tên không được trống")]
    [MaxLength(50,ErrorMessage ="Độ dài không được quá 50 ký tự")]
    public string? FullName { get; set; }
    [DisplayName("Chuyên ngành")]
    public int? DepartmentId { get; set; }
    [DisplayName("Ngày sinh")]
    public DateOnly? Birthday { get; set; }
    [StringLength(50,ErrorMessage ="Độ dài không được quá 50 ký tự")]
    public string? Email { get; set; }
    [DisplayName("Số điện thoại")]
    [MaxLength(15, ErrorMessage ="Độ dài không được quá 15 ký tự")]
    public string? Phone { get; set; }
    [DisplayName("Trạng thái")]
    public byte? IsActive { get; set; }

    public bool? IsDelete { get; set; }
    [DisplayName("Chức vụ")]
    public int? PositionId { get; set; }
    [DisplayName("Vị trí trong hội đồng")]
    public byte? IsLeader { get; set; }
    [DisplayName("Học vấn")]
    [MaxLength(30,ErrorMessage = "Độ dài không được quá 30 ký tự")]
    public string? Education { get; set; }
    public virtual ICollection<AccountLecturer> AccountLecturers { get; set; } = new List<AccountLecturer>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<LecturerInfor> LecturerInfors { get; set; } = new List<LecturerInfor>();

    public virtual Position? Position { get; set; }
}
