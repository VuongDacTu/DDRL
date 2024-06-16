using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Students
{
    [Required(ErrorMessage = "Mã sinh viên không được trống")]
    [DisplayName("Mã sinh viên")]
    [MaxLength(10,ErrorMessage ="Độ dài không vượt quá 10 ký tự")]
    public string Id { get; set; } = null!;

    [DisplayName("Họ và tên")]
    [Required(ErrorMessage = "Họ và tên không được trống")]
    [MaxLength(50,ErrorMessage ="Độ dài không vượt quá 50 ký tự")]
    public string? FullName { get; set; }
    [DisplayName("Ngày sinh")]
    public DateOnly? Birthday { get; set; }
    [MaxLength(50,ErrorMessage ="Độ dài không vượt quá 50 ký tự")]
    public string? Email { get; set; }
    [DisplayName("Số điện thoại")]
    [MaxLength(15,ErrorMessage ="Độ dài không vượt quá 15 ký tự")]
    public string? Phone { get; set; }
    [DisplayName("Giới tính")]
    public bool? Gender { get; set; }
    [DisplayName("Lớp")]
    public int? ClassId { get; set; }
    [DisplayName("Trạng thái")]
    public byte? IsActive { get; set; }

    public bool? IsDelete { get; set; }
    [DisplayName("Chức vụ")]
    public int? PositionId { get; set; }

    public virtual ICollection<AccountStudent> AccountStudents { get; set; } = new List<AccountStudent>();

    public virtual Class? Class { get; set; }

    public virtual ICollection<ClassAnswer> ClassAnswers { get; set; } = new List<ClassAnswer>();

    public virtual Position? Position { get; set; }

    public virtual ICollection<SelfAnswer> SelfAnswers { get; set; } = new List<SelfAnswer>();

    public virtual ICollection<SumaryOfPoint> SumaryOfPoints { get; set; } = new List<SumaryOfPoint>();
}
