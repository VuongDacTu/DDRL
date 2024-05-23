using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Students
{
    [DisplayName("Mã sinh viên")]
    public string Id { get; set; } = null!;

    [DisplayName("Họ và tên")]
    public string? FullName { get; set; }
    [DisplayName("Ngày sinh")]
    public DateOnly? Birthday { get; set; }

    public string? Email { get; set; }
    [DisplayName("Số điện thoại")]
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
