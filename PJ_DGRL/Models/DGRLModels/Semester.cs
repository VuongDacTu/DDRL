using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PJ_DGRL.Models.DGRLModels;

public partial class Semester
{
    public int Id { get; set; }
    [DisplayName("Kì học")]
    public string? Name { get; set; }
    [DisplayName("Năm học")]

    public string? SchoolYear { get; set; }
    [DisplayName("Ngày bắt đầu đánh giá")]

    public DateTime? DateOpenStudent { get; set; }
    [DisplayName("Ngày kết thúc đánh giá cho sinh viên")]

    public DateTime? DateEndStudent { get; set; }
    [DisplayName("Ngày kết thúc đánh giá cho cán bộ lớp")]

    public DateTime? DateEndClass { get; set; }
    [DisplayName("Ngày kết thúc đánh giá cho giảng viên")]

    public DateTime? DateEndLecturer { get; set; }

    public byte? IsActive { get; set; }

    public virtual ICollection<QuestionHisory> QuestionHisories { get; set; } = new List<QuestionHisory>();

    public virtual ICollection<SelfAnswer> SelfAnswers { get; set; } = new List<SelfAnswer>();

    public virtual ICollection<SumaryOfPoint> SumaryOfPoints { get; set; } = new List<SumaryOfPoint>();
}
