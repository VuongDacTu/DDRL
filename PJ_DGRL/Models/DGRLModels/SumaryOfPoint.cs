using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class SumaryOfPoint
{
    public int Id { get; set; }
    
    public string? StudentId { get; set; }

    public int? SemesterId { get; set; }
    [DisplayName("Điểm tự đánh giá")]
    public int? SelfPoint { get; set; }
    [DisplayName("Điểm cán bộ lớp đánh giá")]
    public int? ClassPoint { get; set; }
    [DisplayName("Điểm giảng viên đánh giá")]

    public int? LecturerPoint { get; set; }
    [DisplayName("Xếp loại")]

    public string? Classify { get; set; }
    [DisplayName("Cán bộ lớp")]

    public string? UserClass { get; set; }
    [DisplayName("Giảng viên")]
    public string? UserLecturer { get; set; }
    [DisplayName("Ngày cập nhật")]
    public DateTime? UpdateDate { get; set; }

    public int? ClassId { get; set; }

    public double? LastPoint { get; set; }

    public virtual Class? Class { get; set; }

    public virtual Semester? Semester { get; set; }

    public virtual Students? Student { get; set; }
}
